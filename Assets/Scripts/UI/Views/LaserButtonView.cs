using Infrastructure.InputLogic.Handlers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Views
{
    public class LaserButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private VirtualJoystickInput _joystickInput;

        [Inject]
        public void Construct(VirtualJoystickInput joystickInput)
        {
            _joystickInput = joystickInput;
        }

        public void OnPointerDown(PointerEventData eventData) =>
            _joystickInput.SetLaser(true);

        public void OnPointerUp(PointerEventData eventData) =>
            _joystickInput.SetLaser(false);
    }
}
