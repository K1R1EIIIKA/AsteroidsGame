using System;
using System.Collections.Generic;
using Core.Interfaces;
using Core.Signals;
using Infrastructure.Services;
using Zenject;

namespace Analytics.Firebase
{
    public class ObserverScoreTracker : IInitializable, IDisposable
    {
        private readonly IScoreService _scoreService;
        private readonly SignalBus _signalBus;
        private readonly RewardDictionary _rewardDictionary;

        private readonly List<IScoreObserver> _observers = new List<IScoreObserver>();

        public ObserverScoreTracker(
            IScoreService scoreService,
            SignalBus signalBus,
            RewardDictionary rewardDictionary)
        {
            _scoreService = scoreService;
            _signalBus = signalBus;
            _rewardDictionary = rewardDictionary;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        public void AddObserver(IScoreObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void RemoveObserver(IScoreObserver observer)
        {
            _observers.Remove(observer);
        }

        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            var reward = _rewardDictionary.Get(signal.EnemyType);
            _scoreService.Add(reward);

            var newScore = _scoreService.Score;
            _signalBus.Fire(new ScoreChangedSignal { NewScore = newScore });

            NotifyObservers(newScore);
        }

        private void NotifyObservers(int newScore)
        {
            foreach (var observer in _observers)
                observer.OnScoreChanged(newScore);
        }
    }
}
