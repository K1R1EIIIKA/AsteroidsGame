using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class WorldConfig
    {
        public float worldWidth;
        public float worldHeight;
        public float spawnMargin;
        public int maxEnemiesOnMap;
        public float asteroidSpawnInterval;
        public float saucerSpawnInterval;
        public int maxAsteroids;
        public int maxSaucers;
    }
}
