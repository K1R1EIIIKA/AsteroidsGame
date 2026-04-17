using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.Physics
{
    public class PhysicsBody : IPhysicsBody
    {
        public bool IsActive { get; private set; } = true;

        public PhysicsLayer Layer { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float Radius { get; private set; }
        public object Owner { get; private set; }

        public PhysicsBody(object owner, Vector2 position, float radius, PhysicsLayer layer)
        {
            Owner = owner;
            Position = position;
            Radius = radius;
            Layer = layer;
        }

        public void Deactivate() => IsActive = false;
        public void AddForce(Vector2 force) => Velocity += force;
        public void SetVelocity(Vector2 velocity) => Velocity = velocity;
        public void SetPosition(Vector2 position) => Position = position;

        public void Tick(float delta)
        {
            Position += Velocity * delta;
        }
    }
}
