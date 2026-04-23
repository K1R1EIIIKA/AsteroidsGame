using Core.Enums;
using Infrastructure.Data;
using UnityEngine;

namespace Gameplay.Enemies.Systems
{
    public class EnemySpawnPointProvider
    {
        private readonly ConfigService _config;

        public EnemySpawnPointProvider(ConfigService config)
        {
            _config = config;
        }

        public Vector2 GetSpawnPosition()
        {
            var hw = _config.World.WorldWidth * 0.5f + _config.World.SpawnMargin;
            var hh = _config.World.WorldHeight * 0.5f + _config.World.SpawnMargin;

            return Random.Range(0, 4) switch
                {
                    0 => new Vector2(Random.Range(-hw, hw), hh),
                    1 => new Vector2(Random.Range(-hw, hw), -hh),
                    2 => new Vector2(hw, Random.Range(-hh, hh)),
                    _ => new Vector2(-hw, Random.Range(-hh, hh))
                };
        }

        public Vector2 GetInitialVelocity(EnemyType type)
        {
            var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            var speed = type switch
                {
                    EnemyType.Asteroid => _config.Enemy.Asteroid.Speed,
                    EnemyType.Saucer => _config.Enemy.Saucer.Speed,
                    _ => _config.Enemy.Asteroid.Speed
                };

            return direction * speed;
        }
    }
}
