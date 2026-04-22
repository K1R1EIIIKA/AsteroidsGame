using Core.Interfaces;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.Data;
using UnityEngine;

namespace Gameplay.Weapons
{
    public class WeaponController : IWeaponController
    {
        private readonly Laser _laser;
        private readonly BulletPool _bulletPool;
        private readonly ConfigService _config;
        private readonly PhysicsWorld _physicsWorld;

        private float _fireTimer;

        public int LaserCharges => _laser.Charges;
        public float LaserRechargeProgress => _laser.RechargeProgress;

        public WeaponController(
            Laser laser,
            BulletPool bulletPool,
            ConfigService config,
            PhysicsWorld physicsWorld)
        {
            _laser = laser;
            _bulletPool = bulletPool;
            _config = config;
            _physicsWorld = physicsWorld;
        }

        public void Shoot(Vector2 position, float angle)
        {
            if (_fireTimer > 0f) return;

            var direction     = new Vector2(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                Mathf.Cos(angle * Mathf.Deg2Rad));
            var spawnPosition = position + direction * 1.5f;

            var (bullet, view) = _bulletPool.Get();
            bullet.Activate(spawnPosition, angle);
            view.Initialize(bullet, _physicsWorld); 

            _fireTimer = 1f / _config.Ship.Bullet.FireRate;
        }

        public void ShootLaser(Vector2 position, float angle)
        {
            _laser.TryFire(position, angle);
        }

        public void Tick(float delta)
        {
            _fireTimer = Mathf.Max(0f, _fireTimer - delta);
            _laser.Tick(delta);
            _bulletPool.TickAll(delta);
        }
        
        public void ResetBullets()
        {
            _bulletPool.ReturnAll(); 
        }
        
        public void ResetLaser()
        {
            _laser.Reset();
        }
    }
}
