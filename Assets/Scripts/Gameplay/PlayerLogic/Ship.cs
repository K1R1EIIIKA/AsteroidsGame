using System.Threading;
using Core.Interfaces;
using Core.Signals;
using Cysharp.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Physics;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayerLogic
{
    public class Ship : IShip
    {
        private readonly ShipMovement _movement;
        private readonly ShipHealth _health;
        private readonly IWeaponController _weapons;

        public bool IsInvincible => _health.IsInvincible;
        public Vector2 Position => _movement.Position;
        public float RotationAngle => _movement.RotationAngle;
        public float Speed => _movement.Speed;
        public int Health => _health.Health;
        public int LaserCharges => _weapons.LaserCharges;
        public float LaserRechargeProgress => _weapons.LaserRechargeProgress;

        public Ship(ShipMovement movement, ShipHealth health, IWeaponController weapons)
        {
            _movement = movement;
            _health = health;
            _weapons = weapons;
        }

        public void SetPhysicsBody(PhysicsBody body) => _movement.SetPhysicsBody(body);

        public void Thrust(float delta, Vector2 direction = default)
        {
            if (_health.IsInvincible) return;
            _movement.Thrust(delta, direction);
        }

        public void Rotate(float delta)
        {
            if (_health.IsInvincible) return;
            _movement.Rotate(delta);
        }

        public void RotateTowards(float targetAngle, float delta)
        {
            if (_health.IsInvincible) return;
            _movement.RotateTowards(targetAngle, delta);
        }

        public void Shoot()
        {
            if (_health.IsInvincible) return;
            _weapons.Shoot(_movement.Position, _movement.RotationAngle);
        }

        public void ShootLaser()
        {
            if (_health.IsInvincible) return;
            _weapons.ShootLaser(_movement.Position, _movement.RotationAngle);
        }

        public void Tick(float delta)
        {
            _movement.Tick(delta);
            _weapons.Tick(delta);
        }

        public void TakeDamage(Vector2 hitVelocity)
        {
            if (_health.TakeDamage())
                _movement.SetVelocity(hitVelocity);
        }

        public void SetPosition(Vector2 position) => _movement.SetPosition(position);
        public void SetHealth(int health) => _health.Set(health);

        public void ResetWeapons()
        {
            _weapons.ResetBullets();
            _weapons.ResetLaser();
        }

        public void Reset()
        {
            _movement.Reset();
            _health.Reset();
        }
    }
}
