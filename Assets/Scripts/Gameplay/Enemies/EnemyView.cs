using Core.Configs;
using Core.Interfaces;
using Infrastructure.Physics;
using UnityEngine;

namespace Gameplay.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private PhysicsWorld _physicsWorld;
        
        public IEnemy Enemy { get; private set; }
        public PhysicsBody PhysicsBody { get; set; }
        public float ColliderRadius { get; private set; }

        private void Awake()
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;

            var col = GetComponent<CircleCollider2D>();
            col.isTrigger = true;
            ColliderRadius = col.radius;
        }

        public void Initialize(IEnemy enemy, PhysicsWorld physicsWorld, AsteroidSpritesConfig spritesConfig)
        {
            Enemy = enemy;
            _physicsWorld = physicsWorld;
            
            if (spritesConfig != null && _spriteRenderer != null)
            {
                var sprite = spritesConfig.GetRandom(enemy.Type);
                if (sprite != null)
                    _spriteRenderer.sprite = sprite;
            }

            transform.position = new Vector3(
                enemy.Position.x,
                enemy.Position.y,
                0f);

            gameObject.SetActive(true);
        }


        public void Deactivate()
        {
            if (PhysicsBody != null)
                _physicsWorld.RemoveBody(PhysicsBody);

            Enemy = null;
            PhysicsBody = null;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Enemy == null || !Enemy.IsActive)
            {
                Deactivate();
                return;
            }

            transform.position = new Vector3(
                PhysicsBody.Position.x,
                PhysicsBody.Position.y,
                0f);

            Enemy.SetPosition(PhysicsBody.Position);
        }
    }
}
