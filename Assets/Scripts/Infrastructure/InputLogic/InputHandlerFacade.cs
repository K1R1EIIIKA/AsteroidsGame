using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.InputLogic
{
    public class InputHandlerFacade : IInputHandler
    {
        private readonly InputSwitcher _switcher;

        public InputHandlerFacade(InputSwitcher switcher) => _switcher = switcher;

        public Vector2 Direction => _switcher.Current.Direction;
        public float Rotation => _switcher.Current.Rotation;
        public bool IsThrusting => _switcher.Current.IsThrusting;
        public bool IsShootPressed => _switcher.Current.IsShootPressed;
        public bool IsLaserPressed => _switcher.Current.IsLaserPressed;
        public bool IsPausePressed => _switcher.Current.IsPausePressed;

        public void UpdateInput() => _switcher.Current.UpdateInput();
    }
}
