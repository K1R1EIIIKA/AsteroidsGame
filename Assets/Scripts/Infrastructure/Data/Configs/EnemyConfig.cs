using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class EnemyConfig
    {
        public AsteroidConfig Asteroid;
        public FragmentConfig Fragment;
        public SmallFragmentConfig SmallFragment;
        public SaucerConfig Saucer;
    }
}
