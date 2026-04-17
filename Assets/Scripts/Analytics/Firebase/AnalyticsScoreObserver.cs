using System;
using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Zenject;

namespace Analytics.Firebase
{
    public class AnalyticsScoreObserver : IScoreObserver, IInitializable, IDisposable
    {
        private readonly ObserverScoreTracker _tracker;
        private readonly IAnalytics _analytics;
        private readonly IScoreService _scoreService;
        private readonly SignalBus _signalBus;

        public AnalyticsScoreObserver(
            ObserverScoreTracker tracker,
            IAnalytics analytics,
            IScoreService scoreService,
            SignalBus signalBus)
        {
            _tracker = tracker;
            _analytics = analytics;
            _signalBus = signalBus;
            _scoreService = scoreService;
        }

        public void Initialize()
        {
            _tracker.AddObserver(this);
            _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
            _signalBus.Subscribe<LaserFiredSignal>(OnLaserFired);
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
            _signalBus.Subscribe<RewardedAdCompletedSignal>(OnRewardedCompleted);
        }

        public void Dispose()
        {
            _tracker.RemoveObserver(this);
            _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
            _signalBus.Unsubscribe<LaserFiredSignal>(OnLaserFired);
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
            _signalBus.Unsubscribe<RewardedAdCompletedSignal>(OnRewardedCompleted);
        }

        public void OnScoreChanged(int newScore)
        {
            if (newScore > 0 && newScore % 1000 == 0)
                _analytics.LogEvent("score_milestone", ("score", newScore));
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            _analytics.LogEnemyDestroyed(signal.EnemyType.ToString());
        }

        private void OnLaserFired(LaserFiredSignal signal)
        {
            _analytics.LogLaserFired();
        }

        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.NewState == GameStateType.GameLoop)
                _analytics.LogGameStart();

            if (signal.NewState == GameStateType.GameEnd)
                _analytics.LogGameOver(_scoreService.Score);
        }

        private void OnRewardedCompleted(RewardedAdCompletedSignal signal)
        {
            _analytics.LogReviveWithAd();
        }
    }
}
