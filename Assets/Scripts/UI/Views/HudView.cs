using TMPro;
using UI.ViewModels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Views
{
    public class HudView : BaseView
    {
        [SerializeField] private TextMeshProUGUI _positionText;
        [SerializeField] private TextMeshProUGUI _angleText;
        [SerializeField] private TextMeshProUGUI _speedText;
        [SerializeField] private TextMeshProUGUI _scoreText;

        [Header("Health")]
        [SerializeField] private Image[] _hearts;

        [Header("Laser")]
        [SerializeField] private Image[] _laserChargeIcons;
        [SerializeField] private Image _laserRechargeFill;

        private HudViewModel _viewModel;

        [Inject]
        public void Construct(HudViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void OnEnable()
        {
            _viewModel.OnChanged += Refresh;
        }

        private void OnDisable()
        {
            _viewModel.OnChanged -= Refresh;
        }

        private void Refresh()
        {
            _positionText.text =
                $"X: {_viewModel.Position.x:F0}  Y: {_viewModel.Position.y:F0}";
            _angleText.text =
                $"{_viewModel.Angle:F1}°";
            _speedText.text =
                $"{_viewModel.Speed:F1}";
            _scoreText.text =
                $"{_viewModel.Score}";

            RefreshHearts();
            RefreshLaser();
        }

        private void RefreshHearts()
        {
            for (int i = 0; i < _hearts.Length; i++)
                _hearts[i].enabled = i < _viewModel.Health;
        }

        private void RefreshLaser()
        {
            for (int i = 0; i < _laserChargeIcons.Length; i++)
            {
                _laserChargeIcons[i].color = i < _viewModel.LaserCharges ? Color.white : new Color(1f, 1f, 1f, 0.2f);
            }

            _laserRechargeFill.fillAmount = _viewModel.LaserRechargeProgress;
        }
    }
}
