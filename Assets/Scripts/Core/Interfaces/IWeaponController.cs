using UnityEngine;

namespace Core.Interfaces
{
    public interface IWeaponController
    {
        int LaserCharges { get; }
        float LaserRechargeProgress { get; }

        void Shoot(Vector2 position, float angle);
        void ShootLaser(Vector2 position, float angle);
        void Tick(float delta);
        void ResetBullets();
    }
}
