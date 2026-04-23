using Core.Signals;
using Zenject;

namespace App.Installers
{
    public class SignalsInstaller : Installer<SignalsInstaller>
    {
        public override void InstallBindings()
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
            Container.DeclareSignal<PlayerInvincibilityEndedSignal>();
            Container.DeclareSignal<RewardedAdCompletedSignal>();
            Container.DeclareSignal<WatchAdRequestedSignal>();
            Container.DeclareSignal<GameRestartedSignal>();
            Container.DeclareSignal<ContinueAfterAdSignal>();
        }
    }
}
