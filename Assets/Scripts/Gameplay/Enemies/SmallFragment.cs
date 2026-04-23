using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Infrastructure.Data.Configs;
using UnityEngine;
using Zenject;

namespace Gameplay.Enemies
{
    public class SmallFragment : IEnemy
    {
        private readonly SmallFragmentConfig _config;
        private readonly SignalBus _signalBus;
        private readonly IMovementStrategy _movement;

        private Vector2 _position;
        private Vector2 _velocity;

        public EnemyType Type => EnemyType.SmallFragment;
        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float Radius => _config.Radius;
        public bool IsActive { get; private set; }

        public SmallFragment(SmallFragmentConfig config, SignalBus signalBus, IMovementStrategy movement)
        {
            _config = config;
            _signalBus = signalBus;
            _movement = movement;
        }

        public void Initialize(Vector2 position, Vector2 velocity)
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
                    Reward = _config.Reward,
                    Position = _position,
                    Velocity = _velocity,
                    FragmentCount = 0,
                    EnemyOwner = this
                });
        }

        public void SetPosition(Vector2 position) => _position = position;
        public void SetVelocity(Vector2 velocity) => _velocity = velocity;
    }
}
