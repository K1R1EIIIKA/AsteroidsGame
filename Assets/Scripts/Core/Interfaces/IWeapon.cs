using UnityEngine;

namespace Core.Interfaces
{
    public interface IWeapon
    {
        void Shoot(Vector2 position, float angle);
        void Tick(float delta);
    }
}
