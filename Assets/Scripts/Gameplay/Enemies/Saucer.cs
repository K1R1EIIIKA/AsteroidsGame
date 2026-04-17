using Core.Configs;
using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Infrastructure.Data;
using Infrastructure.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class Saucer : IEnemy
    {
        private readonly ConfigService _config;
        private readonly SignalBus _signalBus;
        private readonly IMovementStrategy _movement;
        private const float BounceDuration = 1.5f;

        private Vector2 _position;
        private Vector2 _velocity;
        private float _bounceTimer;
        
        public ConfigService Config => _config; 
        public bool IsBouncing => _bounceTimer > 0f;
        public EnemyType Type => EnemyType.Saucer;
        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float Radius => _config.Enemy.saucer.radius;
        public bool IsActive { get; private set; }

        public Saucer(
            ConfigService config,
            SignalBus signalBus,
            IMovementStrategy movement)
        {
            _config = config;
            _signalBus = signalBus;
            _movement = movement;
        }

        public void Activate(Vector2 position)
        {
            _position = position;
            _velocity = Vector2.zero;
            IsActive = true;
        }

        public void ApplyBounce(Vector2 bounceVelocity)
        {
            _velocity    = bounceVelocity;
            _bounceTimer = BounceDuration;
        }

        public void Tick(float delta)
        {
            if (!IsActive) return;

            if (_bounceTimer > 0f)
            {
                _bounceTimer -= delta;
            }
            else
            {
                _velocity = _movement.CalculateVelocity(_velocity, _position, delta);
            }
        }

        public void TakeDamage()
        {
            IsActive = false;
            _signalBus.Fire(new EnemyDestroyedSignal
                {
                    EnemyType = Type,
                    Reward = _config.Enemy.saucer.reward,
                    Position = _position,
                    Velocity = _velocity,
                    FragmentCount = 0
                });
        }

        public void SetPosition(Vector2 position) => _position = position;
        public void SetVelocity(Vector2 velocity) => _velocity = velocity;
    }
}
