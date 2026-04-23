using System.Collections.Generic;
using Gameplay.Collisions;
using Gameplay.Weapons.BulletWeapon;
using Infrastructure.Data;
using Infrastructure.Pools;
using UnityEngine;

namespace Gameplay.Weapons
{
    public class BulletPool
    {
        private readonly MonoPool<BulletView> _pool;
        private readonly ConfigService _config; 
        private readonly List<(Bullet bullet, BulletView view)> _active
            = new List<(Bullet, BulletView)>();
        private readonly PhysicsWorld _physicsWorld;

        public IReadOnlyList<(Bullet bullet, BulletView view)> Active => _active;

        public BulletPool(BulletView prefab, ConfigService config, PhysicsWorld physicsWorld)
        {
            _pool = new MonoPool<BulletView>(prefab, 20);
            _config = config;
            _physicsWorld = physicsWorld;
        }
        
        public void Initialize(Transform parent)
        {
            _pool.Initialize(parent);
        }

        public (Bullet bullet, BulletView view) Get()
        {
            var view   = _pool.Get();
            var bullet = new Bullet(_config.Ship.Bullet, _physicsWorld);
            _active.Add((bullet, view));
            return (bullet, view);
        }

        public void TickAll(float delta)
        {
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var (bullet, view) = _active[i];
                bullet.Tick(delta);

                if (!bullet.IsActive)
                {
                    _pool.Return(view);
                    _active.RemoveAt(i);
                }
            }
        }
        public void ReturnAll()
        {
            foreach (var (_, view) in _active)
            {
                _pool.Return(view);
            }
            _active.Clear();
        }
    }
}
