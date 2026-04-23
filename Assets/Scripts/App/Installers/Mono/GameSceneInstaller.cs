using Core.Interfaces;
using Gameplay.States;
using Infrastructure.StateMachine;
using Zenject;

namespace App.Installers.Mono
{
    public class GameSceneInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            InputInstaller.Install(Container);
            StateMachineInstaller.Install(Container);
            UIInstaller.Install(Container);

            Container.BindInterfacesAndSelfTo<GameStateMachineTicker>()
                .FromNewComponentOnNewGameObject()
                .WithGameObjectName("GameStateMachineTicker")
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<GameSceneInstaller>()
                .FromInstance(this)
                .AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IStateSwitcher>().Enter<BootstrapState>();
        }
    }
}
