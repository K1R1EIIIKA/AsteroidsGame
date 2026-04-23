using System.Threading;
using Core.Configs;
using Core.Signals;
using Cysharp.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayerLogic
{
    public class ShipVfx : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private ParticleSystem _invincibilityRing;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private SignalBus _signalBus;
        private CancellationTokenSource _blinkCts;
        private float _invincibilityDuration;

        [Inject]
        public void Construct(SignalBus signalBus, ConfigService config)
        {
            _signalBus = signalBus;
            _invincibilityDuration = config.Ship.InvincibilityDuration;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerDamagedSignal>(OnPlayerDamaged);
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Subscribe<PlayerInvincibilityEndedSignal>(OnInvincibilityEnded);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerDamagedSignal>(OnPlayerDamaged);
            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Unsubscribe<PlayerInvincibilityEndedSignal>(OnInvincibilityEnded);
        }

        private void OnPlayerDamaged(PlayerDamagedSignal signal)
        {
            if (_hitParticles != null)
            {
                _hitParticles.Stop();
                _hitParticles.Play();
            }

            if (_invincibilityRing != null)
            {
                _invincibilityRing.Stop();
                _invincibilityRing.Play();
            }

            StartBlinking();
        }

        private void OnInvincibilityEnded(PlayerInvincibilityEndedSignal signal)
        {
            StopBlinking();

            if (_invincibilityRing != null)
                _invincibilityRing.Stop();
        }

        private void OnPlayerDied(PlayerDiedSignal signal)
        {
            StopBlinking();

            if (_hitParticles != null)
            {
                _hitParticles.Stop();
                _hitParticles.Play();
            }

            if (_invincibilityRing != null)
                _invincibilityRing.Stop();
        }

        private void StartBlinking()
        {
            StopBlinking();
            _blinkCts = new CancellationTokenSource();
            BlinkAsync(_blinkCts.Token).Forget();
        }

        private void StopBlinking()
        {
            _blinkCts?.Cancel();
            _blinkCts?.Dispose();
            _blinkCts = null;

            if (_spriteRenderer != null)
                _spriteRenderer.enabled = true;
        }

        private async UniTask BlinkAsync(CancellationToken token)
        {
            var startTime = Time.time;
            var endTime   = startTime + _invincibilityDuration;

            while (!token.IsCancellationRequested)
            {
                var elapsed  = Time.time - startTime;
                var progress = Mathf.Clamp01(elapsed / _invincibilityDuration);

                var intervalMs = (int)Mathf.Lerp(500f, 100f, progress);

                if (_spriteRenderer != null)
                    _spriteRenderer.enabled = false;

                await UniTask.Delay(intervalMs / 2, cancellationToken: token)
                    .SuppressCancellationThrow();

                if (token.IsCancellationRequested) break;

                if (_spriteRenderer != null)
                    _spriteRenderer.enabled = true;

                await UniTask.Delay(intervalMs / 2, cancellationToken: token)
                    .SuppressCancellationThrow();
            }

            if (_spriteRenderer != null)
                _spriteRenderer.enabled = true;
        }
    }
}
