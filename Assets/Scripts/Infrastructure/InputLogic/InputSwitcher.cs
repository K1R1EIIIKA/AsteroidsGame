using System;
using Core.Interfaces;
using Infrastructure.InputLogic.Handlers;
using UnityEngine;

namespace Infrastructure.InputLogic
{
    public class InputSwitcher
    {
        private readonly KeyboardMouseInput _keyboard;
        private readonly GamepadInput _gamepad;
        private readonly VirtualJoystickInput _joystick;

        public IInputHandler Current { get; private set; }
        public InputType CurrentType { get; private set; }

        public event Action<InputType> OnInputChanged;

        public InputSwitcher(
            KeyboardMouseInput keyboard,
            GamepadInput gamepad,
            VirtualJoystickInput joystick)
        {
            _keyboard = keyboard;
            _gamepad = gamepad;
            _joystick = joystick;

            if (Application.isMobilePlatform)
                SwitchTo(InputType.Joystick);
            else
                SwitchTo(InputType.Keyboard);
        }

        public void UseKeyboard() => SwitchTo(InputType.Keyboard);
        public void UseGamepad() => SwitchTo(InputType.Gamepad);
        public void UseJoystick() => SwitchTo(InputType.Joystick);

        public void SwitchTo(InputType type)
        {
            CurrentType = type;
            Current = type switch
                {
                    InputType.Keyboard => _keyboard,
                    InputType.Gamepad => _gamepad,
                    InputType.Joystick => _joystick,
                    _ => _keyboard
                };

            OnInputChanged?.Invoke(type);
            Debug.Log($"[Input] Switched to {type}");
        }

        public void Tick()
        {
            var joysticks = Input.GetJoystickNames();
            var hasGamepad = joysticks.Length > 0 && !string.IsNullOrEmpty(joysticks[0]);

            if (hasGamepad && CurrentType == InputType.Keyboard)
            {
                SwitchTo(InputType.Gamepad);
            }
            else if (!hasGamepad && CurrentType == InputType.Gamepad)
            {
                SwitchTo(InputType.Keyboard);
            }
        }
    }

    public enum InputType
    {
        Keyboard,
        Gamepad,
        Joystick
    }
}
