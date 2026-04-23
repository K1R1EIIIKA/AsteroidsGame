using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Gameplay.Enemies.Systems;
using Gameplay.Weapons;
using Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace Gameplay.States
{
    public class BootstrapState : IGameState
    {
        private readonly LazyInject<IStateSwitcher> _stateSwitcher;
        private readonly IScoreService _scoreService;
        private readonly RewardDictionary _rewardDictionary;
        private readonly EnemyPool _enemyPool;
        private readonly BulletPool _bulletPool;

        private bool _poolsInitialized;

        public BootstrapState(
            LazyInject<IStateSwitcher> stateSwitcher,
            IScoreService scoreService,
            RewardDictionary rewardDictionary,
            EnemyPool enemyPool,
            BulletPool bulletPool)
        {
            _stateSwitcher = stateSwitcher;
            _scoreService = scoreService;
            _rewardDictionary = rewardDictionary;
            _enemyPool = enemyPool;
            _bulletPool = bulletPool;
        }

        public void Enter()
        {
            InitializeAsync().Forget();
        }

        private async UniTaskVoid InitializeAsync()
        {
            _rewardDictionary.Initialize();
            _scoreService.Reset();

            await UniTask.Yield();

            InitializePools();
            _stateSwitcher.Value.Enter<StartState>();
        }

        private void InitializePools()
        {
            if (_poolsInitialized) return;

            var poolParent = new GameObject("Pools").transform;
            _enemyPool.Initialize(poolParent);
            _bulletPool.Initialize(poolParent);

            _poolsInitialized = true;
        }

        public void Tick() {}
        public void Exit() {}
    }
}
