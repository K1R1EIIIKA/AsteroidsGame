using System.Collections.Generic;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Pools;
using UnityEngine;

namespace Gameplay.Enemies.Systems
{
    public class EnemyPool
    {
        private readonly MonoPool<EnemyView> _asteroidPool;
        private readonly MonoPool<EnemyView> _saucerPool;
        private readonly MonoPool<EnemyView> _fragmentPool;
        private readonly MonoPool<EnemyView> _smallFragmentPool;

        private readonly List<(IEnemy logic, EnemyView view)> _active
            = new List<(IEnemy, EnemyView)>();

        public IReadOnlyList<(IEnemy logic, EnemyView view)> Active => _active;

        public EnemyPool(
            EnemyView asteroidPrefab,
            EnemyView saucerPrefab,
            EnemyView fragmentPrefab,
            EnemyView smallFragmentPrefab)
        {
            _asteroidPool = new MonoPool<EnemyView>(asteroidPrefab, 10);
            _saucerPool = new MonoPool<EnemyView>(saucerPrefab, 3);
            _fragmentPool = new MonoPool<EnemyView>(fragmentPrefab, 20);
            _smallFragmentPool = new MonoPool<EnemyView>(smallFragmentPrefab, 30);
        }

        public EnemyView Get(EnemyType type) => type switch
            {
                EnemyType.Asteroid => _asteroidPool.Get(),
                EnemyType.Saucer => _saucerPool.Get(),
                EnemyType.Fragment => _fragmentPool.Get(),
                EnemyType.SmallFragment => _smallFragmentPool.Get(),
                _ => _asteroidPool.Get()
            };

        public void Initialize(Transform parent)
        {
            _asteroidPool.Initialize(parent);
            _saucerPool.Initialize(parent);
            _fragmentPool.Initialize(parent);
            _smallFragmentPool.Initialize(parent);
        }

        public void Register(IEnemy logic, EnemyView view)
            => _active.Add((logic, view));

        public void Return(IEnemy logic, EnemyView view)
        {
            _active.RemoveAll(e => e.logic == logic);
            ReturnView(logic.Type, view);
        }

        private void ReturnView(EnemyType type, EnemyView view)
        {
            switch (type)
            {
                case EnemyType.Asteroid: _asteroidPool.Return(view); break;
                case EnemyType.Saucer: _saucerPool.Return(view); break;
                case EnemyType.Fragment: _fragmentPool.Return(view); break;
                case EnemyType.SmallFragment: _smallFragmentPool.Return(view); break;
            }
        }
    }
}
