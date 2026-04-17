using UnityEngine;

namespace Infrastructure.Physics
{
    public class CollisionResolver
    {
        private const float MinBounceSpeed = 5f;

        public (Vector2 velocityA, Vector2 velocityB) Resolve(PhysicsBody a, PhysicsBody b)
        {
            var normal = (b.Position - a.Position).normalized;
            var relative = a.Velocity - b.Velocity;
            var speed = Vector2.Dot(relative, normal);

            if (speed > 0f)
                return (a.Velocity, b.Velocity);

            var impulse = speed;
            var velocityA = a.Velocity - impulse * normal;
            var velocityB = b.Velocity + impulse * normal;

            if (velocityA.magnitude < MinBounceSpeed)
                velocityA = -normal * MinBounceSpeed;

            if (velocityB.magnitude < MinBounceSpeed)
                velocityB = normal * MinBounceSpeed;

            return (velocityA, velocityB);
        }
    }
}
