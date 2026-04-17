using Core.Interfaces;
using Infrastructure.Physics;
using UnityEngine;

namespace Gameplay.PlayerLogic
{
    public class ShipFacade : IShip
    {
        private readonly Ship _ship;

        public ShipFacade(Ship ship) => _ship = ship;

        public bool IsInvincible => _ship.IsInvincible;
        public Vector2 Position => _ship.Position;
        public float RotationAngle => _ship.RotationAngle;
        public float Speed => _ship.Speed;
        public int Health => _ship.Health;
        public int LaserCharges => _ship.LaserCharges;
        public float LaserRechargeProgress => _ship.LaserRechargeProgress;

        public void Thrust(float delta, Vector2 direction = default) => _ship.Thrust(delta, direction);
        public void Rotate(float delta) => _ship.Rotate(delta);
        public void Shoot() => _ship.Shoot();
        public void ShootLaser() => _ship.ShootLaser();
        public void Tick(float delta) => _ship.Tick(delta);
        public void TakeDamage(Vector2 hit) => _ship.TakeDamage(hit);
        public void RotateTowards(float targetAngle, float delta) => _ship.RotateTowards(targetAngle, delta);
        public void SetPosition(Vector2 pos) => _ship.SetPosition(pos);
        public void Reset() => _ship.Reset();
        public void SetHealth(int health) => _ship.SetHealth(health);
        public void ResetBullets() => _ship.ResetBullets();
        public void SetPhysicsBody(PhysicsBody body) => _ship.SetPhysicsBody(body);
    }
}
