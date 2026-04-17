using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.InputLogic.Handlers
{
    public class GamepadInput : IInputHandler
    {
        public Vector2 Direction { get; private set; }
        public float Rotation { get; private set; }
        public bool IsThrusting { get; private set; }
        public bool IsShootPressed { get; private set; }
        public bool IsLaserPressed { get; private set; }
        public bool IsPausePressed { get; private set; }

        public void UpdateInput()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            Direction = new Vector2(horizontal, vertical);
            Rotation = -horizontal;
            IsThrusting = Direction.magnitude > 0.1f;
            IsShootPressed = Input.GetButton("Fire2");
            IsLaserPressed = Input.GetButtonDown("Fire1");

            IsPausePressed = Input.GetKeyDown(KeyCode.JoystickButton9) || Input.GetKeyDown(KeyCode.JoystickButton7);
        }
    }
}
