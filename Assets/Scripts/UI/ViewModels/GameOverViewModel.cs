using System;
using Core.Signals;
using Zenject;

namespace UI.ViewModels
{
    public class GameOverViewModel : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        public int FinalScore { get; private set; }
        public event Action OnChanged;

        public GameOverViewModel(
            SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ScoreChangedSignal>(OnScoreChanged);
        }

        public void Restart()
        {
            _signalBus.Fire(new GameStartedSignal());
        }

        private void OnScoreChanged(ScoreChangedSignal signal)
        {
            FinalScore = signal.NewScore;
            OnChanged?.Invoke();
        }
        public void WatchAd()
        {
            _signalBus.Fire(new WatchAdRequestedSignal());
        }
    }
}
