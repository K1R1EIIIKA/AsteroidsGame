using Core.Enums;
using Core.Interfaces;
using Gameplay.Collisions;
using Infrastructure.Physics;
using UnityEngine;
using Zenject;

namespace Gameplay.PlayerLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class ShipView : MonoBehaviour
    {
        private IShip _ship;
        private PhysicsWorld _physicsWorld;
        private PhysicsBody _physicsBody;

        [Inject]
        public void Construct(IShip ship, PhysicsWorld physicsWorld)
        {
            _ship = ship;
            _physicsWorld = physicsWorld;
        }

        private void Awake()
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
        }

        private void Start()
        {
            transform.position = Vector3.zero;

            _physicsBody = _physicsWorld.CreateBody(
                _ship,
                Vector2.zero,
                GetComponent<CircleCollider2D>().radius,
                PhysicsLayer.Ship);

            _physicsWorld.RegisterShipBody(_physicsBody);
            _ship.SetPosition(Vector2.zero);

            if (_ship is ShipFacade facade)
                facade.SetPhysicsBody(_physicsBody);
        }

        private void Update()
        {
            if (_physicsBody == null) return;

            transform.position = new Vector3(
                _physicsBody.Position.x,
                _physicsBody.Position.y,
                0f);

            var angle = _ship.RotationAngle;
            if (!float.IsNaN(angle) && !float.IsInfinity(angle))
                transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        }

        private void OnDestroy()
        {
            if (_physicsBody != null)
                _physicsWorld.RemoveBody(_physicsBody);
        }
    }
}
