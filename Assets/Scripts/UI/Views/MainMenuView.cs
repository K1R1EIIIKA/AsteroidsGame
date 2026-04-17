using Core.Signals;
using UI.ViewModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class MainMenuView : BaseView
    {
        [SerializeField] private Button _startButton;

        private MainMenuViewModel _viewModel;

        [Inject]
        public void Construct(MainMenuViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnEnable()  => _startButton.onClick.AddListener(OnStartClicked);
        private void OnDisable() => _startButton.onClick.RemoveListener(OnStartClicked);

        private void OnStartClicked() =>
            _viewModel.StartGame();  
    }
}
