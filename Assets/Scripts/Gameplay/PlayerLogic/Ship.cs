using System.Threading;
using Core.Interfaces;
using Core.Signals;
using Cysharp.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Physics;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayerLogic
{
    public class Ship : IShip
    {
        private readonly ConfigService _config;
        private readonly SignalBus _signalBus;
        private readonly IWeaponController _weaponController;

        private Vector2 _position;
        private Vector2 _velocity;
        private float _angle;
        private int _health;
        private bool _isInvincible;

        private CancellationTokenSource _invincibilityCts;
        private PhysicsBody _physicsBody;

        public bool IsInvincible => _isInvincible;
        public Vector2 Position => _position;
        public float RotationAngle => _angle;
        public float Speed => _velocity.magnitude;
        public int Health => _health;
        public int LaserCharges => _weaponController.LaserCharges;
        public float LaserRechargeProgress => _weaponController.LaserRechargeProgress;

        public Ship(ConfigService config, SignalBus signalBus, IWeaponController weaponController)
        {
            _config = config;
            _signalBus = signalBus;
            _weaponController = weaponController;
            _health = config.Ship.MaxHealth;
        }

        public void SetPhysicsBody(PhysicsBody physicsBody)
        {
            _physicsBody = physicsBody;
        }

        public void Thrust(float delta, Vector2 direction = default)
        {
            if (_isInvincible) return;

            var moveDirection = direction != default
                ? direction.normalized
                : new Vector2(
                    Mathf.Sin(_angle * Mathf.Deg2Rad),
                    Mathf.Cos(_angle * Mathf.Deg2Rad));

            _velocity += moveDirection * _config.Ship.ThrustForce * delta;

            if (_velocity.magnitude > _config.Ship.MaxSpeed)
                _velocity = _velocity.normalized * _config.Ship.MaxSpeed;
        }

        public void Rotate(float delta)
        {
            if (_isInvincible) return;
            _angle += _config.Ship.RotationSpeed * delta;

            _angle %= 360f;
            if (_angle < 0f) _angle += 360f;
        }

        public void RotateTowards(float targetAngle, float delta)
        {
            if (_isInvincible) return;

            var turnSpeed = _config.Ship.RotationSpeed * 2 * delta;
            _angle = Mathf.MoveTowardsAngle(_angle, targetAngle, turnSpeed);
        }
        
        public void Shoot()
        {
            if (_isInvincible) return;
            _weaponController.Shoot(_position, _angle);
        }
        public void ShootLaser()
        {
            if (_isInvincible) return;
            _weaponController.ShootLaser(_position, _angle);
        }

        public void Tick(float delta)
        {
            _velocity *= Mathf.Pow(_config.Ship.Drag, delta * 60f);

            if (_physicsBody != null)
            {
                _position = _physicsBody.Position;

                _physicsBody.SetVelocity(_velocity);
            }
            else
            {
                _position += _velocity * delta;
            }

            _weaponController.Tick(delta);
        }

        public void TakeDamage(Vector2 hitVelocity)
        {
            if (_isInvincible) return;

            _health--;
            _velocity = hitVelocity;

            _signalBus.Fire(new PlayerDamagedSignal { HealthRemaining = _health });

            if (_health <= 0)
            {
                _signalBus.Fire(new PlayerDiedSignal());
                return;
            }

            StartInvincibilityAsync().Forget();
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            _physicsBody?.SetPosition(position);
        }

        private async UniTaskVoid StartInvincibilityAsync()
        {
            _invincibilityCts?.Cancel();
            _invincibilityCts?.Dispose();
            _invincibilityCts = new CancellationTokenSource();

            _isInvincible = true;

            var cancelled = await UniTask.Delay(
                    (int)(_config.Ship.InvincibilityDuration * 1000),
                    cancellationToken: _invincibilityCts.Token)
                .SuppressCancellationThrow();
            
            if(cancelled) return;
           
            _signalBus.Fire(new PlayerInvincibilityEndedSignal());
            _isInvincible = false;
        }

        public void Reset()
        {
            _health = _config.Ship.MaxHealth;
            _velocity = Vector2.zero;
            _angle = 0f;
            _isInvincible = false;
            _invincibilityCts?.Cancel();
            _invincibilityCts?.Dispose();
            _invincibilityCts = null;
            
            _position = Vector2.zero;
            _physicsBody?.SetPosition(Vector2.zero);
            _physicsBody?.SetVelocity(Vector2.zero);
        }
        
        public void SetHealth(int health)
        {
            _health = Mathf.Clamp(health, 0, _config.Ship.MaxHealth);
            _signalBus.Fire(new PlayerDamagedSignal { HealthRemaining = _health });
            
            _signalBus.Fire(new PlayerInvincibilityEndedSignal());
            _isInvincible = false;
            _invincibilityCts?.Cancel();
            _invincibilityCts?.Dispose();
            _invincibilityCts = null;
        }

        public void ResetWeapons()
        {
            _weaponController.ResetBullets();
            _weaponController.ResetLaser();
        }
    }
}
