using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Physics
{
    public class CollisionDetector
    {
        public struct CollisionPair
        {
            public PhysicsBody A;
            public PhysicsBody B;
        }

        private readonly List<CollisionPair> _result = new List<CollisionPair>();

        public List<CollisionPair> Detect(List<PhysicsBody> bodies)
        {
            _result.Clear();

            for (int i = 0; i < bodies.Count; i++)
            {
                if (!bodies[i].IsActive) continue;

                for (int j = i + 1; j < bodies.Count; j++)
                {
                    if (!bodies[j].IsActive) continue;  

                    if (!CollisionMatrix.CanCollide(bodies[i].Layer, bodies[j].Layer))
                        continue;

                    if (Overlaps(bodies[i], bodies[j]))
                        _result.Add(new CollisionPair { A = bodies[i], B = bodies[j] });
                }
            }

            return _result;
        }

        public bool Overlaps(PhysicsBody a, PhysicsBody b)
        {
            var distance = Vector2.Distance(a.Position, b.Position);
            var combinedRadii = a.Radius + b.Radius;
            return distance < combinedRadii;
        }
    }
}
