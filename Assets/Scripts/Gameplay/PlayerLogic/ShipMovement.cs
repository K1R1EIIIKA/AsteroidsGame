using Infrastructure.Data;
using Infrastructure.Physics;
using UnityEngine;

namespace Gameplay.PlayerLogic
{
    public class ShipMovement
    {
        private readonly ConfigService _config;

        private Vector2 _velocity;
        private float _angle;
        private Vector2 _position;
        private PhysicsBody _physicsBody;

        public Vector2 Position => _position;
        public Vector2 Velocity => _velocity;
        public float RotationAngle => _angle;
        public float Speed => _velocity.magnitude;

        public ShipMovement(ConfigService config)
        {
            _config = config;
        }

        public void SetPhysicsBody(PhysicsBody body) => _physicsBody = body;

        public void Thrust(float delta, Vector2 direction = default)
        {
            var moveDirection = direction != default
                ? direction.normalized
                : new Vector2(
                    Mathf.Sin(_angle * Mathf.Deg2Rad),
                    Mathf.Cos(_angle * Mathf.Deg2Rad));

            _velocity += moveDirection * _config.Ship.ThrustForce * delta;

            if (_velocity.magnitude > _config.Ship.MaxSpeed)
                _velocity = _velocity.normalized * _config.Ship.MaxSpeed;
        }

        public void Rotate(float delta)
        {
            _angle += _config.Ship.RotationSpeed * delta;
            _angle %= 360f;
            if (_angle < 0f) _angle += 360f;
        }

        public void RotateTowards(float targetAngle, float delta)
        {
            var turnSpeed = _config.Ship.RotationSpeed * 2 * delta;
            _angle = Mathf.MoveTowardsAngle(_angle, targetAngle, turnSpeed);
        }

        public void Tick(float delta)
        {
            _velocity *= Mathf.Pow(_config.Ship.Drag, delta * 60f);

            if (_physicsBody != null)
            {
                _position = _physicsBody.Position;
                _physicsBody.SetVelocity(_velocity);
            }
            else
            {
                _position += _velocity * delta;
            }
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            _physicsBody?.SetPosition(position);
        }

        public void SetVelocity(Vector2 velocity)
        {
            _velocity = velocity;
            _physicsBody?.SetVelocity(velocity);
        }

        public void Reset()
        {
            _velocity = Vector2.zero;
            _angle = 0f;
            _position = Vector2.zero;
            _physicsBody?.SetPosition(Vector2.zero);
            _physicsBody?.SetVelocity(Vector2.zero);
        }
    }
}
