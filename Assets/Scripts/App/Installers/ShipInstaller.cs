using Core.Interfaces;
using Gameplay.Configs;
using Gameplay.PlayerLogic;
using Zenject;

namespace App.Installers
{
    public class ShipInstaller : Installer<ShipInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ShipMovement>().AsSingle();
            Container.Bind<ShipHealth>().AsSingle();
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
    }
}
