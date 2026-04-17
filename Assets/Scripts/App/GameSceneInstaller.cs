using Core.Interfaces;
using Gameplay.States;
using Infrastructure.InputLogic;
using Infrastructure.InputLogic.Handlers;
using Infrastructure.StateMachine;
using UI;
using UI.ViewModels;
using UI.Views;
using Zenject;

namespace App
{
    public class GameSceneInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            InstallInput();
            InstallStateMachine();
            InstallUI();
            InstallTicker();

            Container.BindInterfacesAndSelfTo<GameSceneInstaller>().FromInstance(this).AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IStateSwitcher>().Enter<BootstrapState>();
        }

        private void InstallInput()
        {
            Container.Bind<KeyboardMouseInput>().AsSingle();
            Container.Bind<GamepadInput>().AsSingle();
            Container.Bind<VirtualJoystickInput>().AsSingle();
            Container.Bind<InputSwitcher>().AsSingle();
            Container.Bind<IInputHandler>().To<InputHandlerFacade>().AsSingle();
        }

        private void InstallStateMachine()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<StartState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();
            Container.Bind<PauseState>().AsSingle();
            Container.Bind<GameOverState>().AsSingle();

            Container.Bind<IGameState>().To<BootstrapState>().FromResolve();
            Container.Bind<IGameState>().To<StartState>().FromResolve();
            Container.Bind<IGameState>().To<GameLoopState>().FromResolve();
            Container.Bind<IGameState>().To<PauseState>().FromResolve();
            Container.Bind<IGameState>().To<GameOverState>().FromResolve();

            Container.Bind(
                    typeof(IStateSwitcher),
                    typeof(IGameStateMachine),
                    typeof(GameStateMachine))
                .To<GameStateMachine>()
                .AsSingle();
        }

        private void InstallUI()
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

        private void InstallTicker()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachineTicker>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("GameStateMachineTicker")
                .AsSingle()
                .NonLazy();
        }
    }
}
