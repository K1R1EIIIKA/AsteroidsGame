using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class SaucerConfig
    {
        public float speed;
        public float radius;
        public int reward;
        public float chaseAcceleration;
        public float maxChaseSpeed;
    }
}
