using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class AsteroidConfig
    {
        public float speed;
        public float radius;
        public int reward;
        public int fragmentCount;
        public float fragmentSpeedMultiplier;
    }
}
