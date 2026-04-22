using Core.Configs;
using Core.Enums;
using Core.Interfaces;
using Gameplay.Enemies.Strategies;
using Infrastructure.Data;
using Zenject;

namespace Gameplay.Enemies.Systems
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly DiContainer _container;
        private readonly ConfigService _config;

        public EnemyFactory(
            DiContainer container,
            ConfigService config)
        {
            _container = container;
            _config = config;
        }

        public IEnemy Create(EnemyType type) => type switch
            {
                EnemyType.Asteroid => CreateAsteroid(),
                EnemyType.Fragment => CreateFragment(),
                EnemyType.SmallFragment => CreateSmallFragment(),
                EnemyType.Saucer => CreateSaucer(),
                _ => CreateAsteroid()
            };

        private Asteroid CreateAsteroid()
        {
            var strategy = _container.Instantiate<RandomMovementStrategy>();
            return _container.Instantiate<Asteroid>(new object[] { _config.Enemy.Asteroid, strategy });
        }

        private Fragment CreateFragment()
        {
            var strategy = _container.Instantiate<RandomMovementStrategy>();
            return _container.Instantiate<Fragment>(new object[] { _config.Enemy.Fragment, strategy });
        }

        private SmallFragment CreateSmallFragment()
        {
            var strategy = _container.Instantiate<RandomMovementStrategy>();
            return _container.Instantiate<SmallFragment>(new object[] { _config.Enemy.SmallFragment, strategy });
        }

        private Saucer CreateSaucer()
        {
            var strategy = _container.Instantiate<ChaseMovementStrategy>(new object[] { _config.Enemy.Saucer });
            return _container.Instantiate<Saucer>(new object[] { _config, strategy });
        }
    }
}
