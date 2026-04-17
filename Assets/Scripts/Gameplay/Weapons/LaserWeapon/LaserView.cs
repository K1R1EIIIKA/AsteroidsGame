using UnityEngine;

namespace Gameplay.Weapons.LaserWeapon
{
    public class LaserView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Laser _laser;

        public void Initialize(Laser laser)
        {
            _laser = laser;
        }

        private void Update()
        {
            if (_laser == null || !_laser.IsActive)
            {
                _spriteRenderer.enabled = false;
                return;
            }

            _spriteRenderer.enabled = true;

            var origin    = _laser.Origin;
            var direction = new Vector2(
                Mathf.Sin(_laser.Angle * Mathf.Deg2Rad),
                Mathf.Cos(_laser.Angle * Mathf.Deg2Rad));

            var center = origin + direction * (_laser.Length * 0.5f);

            transform.position = new Vector3(center.x, center.y, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, -_laser.Angle);

            _spriteRenderer.size = new Vector2(
                _spriteRenderer.size.x,
                _laser.Length);
        }
    }
}
