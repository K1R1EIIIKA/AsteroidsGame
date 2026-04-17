using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Zenject;

namespace Gameplay.States
{
    public class GameOverState : IGameState
    {
        private readonly LazyInject<IStateSwitcher> _stateSwitcher;
        private readonly LazyInject<IUIManager> _uiManager;
        private readonly LazyInject<GameLoopState> _gameLoopState;
        private readonly IScoreService _scoreService;
        private readonly IAdvertising _advertising;
        private readonly SignalBus _signalBus;
        
        private static int _deathCount; 

        public GameOverState(
            LazyInject<IStateSwitcher> stateSwitcher,
            LazyInject<IUIManager> uiManager,
            IScoreService scoreService,
            LazyInject<GameLoopState> gameLoopState,
            IAdvertising advertising,
            SignalBus signalBus)
        {
            _stateSwitcher = stateSwitcher;
            _uiManager = uiManager;
            _scoreService = scoreService;
            _gameLoopState = gameLoopState;
            _advertising = advertising;
            _signalBus = signalBus;
        }

        public void Enter()
        {
            _deathCount++;

            if (_deathCount % 2 == 0)
                _advertising.ShowInterstitial();

            _uiManager.Value.ShowGameOver();

            _signalBus.Subscribe<GameStartedSignal>(OnRestart);
            _signalBus.Subscribe<WatchAdRequestedSignal>(OnWatchAd); 
            _signalBus.Subscribe<RewardedAdCompletedSignal>(OnRewardedCompleted);
            _signalBus.Fire(new GameStateChangedSignal
                {
                    NewState = GameStateType.GameEnd
                });
        }

        public void Tick() {}

        public void Exit()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnRestart);
            _signalBus.Unsubscribe<WatchAdRequestedSignal>(OnWatchAd);
            _signalBus.Unsubscribe<RewardedAdCompletedSignal>(OnRewardedCompleted); 
            _uiManager.Value.HideGameOver();
        }

        private void OnRestart()
        {
            _gameLoopState.Value.ResetGame();
            _stateSwitcher.Value.Enter<GameLoopState>();
        }
        
        private void OnRewardedCompleted(RewardedAdCompletedSignal signal)
        {
            _gameLoopState.Value.ContinueAfterAd();
            _stateSwitcher.Value.Enter<GameLoopState>();
        }
        
        
        private void OnWatchAd(WatchAdRequestedSignal signal)
        {
            _advertising.ShowRewarded(() =>
            {
                _signalBus.Fire(new RewardedAdCompletedSignal());
            });
        }
    }
}
