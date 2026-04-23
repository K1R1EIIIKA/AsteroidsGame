using Core.Interfaces;
using UI;
using UI.ViewModels;
using UI.Views;
using Zenject;

namespace App.Installers
{
    public class UIInstaller : Installer<UIInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<MainMenuView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<HudView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseMenuView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameOverView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<VirtualJoystickView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ShootButtonView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<LaserButtonView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseButtonView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IUIManager>().To<UIManager>().AsSingle();

            Container.BindInterfacesAndSelfTo<HudViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverViewModel>().AsSingle();
            Container.Bind<MainMenuViewModel>().AsSingle();
            Container.Bind<PauseMenuViewModel>().AsSingle();
        }
    }
}
