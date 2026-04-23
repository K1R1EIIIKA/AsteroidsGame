using System.Collections.Generic;
using Core.Interfaces;
using Gameplay.Enemies.Systems;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.Physics;
using UnityEngine;

namespace Gameplay.Collisions
{
    public class LaserCollisionHandler
    {
        private readonly List<(IEnemy enemy, PhysicsBody body)> _hits = new List<(IEnemy, PhysicsBody)>();

        public void CheckCollisions(Laser laser, EnemyPool enemyPool)
        {
            if (!laser.IsActive) return;

            var origin = laser.Origin;
            var direction = new Vector2(
                Mathf.Sin(laser.Angle * Mathf.Deg2Rad),
                Mathf.Cos(laser.Angle * Mathf.Deg2Rad));

            _hits.Clear();

            foreach (var (enemy, view) in enemyPool.Active)
            {
                if (!enemy.IsActive) continue;
                if (view.PhysicsBody == null) continue;

                if (RayIntersectsCircle(
                    origin, direction,
                    laser.Length, laser.Radius,
                    enemy.Position, view.PhysicsBody.Radius))
                {
                    _hits.Add((enemy, view.PhysicsBody));
                }
            }

            foreach (var (enemy, body) in _hits)
            {
                body.Deactivate();
                enemy.TakeDamage();
            }
        }

        private static bool RayIntersectsCircle(
            Vector2 origin, Vector2 direction, float length,
            float rayRadius, Vector2 circleCenter, float circleRadius)
        {
            var toCircle = circleCenter - origin;
            var projectionLength = Vector2.Dot(toCircle, direction);

            if (projectionLength < 0 || projectionLength > length)
                return false;

            var closestPoint = origin + direction * projectionLength;
            var distanceToCircle = Vector2.Distance(closestPoint, circleCenter);

            return distanceToCircle <= (circleRadius + rayRadius);
        }
    }
}