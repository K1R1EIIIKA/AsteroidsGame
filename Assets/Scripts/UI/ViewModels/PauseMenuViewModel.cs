using System;
using Core.Signals;
using Infrastructure.InputLogic;
using Zenject;

namespace UI.ViewModels
{
    public class PauseMenuViewModel
    {
        private readonly SignalBus _signalBus;
        private readonly InputSwitcher _inputSwitcher;

        public InputType CurrentInputType => _inputSwitcher.CurrentType;
        public event Action OnInputTypeChanged;

        public PauseMenuViewModel(SignalBus signalBus, InputSwitcher inputSwitcher)
        {
            _signalBus = signalBus;
            _inputSwitcher = inputSwitcher;

            _inputSwitcher.OnInputChanged += _ => OnInputTypeChanged?.Invoke();
        }

        public void Resume() => _signalBus.Fire(new GameResumedSignal());
        public void SwitchToKeyboard() => _inputSwitcher.UseKeyboard();
        public void SwitchToGamepad() => _inputSwitcher.UseGamepad();
        public void SwitchToJoystick() => _inputSwitcher.UseJoystick();
    }
}
