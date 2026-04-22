using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class FragmentConfig
    {
        public float Speed;
        public float Radius;
        public int Reward;
        public int FragmentCount;
        public float FragmentSpeedMultiplier;
    }
}
