using Analytics.Advertising;
using Analytics.Firebase;
using Core.Configs;
using Core.Interfaces;
using Core.Signals;
using Gameplay;
using Gameplay.Configs;
using Gameplay.Enemies.Systems;
using Gameplay.PlayerLogic;
using Gameplay.Weapons;
using Gameplay.Weapons.BulletWeapon;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.Data;
using Infrastructure.Data.Configs;
using Infrastructure.Physics;
using Infrastructure.Services;
using UnityEngine;
using Zenject;

namespace App
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private AsteroidSpritesConfig _asteroidSpritesConfig;

        public override void InstallBindings()
        {
            InstallSignals();
            InstallConfigs();
            InstallPhysics();
            InstallServices();
            InstallPools();
            InstallShip();
            InstallEnemies();
            InstallWeapons();
            InstallPatterns();
            InstallAnalytics();
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<EnemyDestroyedSignal>();
            Container.DeclareSignal<PlayerDamagedSignal>();
            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<LaserFiredSignal>();
            Container.DeclareSignal<GameStateChangedSignal>();
            Container.DeclareSignal<ScoreChangedSignal>();
            Container.DeclareSignal<ShipCollisionSignal>();
            Container.DeclareSignal<GameStartedSignal>();
            Container.DeclareSignal<GamePausedSignal>();
            Container.DeclareSignal<GameResumedSignal>();
            Container.DeclareSignal<PlayerRespawnedSignal>();
            Container.DeclareSignal<EnemySpawnedSignal>();
            Container.DeclareSignal<PlayerInvincibilityEndedSignal>();
            Container.DeclareSignal<RewardedAdCompletedSignal>();
            Container.DeclareSignal<WatchAdRequestedSignal>();
        }

        private void InstallConfigs()
        {
            var shipConfig  = JsonConfigLoader.Load<ShipConfig>("ship_config");
            var enemyConfig = JsonConfigLoader.Load<EnemyConfig>("enemy_config");
            var worldConfig = JsonConfigLoader.Load<WorldConfig>("world_config");

            var configService = new ConfigService();
            configService.Initialize(shipConfig, enemyConfig, worldConfig);

            Container.Bind<ConfigService>().FromInstance(configService).AsSingle();

            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
            
            Container.Bind<AsteroidSpritesConfig>().FromInstance(_asteroidSpritesConfig).AsSingle();
        }

        private void InstallPhysics()
        {
            Container.Bind<CollisionDetector>().AsSingle();
            Container.Bind<CollisionResolver>().AsSingle();
            Container.Bind<BoundaryWrapper>().AsSingle();
            Container.Bind<PhysicsWorld>().AsSingle();

            Container.BindInterfacesAndSelfTo<ShipCollisionHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletCollisionHandler>().AsSingle();
        }

        private void InstallServices()
        {
            Container.Bind<IScoreService>().To<ScoreService>().AsSingle();

            Container.Bind<IGameTimeService>().To<GameTimeService>().AsSingle();

            Container.Bind<RewardDictionary>().AsSingle();
        }

        private void InstallPools()
        {
            Container.Bind<EnemyPool>()
                .FromMethod(ctx =>
                {
                    var config = ctx.Container.Resolve<GameConfig>();
                    return new EnemyPool(
                        config.AsteroidPrefab,
                        config.SaucerPrefab,
                        config.FragmentPrefab,
                        config.SmallFragmentPrefab);
                })
                .AsSingle();

            Container.Bind<BulletPool>()
                .FromMethod(ctx =>
                {
                    var gameConfig = ctx.Container.Resolve<GameConfig>();
                    var bulletConfig = ctx.Container.Resolve<ConfigService>();
                    var physicsWorld = ctx.Container.Resolve<PhysicsWorld>();
                    return new BulletPool(gameConfig.BulletPrefab, bulletConfig, physicsWorld);
                })
                .AsSingle();
        }

        private void InstallShip()
        {
            Container.Bind<Ship>().AsSingle();
            Container.Bind<IShip>().To<ShipFacade>().AsSingle();

            Container.Bind<ShipView>()
                .FromMethod(ctx =>
                {
                    var config = ctx.Container.Resolve<GameConfig>();
                    return ctx.Container
                        .InstantiatePrefabForComponent<ShipView>(config.ShipPrefab);
                })
                .AsSingle();
        }

        private void InstallEnemies()
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();

            Container.Bind<EnemySpawner>().AsSingle();
            Container.Bind<IEnemySpawner>()
                .FromMethod(ctx => ctx.Container.Resolve<EnemySpawner>())
                .AsSingle();
        }

        private void InstallWeapons()
        {
            Container.Bind<Laser>().AsSingle();

            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();

            Container.Bind<IWeaponController>()
                .FromMethod(ctx => ctx.Container.Resolve<IWeaponFactory>().Create())
                .AsSingle();
        }

        private void InstallPatterns()
        {
            Container.BindInterfacesAndSelfTo<ObserverScoreTracker>().AsSingle();
        }

        private void InstallAnalytics()
        {
            Container.Bind<IAnalytics>().To<FirebaseAnalyticsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<YandexAdsAdapter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AnalyticsScoreObserver>().AsSingle();
        }
    }
}
