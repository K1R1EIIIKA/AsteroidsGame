using UnityEngine;

namespace Core.Interfaces
{
    public interface IShip
    {
        Vector2 Position { get; }
        float RotationAngle { get; }
        float Speed { get; }
        int Health { get; }
        int LaserCharges { get; }
        float LaserRechargeProgress { get; }
        bool IsInvincible { get; }

        void Thrust(float delta, Vector2 direction = default);
        void Rotate(float delta);
        void RotateTowards(float targetAngle, float delta);
        void Shoot();
        void ShootLaser();

        void Tick(float delta);
        void TakeDamage(Vector2 hitVelocity);
        void SetPosition(Vector2 position);
        void Reset();
        void SetHealth(int health);
        void ResetWeapons();
    }
}
