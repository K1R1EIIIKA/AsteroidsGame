using System.Collections.Generic;
using Core.Enums;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class RewardDictionary
    {
        private readonly ConfigService _config;
        private Dictionary<EnemyType, int> _rewards;

        public RewardDictionary(ConfigService config)
        {
            _config = config;
        }

        public void Initialize()
        {
            _rewards = new Dictionary<EnemyType, int>
                {
                    [EnemyType.Asteroid] = _config.Enemy.Asteroid.Reward,
                    [EnemyType.Fragment] = _config.Enemy.Fragment.Reward,
                    [EnemyType.SmallFragment] = _config.Enemy.SmallFragment.Reward,
                    [EnemyType.Saucer] = _config.Enemy.Saucer.Reward,
                };
        }

        public int Get(EnemyType type)
        {
            return _rewards.GetValueOrDefault(type, 0);
        }
    }
}
