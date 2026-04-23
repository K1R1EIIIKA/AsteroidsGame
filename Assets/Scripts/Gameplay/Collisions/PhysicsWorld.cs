using System.Collections.Generic;
using Core.Enums;
using Gameplay.Enemies.Systems;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.Physics;
using UnityEngine;

namespace Gameplay.Collisions
{
    public class PhysicsWorld
    {
        private readonly ContactCollisionHandler _contactHandler;
        private readonly LaserCollisionHandler _laserHandler;
        private readonly BoundaryWrapper _boundary;

        private readonly List<PhysicsBody> _bodies = new List<PhysicsBody>();

        public PhysicsWorld(
            ContactCollisionHandler contactHandler,
            LaserCollisionHandler laserHandler,
            BoundaryWrapper boundary)
        {
            _contactHandler = contactHandler;
            _laserHandler = laserHandler;
            _boundary = boundary;
        }

        public PhysicsBody CreateBody(object owner, Vector2 position, float radius, PhysicsLayer layer)
        {
            var body = new PhysicsBody(owner, position, radius, layer);
            _bodies.Add(body);
            return body;
        }

        public void RegisterShipBody(PhysicsBody body)
        {
            _contactHandler.RegisterShipBody(body);
        }

        public void RemoveBody(PhysicsBody body)
        {
            if (body == null) return;
            body.Deactivate();
            _bodies.Remove(body);
        }

        public void Tick(float delta)
        {
            foreach (var body in _bodies)
                body.Tick(delta);

            foreach (var body in _bodies)
                body.SetPosition(_boundary.Wrap(body.Position));

            _contactHandler.Resolve(_bodies);
        }

        public void CheckLaserCollisions(Laser laser, EnemyPool enemyPool)
            => _laserHandler.CheckCollisions(laser, enemyPool);
    }
}
