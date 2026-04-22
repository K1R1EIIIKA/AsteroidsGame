using System.Collections.Generic;
using System.Threading;
using Core.Configs;
using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Cysharp.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Physics;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies.Systems
{
    public class EnemySpawner : IEnemySpawner
    {
        private readonly EnemyPool _pool;
        private readonly IEnemyFactory _factory;
        private readonly SignalBus _signalBus;
        private readonly ConfigService _config;
        private readonly PhysicsWorld _physicsWorld;
        private readonly AsteroidSpritesConfig _spritesConfig;

        private readonly Dictionary<IEnemy, PhysicsBody> _enemyBodies
            = new Dictionary<IEnemy, PhysicsBody>();

        private CancellationTokenSource _cts;
        public EnemyPool Pool => _pool;

        public EnemySpawner(
            EnemyPool pool,
            IEnemyFactory factory,
            ConfigService config,
            PhysicsWorld physicsWorld,
            AsteroidSpritesConfig spritesConfig,
            SignalBus signalBus)
        {
            _pool = pool;
            _factory = factory;
            _signalBus = signalBus;
            _config = config;
            _physicsWorld = physicsWorld;
            _spritesConfig = spritesConfig;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

            SpawnLoopAsync(EnemyType.Asteroid,
                _config.World.AsteroidSpawnInterval,
                _config.World.MaxAsteroids,
                _cts.Token).Forget();

            SpawnLoopAsync(EnemyType.Saucer,
                _config.World.SaucerSpawnInterval,
                _config.World.MaxSaucers,
                _cts.Token).Forget();
        }

        public void Stop()
        {
            _cts?.Cancel();
            _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
            ReturnAll();
        }

        public void Tick(float delta)
        {
            foreach (var (logic, _) in _pool.Active)
            {
                if (_enemyBodies.TryGetValue(logic, out var body))
                {
                    logic.SetPosition(body.Position);
                    logic.Tick(delta);

                    if (logic is Saucer saucer && saucer.IsBouncing)
                    {
                        body.SetVelocity(saucer.Velocity);
                    }
                    else
                    {
                        body.SetVelocity(logic.Velocity);
                    }
                }
                else
                {
                    logic.Tick(delta);
                }
            }
        }

        private async UniTaskVoid SpawnLoopAsync(
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

                SpawnEnemy(type);
            }
        }

        private void SpawnEnemy(EnemyType type)
        {
            var logic = _factory.Create(type);
            var view = _pool.Get(type);
            var spawnPos = GetSpawnPosition();
            var velocity = GetInitialVelocity(type);

            logic.SetPosition(spawnPos);
            logic.SetVelocity(velocity);

            if (logic is Asteroid asteroid) asteroid.Activate(spawnPos, velocity);
            else if (logic is Fragment fragment) fragment.Activate(spawnPos, velocity);
            else if (logic is SmallFragment small) small.Activate(spawnPos, velocity);
            else if (logic is Saucer saucer) saucer.Activate(spawnPos);

            view.Initialize(logic, _physicsWorld, _spritesConfig); 

            var body = _physicsWorld.CreateBody(
                logic,
                spawnPos,
                view.ColliderRadius,
                PhysicsLayer.Enemy);

            body.SetVelocity(velocity);
            view.PhysicsBody = body;

            _pool.Register(logic, view);
            _enemyBodies[logic] = body;
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            var entry = FindActive(signal.EnemyOwner);
            if (!entry.HasValue) return;

            var (logic, view) = entry.Value;

            if (_enemyBodies.TryGetValue(logic, out var body))
            {
                _physicsWorld.RemoveBody(body);
                _enemyBodies.Remove(logic);
            }

            _pool.Return(logic, view);

            if (signal.FragmentCount <= 0) return;

            var childType = signal.EnemyType switch
                {
                    EnemyType.Asteroid => EnemyType.Fragment,
                    EnemyType.Fragment => EnemyType.SmallFragment,
                    _ => EnemyType.SmallFragment
                };

            SpawnFragments(signal, childType);
        }

        private void SpawnFragments(EnemyDestroyedSignal signal, EnemyType childType)
        {
            for (int i = 0; i < signal.FragmentCount; i++)
            {
                var spreadAngle = (360f / signal.FragmentCount) * i;
                var baseAngle = Mathf.Atan2(signal.Velocity.y, signal.Velocity.x)
                                * Mathf.Rad2Deg;
                var finalAngle = (baseAngle + spreadAngle) * Mathf.Deg2Rad;

                var direction = new Vector2(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle));
                var speed = Mathf.Max(signal.Velocity.magnitude, 2f)
                            * signal.FragmentSpeedMultiplier;

                SpawnAt(childType, signal.Position, direction * speed);
            }
        }

        private void SpawnAt(EnemyType type, Vector2 position, Vector2 velocity)
        {
            var logic = _factory.Create(type);
            var view = _pool.Get(type);

            logic.SetPosition(position);
            logic.SetVelocity(velocity);

            if (logic is Fragment fragment) fragment.Activate(position, velocity);
            else if (logic is SmallFragment small) small.Activate(position, velocity);

            view.Initialize(logic, _physicsWorld, _spritesConfig); 

            var body = _physicsWorld.CreateBody(
                logic,
                position,
                view.ColliderRadius,
                PhysicsLayer.Enemy);

            body.SetVelocity(velocity);
            view.PhysicsBody = body;

            _pool.Register(logic, view);
            _enemyBodies[logic] = body;
        }

        private Vector2 GetSpawnPosition()
        {
            var hw = _config.World.WorldWidth * 0.5f + _config.World.SpawnMargin;
            var hh = _config.World.WorldHeight * 0.5f + _config.World.SpawnMargin;

            return Random.Range(0, 4) switch
                {
                    0 => new Vector2(Random.Range(-hw, hw), hh),
                    1 => new Vector2(Random.Range(-hw, hw), -hh),
                    2 => new Vector2(hw, Random.Range(-hh, hh)),
                    _ => new Vector2(-hw, Random.Range(-hh, hh))
                };
        }

        private Vector2 GetInitialVelocity(EnemyType type)
        {
            var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            var speed = type switch
                {
                    EnemyType.Asteroid => _config.Enemy.Asteroid.Speed,
                    EnemyType.Saucer => _config.Enemy.Saucer.Speed,
                    _ => _config.Enemy.Asteroid.Speed
                };

            return direction * speed;
        }

        private int CountByType(EnemyType type)
        {
            int count = 0;
            foreach (var (logic, _) in _pool.Active)
                if (logic.Type == type)
                    count++;
            return count;
        }

        private (IEnemy logic, EnemyView view)? FindActive(IEnemy body)
        {
            foreach (var (logic, view) in _pool.Active)
                if (logic == body)
                    return (logic, view);
            return null;
        }

        private void ReturnAll()
        {
            var copy = new List<(IEnemy, EnemyView)>(_pool.Active);
            foreach (var (logic, view) in copy)
            {
                if (_enemyBodies.TryGetValue(logic, out var body))
                {
                    _physicsWorld.RemoveBody(body);
                    _enemyBodies.Remove(logic);
                }

                view.Deactivate();
                _pool.Return(logic, view);
            }
        }
    }
}
