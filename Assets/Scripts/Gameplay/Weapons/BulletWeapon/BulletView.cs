using Core.Enums;
using Gameplay.Collisions;
using Infrastructure.Physics;
using UnityEngine;

namespace Gameplay.Weapons.BulletWeapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class BulletView : MonoBehaviour
    {
        private PhysicsWorld _physicsWorld;

        public Bullet Bullet { get; private set; }
        public PhysicsBody PhysicsBody { get; private set; }

        private void Awake()
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
        }

        public void Initialize(Bullet bullet, PhysicsWorld physicsWorld)
        {
            Bullet = bullet;
            _physicsWorld = physicsWorld;

            var col = GetComponent<CircleCollider2D>();

            PhysicsBody = _physicsWorld.CreateBody(
                bullet,
                bullet.Position,
                col.radius,
                PhysicsLayer.Bullet);
            
            transform.position = new Vector3(
                PhysicsBody.Position.x,
                PhysicsBody.Position.y,
                0f);

            PhysicsBody.SetVelocity(bullet.Velocity);
            Bullet.SetPhysicsBody(PhysicsBody);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (PhysicsBody != null)
                _physicsWorld.RemoveBody(PhysicsBody);

            Bullet = null;
            PhysicsBody = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Bullet == null || !Bullet.IsActive)
            {
                Deactivate();
                return;
            }

            transform.position = new Vector3(
                PhysicsBody.Position.x,
                PhysicsBody.Position.y,
                0f);
        }
    }
}
