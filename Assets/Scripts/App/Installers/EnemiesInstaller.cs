using Core.Interfaces;
using Gameplay.Enemies.Systems;
using Zenject;

namespace App.Installers
{
    public class EnemiesInstaller : Installer<EnemiesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
 
            Container.Bind<EnemySpawnPointProvider>().AsSingle();
            Container.Bind<EnemyLifecycleManager>().AsSingle();
 
            Container.Bind<EnemySpawner>().AsSingle();
            Container.Bind<IEnemySpawner>()
                .FromMethod(ctx => ctx.Container.Resolve<EnemySpawner>())
                .AsSingle();
        }
    }
}
