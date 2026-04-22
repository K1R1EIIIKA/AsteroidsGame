using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class LaserConfig
    {
        public int MaxCharges;
        public float RechargeTime;
        public float LaserDuration;
        public float LaserLength;
        public float LaserRadius; }
}
