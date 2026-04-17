using UnityEngine;

namespace Core.Interfaces
{
    public interface IInputHandler
    {
        Vector2 Direction { get; }
        float Rotation { get; }
        bool IsShootPressed { get; }
        bool IsLaserPressed { get; }
        bool IsPausePressed { get; }
        bool IsThrusting { get; }
        
        void UpdateInput();
    }
}
