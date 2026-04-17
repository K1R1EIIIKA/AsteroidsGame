using TMPro;
using UI.ViewModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class GameOverView : BaseView
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _watchAdButton;

        private GameOverViewModel _viewModel;

        [Inject]
        public void Construct(GameOverViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnEnable()
        {
            _viewModel.OnChanged += Refresh;
            _restartButton.onClick.AddListener(OnRestartClicked);
            _watchAdButton.onClick.AddListener(OnWatchAdClicked);
            Refresh();
        }

        private void OnDisable()
        {
            _viewModel.OnChanged -= Refresh;
            _restartButton.onClick.RemoveListener(OnRestartClicked);
            _watchAdButton.onClick.RemoveListener(OnWatchAdClicked);
        }

        private void Refresh()
        {
            _scoreText.text = $"{_viewModel.FinalScore}";
        }

        private void OnRestartClicked()
        {
            _viewModel.Restart();  
        }
        
        private void OnWatchAdClicked()
        {
            _viewModel.WatchAd();
        }
    }
}
