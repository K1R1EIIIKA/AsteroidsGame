using Gameplay;
using Gameplay.Collisions;
using Gameplay.Configs;
using Gameplay.Enemies.Systems;
using Gameplay.Weapons;
using Infrastructure.Data;
using Zenject;

namespace App.Installers
{
    public class PoolsInstaller : Installer<PoolsInstaller>
    {
        public override void InstallBindings()
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
                    var gameConfig   = ctx.Container.Resolve<GameConfig>();
                    var configService = ctx.Container.Resolve<ConfigService>();
                    var physicsWorld = ctx.Container.Resolve<PhysicsWorld>();
                    return new BulletPool(gameConfig.BulletPrefab, configService, physicsWorld);
                })
                .AsSingle();
        }
    }
}
