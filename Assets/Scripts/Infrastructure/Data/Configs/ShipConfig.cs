using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class ShipConfig
    {
        public int maxHealth;
        public float invincibilityDuration;

        public float thrustForce;
        public float rotationSpeed;
        public float maxSpeed;
        public float drag;

        public BulletConfig bullet;
        public LaserConfig laser;
    }
}
