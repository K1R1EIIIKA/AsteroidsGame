using Core.Enums;
using Core.Signals;
using Infrastructure.InputLogic;
using UnityEngine;
using Zenject;

namespace UI.Views
{
    public class MobileControlsView : MonoBehaviour
    {
        private InputSwitcher _inputSwitcher;
        private SignalBus _signalBus;
        private GameStateType _currentState;

        [Inject]
        public void Construct(InputSwitcher inputSwitcher, SignalBus signalBus)
        {
            _inputSwitcher = inputSwitcher;
            _signalBus = signalBus;
        }

        private void Start()
        {
            _inputSwitcher.OnInputChanged += OnInputChanged;
            _signalBus.Subscribe<GameStateChangedSignal>(OnStateChanged);
            Refresh();
        }

        private void OnDestroy()
        {
            _inputSwitcher.OnInputChanged -= OnInputChanged;
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnStateChanged);
        }

        private void OnInputChanged(InputType _)
        {
            Refresh();
        }

        private void OnStateChanged(GameStateChangedSignal signal)
        {
            _currentState = signal.NewState;
            Refresh();
        }

        private void Refresh()
        {
            var isJoystick = _inputSwitcher.CurrentType == InputType.Joystick;
            var isGameLoop = _currentState == GameStateType.GameLoop;

            gameObject.SetActive(isJoystick && isGameLoop);
        }
    }
}
