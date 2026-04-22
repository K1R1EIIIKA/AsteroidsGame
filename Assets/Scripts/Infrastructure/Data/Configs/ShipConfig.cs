using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class ShipConfig
    {
        public int MaxHealth;
        public float InvincibilityDuration;

        public float ThrustForce;
        public float RotationSpeed;
        public float MaxSpeed;
        public float Drag;

        public BulletConfig Bullet;
        public LaserConfig Laser;
    }
}
