using Gameplay;
using Gameplay.Collisions;
using Gameplay.PlayerLogic;
using Infrastructure.Physics;
using Zenject;

namespace App.Installers
{
    public class PhysicsInstaller : Installer<PhysicsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<CollisionDetector>().AsSingle();
            Container.Bind<CollisionResolver>().AsSingle();
            Container.Bind<BoundaryWrapper>().AsSingle();
            Container.Bind<ContactCollisionHandler>().AsSingle();
            Container.Bind<LaserCollisionHandler>().AsSingle();
            Container.Bind<PhysicsWorld>().AsSingle();

            Container.BindInterfacesAndSelfTo<ShipCollisionHandler>().AsSingle();
        }
    }
}
