using System.Collections.Generic;
using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Gameplay.Enemies.Systems;
using Gameplay.Weapons.BulletWeapon;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.Physics;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class PhysicsWorld
    {
        private readonly CollisionDetector _detector;
        private readonly CollisionResolver _resolver;
        private readonly BoundaryWrapper _boundary;
        private readonly SignalBus _signalBus;
        private readonly List<(IEnemy enemy, PhysicsBody body)> _laserHits = new List<(IEnemy, PhysicsBody)>();
        private readonly List<PhysicsBody> _bodies = new List<PhysicsBody>();

        private PhysicsBody _shipBody;
        private readonly LazyInject<IShip> _ship;

        public PhysicsWorld(
            CollisionDetector detector,
            CollisionResolver resolver,
            BoundaryWrapper boundary,
            SignalBus signalBus,
            LazyInject<IShip> ship)
        {
            _detector = detector;
            _resolver = resolver;
            _boundary = boundary;
            _signalBus = signalBus;
            _ship = ship;
        }

        public void Initialize() {}

        public PhysicsBody CreateBody(object owner, Vector2 position, float radius, PhysicsLayer layer)
        {
            var body = new PhysicsBody(owner, position, radius, layer);
            _bodies.Add(body);
            return body;
        }

        public void RegisterShipBody(PhysicsBody body)
        {
            _shipBody = body;
        }

        public void RemoveBody(PhysicsBody body)
        {
            if (body == null) return;
            body.Deactivate();
            _bodies.Remove(body);
        }

        public void Tick(float delta)
        {
            UpdateBodies(delta);
            WrapBodies();
            ResolveCollisions();
        }

        private void UpdateBodies(float delta)
        {
            foreach (var body in _bodies)
                body.Tick(delta);
        }

        private void WrapBodies()
        {
            foreach (var body in _bodies)
                body.SetPosition(_boundary.Wrap(body.Position));
        }

        private void ResolveCollisions()
        {
            var pairs = _detector.Detect(_bodies);

            foreach (var pair in pairs)
            {
                if (!pair.A.IsActive || !pair.B.IsActive) continue;

                HandleCollision(pair);
            }
        }

        private void HandleCollision(CollisionDetector.CollisionPair pair)
        {
            var layerA = pair.A.Layer;
            var layerB = pair.B.Layer;

            if ((layerA == PhysicsLayer.Ship   && layerB == PhysicsLayer.Enemy) ||
                (layerA == PhysicsLayer.Enemy  && layerB == PhysicsLayer.Ship))
            {
                if (_ship.Value.IsInvincible) return;

                HandleShipCollision(pair);
                return;
            }

            if ((layerA == PhysicsLayer.Bullet && layerB == PhysicsLayer.Enemy) ||
                (layerA == PhysicsLayer.Enemy  && layerB == PhysicsLayer.Bullet))
            {
                HandleBulletCollision(pair);
                return;
            }
        }
        
        private void HandleBulletCollision(CollisionDetector.CollisionPair pair)
        {
            var bulletBody = pair.A.Layer == PhysicsLayer.Bullet ? pair.A : pair.B;
            var enemyBody  = pair.A.Layer == PhysicsLayer.Enemy  ? pair.A : pair.B;

            bulletBody.Deactivate();
            enemyBody.Deactivate();

            if (bulletBody.Owner is Bullet bullet)
                bullet.Deactivate();

            if (enemyBody.Owner is IEnemy enemy)
                enemy.TakeDamage();
        }

        public void CheckLaserCollisions(Laser laser, EnemyPool enemyPool)
        {
            if (!laser.IsActive) return;

            var origin = laser.Origin;
            var direction = new Vector2(
                Mathf.Sin(laser.Angle * Mathf.Deg2Rad),
                Mathf.Cos(laser.Angle * Mathf.Deg2Rad));

            _laserHits.Clear();

            foreach (var (enemy, view) in enemyPool.Active)
            {

                if (!enemy.IsActive) continue;
                if (view.PhysicsBody == null) continue;

                var hit = RayIntersectsCircle(
                    origin, direction,
                    laser.Length,
                    laser.Radius,
                    enemy.Position,
                    view.PhysicsBody.Radius);


                if (hit)
                    _laserHits.Add((enemy, view.PhysicsBody));
            }

            foreach (var (enemy, body) in _laserHits)
            {
                body.Deactivate();
                enemy.TakeDamage();
            }
        }

        private bool RayIntersectsCircle(Vector2 origin, Vector2 direction, float length,
            float rayRadius, Vector2 circleCenter, float circleRadius)
        {
            var toCircle = circleCenter - origin;
            var projectionLength = Vector2.Dot(toCircle, direction);
            var closestPoint = origin + direction * projectionLength;
            var distanceToCircle = Vector2.Distance(closestPoint, circleCenter);

            if (projectionLength < 0 || projectionLength > length)
                return false;

            return distanceToCircle <= (circleRadius + rayRadius);
        }

        private void HandleShipCollision(CollisionDetector.CollisionPair pair)
        {
            var shipBody = pair.A == _shipBody ? pair.A : pair.B;
            var enemyBody = pair.A == _shipBody ? pair.B : pair.A;

            var (shipVel, enemyVel) = _resolver.Resolve(shipBody, enemyBody);

            shipBody.SetVelocity(shipVel);
            enemyBody.SetVelocity(enemyVel);

            SeparateBodies(shipBody, enemyBody);

            _signalBus.Fire(new ShipCollisionSignal
                {
                    HitVelocity = shipVel,
                    EnemyOwner = enemyBody.Owner,
                    EnemyVelocity = enemyVel
                });
        }

        private void SeparateBodies(PhysicsBody a, PhysicsBody b)
        {
            var normal = (b.Position - a.Position).normalized;
            var overlap = (a.Radius + b.Radius)
                          - Vector2.Distance(a.Position, b.Position);
            var half = overlap * 0.5f;

            a.SetPosition(a.Position - normal * half);
            b.SetPosition(b.Position + normal * half);
        }
    }
}
