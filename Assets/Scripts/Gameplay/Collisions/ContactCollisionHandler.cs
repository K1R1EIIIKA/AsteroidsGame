using System.Collections.Generic;
using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Gameplay.Weapons.BulletWeapon;
using Infrastructure.Physics;
using UnityEngine;
using Zenject;

namespace Gameplay.Collisions
{
    public class ContactCollisionHandler
    {
        private readonly CollisionDetector _detector;
        private readonly CollisionResolver _resolver;
        private readonly SignalBus _signalBus;
        private readonly LazyInject<IShip> _ship;

        private PhysicsBody _shipBody;

        public ContactCollisionHandler(
            CollisionDetector detector,
            CollisionResolver resolver,
            SignalBus signalBus,
            LazyInject<IShip> ship)
        {
            _detector = detector;
            _resolver = resolver;
            _signalBus = signalBus;
            _ship = ship;
        }

        public void RegisterShipBody(PhysicsBody body) => _shipBody = body;

        public void Resolve(List<PhysicsBody> bodies)
        {
            var pairs = _detector.Detect(bodies);

            foreach (var pair in pairs)
            {
                if (!pair.A.IsActive || !pair.B.IsActive) continue;
                HandlePair(pair);
            }
        }


        private void HandlePair(CollisionDetector.CollisionPair pair)
        {
            var la = pair.A.Layer;
            var lb = pair.B.Layer;

            if ((la == PhysicsLayer.Ship && lb == PhysicsLayer.Enemy) ||
                (la == PhysicsLayer.Enemy && lb == PhysicsLayer.Ship))
            {
                if (_ship.Value.IsInvincible) return;
                HandleShipVsEnemy(pair);
                return;
            }

            if ((la == PhysicsLayer.Bullet && lb == PhysicsLayer.Enemy) ||
                (la == PhysicsLayer.Enemy && lb == PhysicsLayer.Bullet))
            {
                HandleBulletVsEnemy(pair);
            }
        }

        private void HandleShipVsEnemy(CollisionDetector.CollisionPair pair)
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

        private static void HandleBulletVsEnemy(CollisionDetector.CollisionPair pair)
        {
            var bulletBody = pair.A.Layer == PhysicsLayer.Bullet ? pair.A : pair.B;
            var enemyBody = pair.A.Layer == PhysicsLayer.Enemy ? pair.A : pair.B;

            bulletBody.Deactivate();
            enemyBody.Deactivate();

            if (bulletBody.Owner is Bullet bullet) bullet.Deactivate();
            if (enemyBody.Owner is IEnemy enemy) enemy.TakeDamage();
        }

        private static void SeparateBodies(PhysicsBody a, PhysicsBody b)
        {
            var normal = (b.Position - a.Position).normalized;
            var overlap = (a.Radius + b.Radius) - Vector2.Distance(a.Position, b.Position);
            var half = overlap * 0.5f;

            a.SetPosition(a.Position - normal * half);
            b.SetPosition(b.Position + normal * half);
        }
    }
}
