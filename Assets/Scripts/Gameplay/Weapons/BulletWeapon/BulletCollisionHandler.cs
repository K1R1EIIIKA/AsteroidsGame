using System;
using Gameplay.Enemies.Systems;
using Infrastructure.Physics;
using Zenject;

namespace Gameplay.Weapons.BulletWeapon
{
    public class BulletCollisionHandler : IInitializable, IDisposable
    {
        private readonly BulletPool _bulletPool;
        private readonly EnemyPool _enemyPool;
        private readonly CollisionDetector _detector;

        public BulletCollisionHandler(
            BulletPool bulletPool,
            EnemyPool enemyPool,
            CollisionDetector detector)
        {
            _bulletPool = bulletPool;
            _enemyPool = enemyPool;
            _detector = detector;
        }

        public void Initialize() {}
        public void Dispose() {}

        public void Tick()
        {
            foreach (var (bullet, bulletView) in _bulletPool.Active)
            {
                if (!bullet.IsActive) continue;
                if (bulletView.PhysicsBody == null) continue;

                foreach (var (enemy, enemyView) in _enemyPool.Active)
                {
                    if (!enemy.IsActive) continue;
                    if (enemyView.PhysicsBody == null) continue;

                    if (!_detector.Overlaps(bulletView.PhysicsBody, enemyView.PhysicsBody))
                        continue;

                    bulletView.PhysicsBody.Deactivate();
                    bullet.Deactivate();
                    enemy.TakeDamage();
                    break;
                }
            }
        }
    }
}
