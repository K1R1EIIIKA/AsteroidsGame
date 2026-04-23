using System.Threading;
using Core.Signals;
using Cysharp.Threading.Tasks;
using Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayerLogic
{
    public class ShipHealth
    {
        private readonly ConfigService _config;
        private readonly SignalBus _signalBus;

        private int _health;
        private bool _isInvincible;
        private CancellationTokenSource _cts;

        public int Health => _health;
        public bool IsInvincible => _isInvincible;

        public ShipHealth(ConfigService config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            _health = config.Ship.MaxHealth;
        }

        public bool TakeDamage()
        {
            if (_isInvincible) return false;

            _health--;
            _signalBus.Fire(new PlayerDamagedSignal { HealthRemaining = _health });

            if (_health <= 0)
            {
                _signalBus.Fire(new PlayerDiedSignal());
                return false;
            }

            StartInvincibilityAsync().Forget();
            return true;
        }

        public void Set(int health)
        {
            _health = Mathf.Clamp(health, 0, _config.Ship.MaxHealth);
            _signalBus.Fire(new PlayerDamagedSignal { HealthRemaining = _health });

            CancelInvincibility();
            _signalBus.Fire(new PlayerInvincibilityEndedSignal());
        }

        public void Reset()
        {
            _health = _config.Ship.MaxHealth;
            CancelInvincibility();
        }

        private async UniTask StartInvincibilityAsync()
        {
            CancelInvincibility();
            _cts = new CancellationTokenSource();

            _isInvincible = true;

            var cancelled = await UniTask.Delay(
                    (int)(_config.Ship.InvincibilityDuration * 1000),
                    cancellationToken: _cts.Token)
                .SuppressCancellationThrow();

            if (cancelled) return;

            _isInvincible = false;
            _signalBus.Fire(new PlayerInvincibilityEndedSignal());
        }

        private void CancelInvincibility()
        {
            _isInvincible = false;
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
