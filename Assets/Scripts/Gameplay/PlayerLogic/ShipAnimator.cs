using Core.Interfaces;
using Gameplay.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayerLogic
{
    public class ShipAnimator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private IInputHandler _input;
        private GameConfig _config;

        [Inject]
        public void Construct(IInputHandler input, GameConfig config)
        {
            _input  = input;
            _config = config;
        }

        private void Update()
        {
            if (_spriteRenderer == null) return;

            _spriteRenderer.sprite = _input.IsThrusting ? _config.ShipThrust : _config.ShipIdle;
        }
    }
}
