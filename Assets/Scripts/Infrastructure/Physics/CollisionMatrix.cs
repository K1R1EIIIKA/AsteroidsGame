using Core.Enums;

namespace Infrastructure.Physics
{
 public static class CollisionMatrix
    {
        private readonly static bool[,] Matrix = new bool[4, 4]
        {
            //              Ship   Enemy  Bullet  Laser
            /* Ship   */  { false, true,  false,  false },
            /* Enemy  */  { true,  false, true,   true  },
            /* Bullet */  { false, true,  false,  false },
            /* Laser  */  { false, true,  false,  false },
        };

        public static bool CanCollide(PhysicsLayer a, PhysicsLayer b)
        {
            return Matrix != null && Matrix[(int)a, (int)b];
        }
    }
}
