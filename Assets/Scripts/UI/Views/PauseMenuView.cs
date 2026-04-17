using Infrastructure.InputLogic;
using UI.ViewModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class PauseMenuView : BaseView
    {
        [SerializeField] private Button _resumeButton;

        [Header("Input Switch")]
        [SerializeField] private Button _keyboardButton;
        [SerializeField] private Button _gamepadButton;
        [SerializeField] private Button _joystickButton;
        [SerializeField] private GameObject _joystickContainer;

        [Header("Highlights")]
        [SerializeField] private Image _keyboardHighlight;
        [SerializeField] private Image _gamepadHighlight;
        [SerializeField] private Image _joystickHighlight;

        private PauseMenuViewModel _viewModel;

        [Inject]
        public void Construct(PauseMenuViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(OnResumeClicked);
            _keyboardButton.onClick.AddListener(_viewModel.SwitchToKeyboard);
            _gamepadButton.onClick.AddListener(_viewModel.SwitchToGamepad);

            if (_joystickContainer != null)
                _joystickContainer.SetActive(Application.isMobilePlatform);

            if (_joystickButton != null)
                _joystickButton.onClick.AddListener(_viewModel.SwitchToJoystick);

            _viewModel.OnInputTypeChanged += RefreshHighlights;
            RefreshHighlights();
        }

        private void OnDisable()
        {
            _resumeButton.onClick.RemoveListener(OnResumeClicked);
            _keyboardButton.onClick.RemoveListener(_viewModel.SwitchToKeyboard);
            _gamepadButton.onClick.RemoveListener(_viewModel.SwitchToGamepad);

            if (_joystickButton != null)
                _joystickButton.onClick.RemoveListener(_viewModel.SwitchToJoystick);

            _viewModel.OnInputTypeChanged -= RefreshHighlights;
        }

        private void RefreshHighlights()
        {
            var type = _viewModel.CurrentInputType;

            if (_keyboardHighlight != null)
                _keyboardHighlight.enabled = type == InputType.Keyboard;
            if (_gamepadHighlight != null)
                _gamepadHighlight.enabled = type == InputType.Gamepad;
            if (_joystickHighlight != null)
                _joystickHighlight.enabled = type == InputType.Joystick;
        }

        private void OnResumeClicked() => _viewModel.Resume();
    }
}
