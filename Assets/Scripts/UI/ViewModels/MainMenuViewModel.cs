using Core.Signals;
using Zenject;

namespace UI.ViewModels
{
    public class MainMenuViewModel
    {
        private readonly SignalBus _signalBus;

        public MainMenuViewModel(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void StartGame()
        {
            _signalBus.Fire(new GameStartedSignal());
        }
    }
}
