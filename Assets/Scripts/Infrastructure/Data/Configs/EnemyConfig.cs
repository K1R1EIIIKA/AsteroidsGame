using System;

namespace Infrastructure.Data.Configs
{
    [Serializable]
    public class EnemyConfig
    {
        public AsteroidConfig asteroid;
        public FragmentConfig fragment;
        public SmallFragmentConfig small_fragment;
        public SaucerConfig saucer;
    }
}
