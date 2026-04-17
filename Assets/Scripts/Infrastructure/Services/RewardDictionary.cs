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
                    [EnemyType.Asteroid] = _config.Enemy.asteroid.reward,
                    [EnemyType.Fragment] = _config.Enemy.fragment.reward,
                    [EnemyType.SmallFragment] = _config.Enemy.small_fragment.reward,
                    [EnemyType.Saucer] = _config.Enemy.saucer.reward,
                };
        }

        public int Get(EnemyType type)
        {
            return _rewards.TryGetValue(type, out var reward) ? reward : 0;
        }
    }
}
