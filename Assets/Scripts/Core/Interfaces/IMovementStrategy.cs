using UnityEngine;

namespace Core.Interfaces
{
    public interface IMovementStrategy
    {
        Vector2 CalculateVelocity(Vector2 currentVelocity, Vector2 position, float delta);
    }
}
