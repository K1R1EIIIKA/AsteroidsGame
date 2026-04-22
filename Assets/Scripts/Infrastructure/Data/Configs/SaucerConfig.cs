using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class SaucerConfig
    {
        public float Speed;
        public float Radius;
        public int Reward;
        public float ChaseAcceleration;
        public float MaxChaseSpeed;
    }
}
