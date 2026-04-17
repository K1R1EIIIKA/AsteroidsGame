using Infrastructure.InputLogic.Handlers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Views
{
    public class ShootButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private VirtualJoystickInput _joystickInput;

        [Inject]
        public void Construct(VirtualJoystickInput joystickInput)
        {
            _joystickInput = joystickInput;
        }

        public void OnPointerDown(PointerEventData eventData) =>
            _joystickInput.SetShoot(true);

        public void OnPointerUp(PointerEventData eventData) =>
            _joystickInput.SetShoot(false);
    }
}
