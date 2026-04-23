using System.Collections.Generic;
using Core.Configs;
using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Gameplay.Collisions;
using Infrastructure.Physics;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies.Systems
{
    public class EnemyLifecycleManager
    {
        private readonly EnemyPool _pool;
        private readonly IEnemyFactory _factory;
        private readonly PhysicsWorld _physicsWorld;
        private readonly AsteroidSpritesConfig _spritesConfig;
        private readonly SignalBus _signalBus;

        private readonly Dictionary<IEnemy, PhysicsBody> _enemyBodies
            = new Dictionary<IEnemy, PhysicsBody>();

        public EnemyLifecycleManager(
            EnemyPool pool,
            IEnemyFactory factory,
            PhysicsWorld physicsWorld,
            AsteroidSpritesConfig spritesConfig,
            SignalBus signalBus)
        {
            _pool = pool;
            _factory = factory;
            _physicsWorld = physicsWorld;
            _spritesConfig = spritesConfig;
            _signalBus = signalBus;
        }

        public void Subscribe() => _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        public void Unsubscribe() => _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);

        public bool TryGetBody(IEnemy logic, out PhysicsBody body)
            => _enemyBodies.TryGetValue(logic, out body);

        public void SpawnEnemy(EnemyType type, Vector2 position, Vector2 velocity)
        {
            var enemy = _factory.Create(type);
            var view = _pool.Get(type);

            enemy.SetPosition(position);
            enemy.SetVelocity(velocity);
            InitializeEnemy(enemy, position, velocity);

            view.Initialize(enemy, _physicsWorld, _spritesConfig);

            var body = _physicsWorld.CreateBody(
                enemy,
                position,
                view.ColliderRadius,
                PhysicsLayer.Enemy);

            body.SetVelocity(velocity);
            view.PhysicsBody = body;

            _pool.Register(enemy, view);
            _enemyBodies[enemy] = body;
        }

        public void DespawnAll()
        {
            var copy = new List<(IEnemy, EnemyView)>(_pool.Active);
            foreach (var (logic, view) in copy)
                Dematerialize(logic, view, deactivateView: true);
        }


        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            var entry = FindActive(signal.EnemyOwner);
            if (!entry.HasValue) return;

            var (logic, view) = entry.Value;
            Dematerialize(logic, view, deactivateView: false);

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

                SpawnEnemy(childType, signal.Position, direction * speed);
            }
        }

        private void Dematerialize(IEnemy logic, EnemyView view, bool deactivateView)
        {
            if (_enemyBodies.TryGetValue(logic, out var body))
            {
                _physicsWorld.RemoveBody(body);
                _enemyBodies.Remove(logic);
            }

            if (deactivateView) view.Deactivate();
            _pool.Return(logic, view);
        }

        private static void InitializeEnemy(IEnemy logic, Vector2 position, Vector2 velocity)
        {
            switch (logic)
            {
                case Asteroid a: a.Initialize(position, velocity); break;
                case Fragment f: f.Initialize(position, velocity); break;
                case SmallFragment s: s.Initialize(position, velocity); break;
                case Saucer u: u.Initialize(position); break;
            }
        }

        private (IEnemy logic, EnemyView view)? FindActive(IEnemy target)
        {
            foreach (var (logic, view) in _pool.Active)
                if (logic == target)
                    return (logic, view);
            return null;
        }
    }
}
