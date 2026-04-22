using Core.Interfaces;
using Infrastructure.Data.Configs;
using UnityEngine;

namespace Gameplay.Enemies.Strategies
{
    public class ChaseMovementStrategy : IMovementStrategy
    {
        private readonly IShip _ship;
        private readonly SaucerConfig _config;

        public ChaseMovementStrategy(IShip ship, SaucerConfig config)
        {
            _ship = ship;
            _config = config;
        }

        public Vector2 CalculateVelocity(Vector2 currentVelocity, Vector2 position, float delta)
        {
            var direction = (_ship.Position - position).normalized;
            var targetVelocity = direction * _config.MaxChaseSpeed;

            var turnSpeed = 5f; 
            var velocity = Vector2.Lerp(currentVelocity, targetVelocity, turnSpeed * delta);

            return velocity;
        }
    }
}
