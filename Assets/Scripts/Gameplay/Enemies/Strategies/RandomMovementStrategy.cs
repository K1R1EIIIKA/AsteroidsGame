using Core.Interfaces;
using UnityEngine;

namespace Gameplay.Enemies.Strategies
{
    public class RandomMovementStrategy : IMovementStrategy
    {
        public Vector2 CalculateVelocity(Vector2 currentVelocity, Vector2 position, float delta)
        {
            return currentVelocity;
        }
    }
}
