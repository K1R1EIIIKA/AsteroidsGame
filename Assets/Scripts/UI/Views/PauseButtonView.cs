using Core.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class PauseButtonView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()  => _pauseButton.onClick.AddListener(OnPauseClicked);
        private void OnDisable() => _pauseButton.onClick.RemoveListener(OnPauseClicked);

        private void OnPauseClicked() =>
            _signalBus.Fire(new GamePausedSignal());
    }
}
