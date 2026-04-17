using Core.Interfaces;
using Core.Signals;
using Zenject;

namespace Gameplay.States
{
    public class StartState : IGameState
    {
        private readonly LazyInject<IStateSwitcher> _stateSwitcher;
        private readonly LazyInject<IUIManager> _uiManager;
        private readonly SignalBus _signalBus;

        public StartState(
            LazyInject<IStateSwitcher> stateSwitcher,
            LazyInject<IUIManager> uiManager,
            SignalBus signalBus)
        {
            _stateSwitcher = stateSwitcher;
            _uiManager = uiManager;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _uiManager.Value.ShowMainMenu();
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
        }

        public void Tick() { }

        public void Exit()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _uiManager.Value.HideMainMenu();
        }

        private void OnGameStarted()
        {
            _stateSwitcher.Value.Enter<GameLoopState>();
        }
    }
}
