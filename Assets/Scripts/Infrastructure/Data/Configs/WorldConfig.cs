using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class WorldConfig
    {
        public float WorldWidth;
        public float WorldHeight;
        public float SpawnMargin;
        public int MaxEnemiesOnMap;
        public float AsteroidSpawnInterval;
        public float SaucerSpawnInterval;
        public int MaxAsteroids;
        public int MaxSaucers;
    }
}
