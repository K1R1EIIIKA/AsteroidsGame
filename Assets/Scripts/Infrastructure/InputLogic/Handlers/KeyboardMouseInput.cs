using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.InputLogic.Handlers
{
    public class KeyboardMouseInput : IInputHandler
    {
        public Vector2 Direction { get; private set; }
        public float Rotation { get; private set; }
        public bool IsThrusting { get; private set; }
        public bool IsShootPressed { get; private set; }
        public bool IsLaserPressed { get; private set; }
        public bool IsPausePressed { get; private set; }

        public void UpdateInput()
        {
            Rotation = Input.GetAxis("Horizontal");
            IsThrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            IsShootPressed = Input.GetKey(KeyCode.Space);
            IsLaserPressed = Input.GetKeyDown(KeyCode.LeftShift);
            IsPausePressed = Input.GetKeyDown(KeyCode.Escape);
            Direction = IsThrusting ? Vector2.up : Vector2.zero;
        }
    }

}
