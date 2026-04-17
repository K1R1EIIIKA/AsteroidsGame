using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class FragmentConfig
    {
        public float speed;
        public float radius;
        public int reward;
        public int fragmentCount;
        public float fragmentSpeedMultiplier;
    }
}
