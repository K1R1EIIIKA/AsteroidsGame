using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Infrastructure.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class Fragment : IEnemy
    {
        private readonly FragmentConfig _config;
        private readonly SignalBus _signalBus;
        private readonly IMovementStrategy _movement;

        private Vector2 _position;
        private Vector2 _velocity;

        public EnemyType Type => EnemyType.Fragment;
        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float Radius => _config.radius;
        public bool IsActive { get; private set; }

        public Fragment(FragmentConfig config, SignalBus signalBus, IMovementStrategy movement)
        {
            _config = config;
            _signalBus = signalBus;
            _movement = movement;
        }

        public void Activate(Vector2 position, Vector2 velocity)
        {
            _position = position;
            _velocity = velocity;
            IsActive = true;
        }

        public void Tick(float delta)
        {
            if (!IsActive) return;
            
            _velocity = _movement.CalculateVelocity(_velocity, _position, delta);
            _position += _velocity * delta;
        }

        public void TakeDamage()
        {
            IsActive = false;
            _signalBus.Fire(new EnemyDestroyedSignal
                {
                    EnemyType = Type,
                    Reward = _config.reward,
                    Position = _position,
                    Velocity = _velocity,
                    FragmentCount = _config.fragmentCount,
                    FragmentSpeedMultiplier = _config.fragmentSpeedMultiplier
                });
        }

        public void SetPosition(Vector2 position) => _position = position;
        public void SetVelocity(Vector2 velocity) => _velocity = velocity;

    }
}
