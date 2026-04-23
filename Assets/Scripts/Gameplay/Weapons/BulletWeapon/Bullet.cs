using Gameplay.Collisions;
using Infrastructure.Data.Configs;
using Infrastructure.Physics;
using UnityEngine;

namespace Gameplay.Weapons.BulletWeapon
{
    public class Bullet
    {
        private readonly PhysicsWorld _physicsWorld;
        private readonly BulletConfig _config;

        private Vector2 _position;
        private Vector2 _velocity;
        private float _lifetime;

        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public bool IsActive { get; private set; }
        public PhysicsBody PhysicsBody { get; private set; }

        public Bullet(BulletConfig config, PhysicsWorld physicsWorld)
        {
            _config = config;
            _physicsWorld = physicsWorld;
        }

        public void Activate(Vector2 position, float angle)
        {
            _position = position;
            _velocity = new Vector2(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                Mathf.Cos(angle * Mathf.Deg2Rad)) * _config.BulletSpeed;
            _lifetime = _config.BulletLifetime;
            IsActive = true;
        }

        public void Tick(float delta)
        {
            if (!IsActive) return;

            _position += _velocity * delta;
            _lifetime -= delta;

            if (_lifetime <= 0f)
                Deactivate();
        }

        public void Deactivate()
        {
            IsActive = false;
            _physicsWorld.RemoveBody(PhysicsBody);
        }
        
        public void SetPhysicsBody(PhysicsBody physicsBody)
        {
            PhysicsBody = physicsBody;
        }
    }
}
