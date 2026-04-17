using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.InputLogic.Handlers
{
    public class VirtualJoystickInput : IInputHandler
    {
        public Vector2 Direction { get; private set; }
        public float Rotation { get; private set; }
        public bool IsThrusting { get; private set; }
        public bool IsShootPressed { get; private set; }
        public bool IsLaserPressed { get; private set; }
        public bool IsPausePressed { get; private set; }

        private Vector2 _joystickValue;
        private bool _shootPressed;
        private bool _laserPressed;

        public void SetJoystick(Vector2 value) => _joystickValue = value;
        public void SetShoot(bool pressed) => _shootPressed = pressed;
        public void SetLaser(bool pressed) => _laserPressed = pressed;

        public void UpdateInput()
        {
            Direction = _joystickValue;
            Rotation = -_joystickValue.x;
            IsThrusting = _joystickValue.magnitude > 0.1f;
            IsShootPressed = _shootPressed;
            IsLaserPressed = _laserPressed;
            IsPausePressed = false;

            _laserPressed = false;
        }
    }
}
