using System.Threading;
using Core.Enums;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using Infrastructure.Data;

namespace Gameplay.Enemies.Systems
{
    public class EnemySpawner : IEnemySpawner
    {
        private readonly EnemyPool _pool;
        private readonly EnemyLifecycleManager _lifecycle;
        private readonly EnemySpawnPointProvider _spawnPoints;
        private readonly ConfigService _config;

        private CancellationTokenSource _cts;

        public EnemySpawner(
            EnemyPool pool,
            EnemyLifecycleManager lifecycle,
            EnemySpawnPointProvider spawnPoints,
            ConfigService config)
        {
            _pool = pool;
            _lifecycle = lifecycle;
            _spawnPoints = spawnPoints;
            _config = config;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _lifecycle.Subscribe();

            RunLoopAsync(
                EnemyType.Asteroid,
                _config.World.AsteroidSpawnInterval,
                _config.World.MaxAsteroids,
                _cts.Token).Forget();

            RunLoopAsync(
                EnemyType.Saucer,
                _config.World.SaucerSpawnInterval,
                _config.World.MaxSaucers,
                _cts.Token).Forget();
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _lifecycle.Unsubscribe();
            _lifecycle.DespawnAll();
        }

        public void Tick(float delta)
        {
            foreach (var (logic, _) in _pool.Active)
            {
                if (!_lifecycle.TryGetBody(logic, out var body))
                {
                    logic.Tick(delta);
                    continue;
                }

                logic.SetPosition(body.Position);
                logic.Tick(delta);

                if (logic is Saucer saucer && saucer.IsBouncing)
                    body.SetVelocity(saucer.Velocity);
                else
                    body.SetVelocity(logic.Velocity);
            }
        }


        private async UniTaskVoid RunLoopAsync(
            EnemyType type, float interval, int max, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(
                        (int)(interval * 1000),
                        cancellationToken: token)
                    .SuppressCancellationThrow();

                if (token.IsCancellationRequested) break;
                if (_pool.Active.Count >= _config.World.MaxEnemiesOnMap) continue;
                if (CountByType(type) >= max) continue;

                _lifecycle.SpawnEnemy(
                    type,
                    _spawnPoints.GetSpawnPosition(),
                    _spawnPoints.GetInitialVelocity(type));
            }
        }

        private int CountByType(EnemyType type)
        {
            int count = 0;
            foreach (var (logic, _) in _pool.Active)
                if (logic.Type == type)
                    count++;
            return count;
        }
    }
}
