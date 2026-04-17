using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class LaserConfig
    {
        public int maxCharges;
        public float rechargeTime;
        public float laserDuration;
        public float laserLength;
        public float laserRadius; }
}
