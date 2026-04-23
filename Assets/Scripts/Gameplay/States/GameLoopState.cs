using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Gameplay.PlayerLogic;
using Zenject;

namespace Gameplay.States
{
    public class GameLoopState : IGameState
    {
        private readonly LazyInject<IStateSwitcher> _stateSwitcher;
        private readonly LazyInject<ShipView> _shipView;
        private readonly LazyInject<IUIManager> _uiManager;
        private readonly IShip _ship;
        private readonly IEnemySpawner _spawner;
        private readonly IScoreService _scoreService;
        private readonly SignalBus _signalBus;
        private readonly GameplayUpdater _updater;

        private bool _isFirstEnter = true;

        public GameLoopState(
            LazyInject<IStateSwitcher> stateSwitcher,
            LazyInject<ShipView> shipView,
            LazyInject<IUIManager> uiManager,
            IShip ship,
            IEnemySpawner spawner,
            IScoreService scoreService,
            SignalBus signalBus,
            GameplayUpdater updater)
        {
            _stateSwitcher = stateSwitcher;
            _shipView = shipView;
            _uiManager = uiManager;
            _ship = ship;
            _spawner = spawner;
            _scoreService = scoreService;
            _signalBus = signalBus;
            _updater = updater;

            _signalBus.Subscribe<GameRestartedSignal>(OnGameRestarted);
            _signalBus.Subscribe<ContinueAfterAdSignal>(OnContinueAfterAd);
        }

        public void Enter()
        {
            if (_isFirstEnter)
            {
                _scoreService.Reset();
                _ship.Reset();
                _shipView.Value.gameObject.SetActive(true);
                _isFirstEnter = false;
            }

            _ship.ResetWeapons();
            _spawner.Start();
            _uiManager.Value.ShowHud();

            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Subscribe<GamePausedSignal>(OnPaused);

            _signalBus.Fire(new GameStateChangedSignal { NewState = GameStateType.GameLoop });
        }

        public void Tick() => _updater.Tick();

        public void Exit()
        {
            _spawner.Stop();
            _uiManager.Value.HideHud();

            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Unsubscribe<GamePausedSignal>(OnPaused);
        }

        private void OnGameRestarted() => _isFirstEnter = true;

        private void OnContinueAfterAd()
        {
            _ship.SetHealth(1);
            _isFirstEnter = false;
        }

        private void OnPlayerDied() =>
            _stateSwitcher.Value.Enter<GameOverState>();

        private void OnPaused(GamePausedSignal _) =>
            _stateSwitcher.Value.Enter<PauseState>();
    }
}
