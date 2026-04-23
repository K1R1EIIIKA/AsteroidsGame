using Core.Interfaces;
using Gameplay.States;
using Infrastructure.StateMachine;
using Zenject;

namespace App.Installers
{
    public class StateMachineInstaller : Installer<StateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<StartState>().AsSingle();
            Container.Bind<GameplayUpdater>().AsSingle();
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
    }
}
