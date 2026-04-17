using Infrastructure.InputLogic.Handlers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI.Views
{
    public class VirtualJoystickView : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform _handle;

        private VirtualJoystickInput _joystickInput;
        private Vector2 _center;
        private float _radius;

        [Inject]
        public void Construct(VirtualJoystickInput joystickInput)
        {
            _joystickInput = joystickInput;
        }

        private void Start()
        {
            _radius = _background.rect.width * 0.5f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _radius = _background.rect.width * 0.5f;
            OnDrag(eventData);
        }

        private Vector2 GetHandleOffset()
        {
            var bgOffset = new Vector2(
                (0.5f - _background.pivot.x) * _background.rect.width,
                (0.5f - _background.pivot.y) * _background.rect.height);

            var handleOffset = new Vector2(
                _handle.pivot.x * _handle.rect.width,
                _handle.pivot.y * _handle.rect.height);

            return bgOffset - handleOffset;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_radius <= 0f) return;

            var canvas = _background.GetComponentInParent<Canvas>();
            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null : canvas.worldCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _background, eventData.position, camera, out var localPoint);

            var bgOffset = new Vector2(
                (0.5f - _background.pivot.x) * _background.rect.width,
                (0.5f - _background.pivot.y) * _background.rect.height);

            localPoint -= bgOffset;

            var clamped = Vector2.ClampMagnitude(localPoint, _radius);

            _handle.anchoredPosition = clamped + GetHandleOffset();
            _joystickInput.SetJoystick(clamped / _radius);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _handle.anchoredPosition = GetHandleOffset();
            _joystickInput.SetJoystick(Vector2.zero);
        }
    }
}
