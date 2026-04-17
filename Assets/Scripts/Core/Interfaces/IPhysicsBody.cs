using Core.Enums;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IPhysicsBody
    {
        bool IsActive { get; }
        PhysicsLayer Layer { get; }
        Vector2 Position { get; }
        Vector2 Velocity { get; }
        float Radius { get; }
        object Owner { get; }

        void AddForce(Vector2 force);
        void SetVelocity(Vector2 velocity);
        void SetPosition(Vector2 position);
        void Tick(float delta);
    }
}
