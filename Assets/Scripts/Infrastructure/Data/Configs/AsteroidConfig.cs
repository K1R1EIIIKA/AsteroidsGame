using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class AsteroidConfig
    {
        public float Speed;
        public float Radius;
        public int Reward;
        public int FragmentCount;
        public float FragmentSpeedMultiplier;
    }
}
