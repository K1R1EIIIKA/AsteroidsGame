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
            var strategy = new RandomMovementStrategy();
            return _container.Instantiate<Asteroid>(
                new object[] { _config.Enemy.asteroid, strategy });
        }

        private Fragment CreateFragment()
        {
            var strategy = new RandomMovementStrategy();
            return _container.Instantiate<Fragment>(
                new object[] { _config.Enemy.fragment, strategy });
        }

        private SmallFragment CreateSmallFragment()
        {
            var strategy = new RandomMovementStrategy();
            return _container.Instantiate<SmallFragment>(
                new object[] { _config.Enemy.small_fragment, strategy });
        }

        private Saucer CreateSaucer()
        {
            var ship = _container.Resolve<IShip>();
            var strategy = new ChaseMovementStrategy(ship, _config.Enemy.saucer);
            return _container.Instantiate<Saucer>(
                new object[] { _config, strategy });
        }
    }
}
