using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Zenject;

namespace Gameplay.States
{
    public class PauseState : IGameState
    {
        private readonly LazyInject<IStateSwitcher> _stateSwitcher;
        private readonly LazyInject<IUIManager> _uiManager;
        private readonly IInputHandler _input;
        private readonly IAdvertising _advertising;
        private readonly IGameTimeService _timeService;
        private readonly SignalBus _signalBus;

        public PauseState(
            LazyInject<IStateSwitcher> stateSwitcher,
            IInputHandler input,
            LazyInject<IUIManager> uiManager,
            IAdvertising advertising,
            IGameTimeService timeService,
            SignalBus signalBus)
        {
            _stateSwitcher = stateSwitcher;
            _uiManager = uiManager;
            _timeService = timeService;
            _input = input;
            _advertising = advertising;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _timeService.Pause();
            _uiManager.Value.ShowPauseMenu();
            _advertising.ShowBanner();

            _signalBus.Subscribe<GameResumedSignal>(OnResumed);
            _signalBus.Fire(new GameStateChangedSignal
                {
                    NewState = GameStateType.Pause
                });
        }

        public void Tick()
        {
            _input.UpdateInput();

            if (_input.IsPausePressed)
                OnResumed();
        }

        public void Exit()
        {
            _timeService.Resume();
            _advertising.HideBanner();
            _uiManager.Value.HidePauseMenu();

            _signalBus.Unsubscribe<GameResumedSignal>(OnResumed);
        }

        private void OnResumed() =>
            _stateSwitcher.Value.Enter<GameLoopState>();
    }
}
