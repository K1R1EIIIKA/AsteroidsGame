using Core.Interfaces;
using Gameplay.Weapons;
using Gameplay.Weapons.LaserWeapon;
using Zenject;

namespace App.Installers
{
    public class WeaponsInstaller : Installer<WeaponsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Laser>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle();

            Container.Bind<IWeaponController>()
                .FromMethod(ctx => ctx.Container.Resolve<IWeaponFactory>().Create())
                .AsSingle();
        }
    }
}
