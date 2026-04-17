using Core.Enums;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IEnemy
    {
        EnemyType Type { get; }
        Vector2 Position { get; }
        Vector2 Velocity { get; }
        float Radius { get; }
        bool IsActive { get; }

        void Tick(float delta);
        void TakeDamage();
        void SetPosition(Vector2 position);
        void SetVelocity(Vector2 velocity);
    }
}
