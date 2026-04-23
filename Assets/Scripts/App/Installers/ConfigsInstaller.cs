using Core.Configs;
using Gameplay.Configs;
using Infrastructure.Data;
using Infrastructure.Data.Configs;
using Zenject;

namespace App.Installers
{
    public class ConfigsInstaller : Installer<GameConfig, AsteroidSpritesConfig, ConfigsInstaller>
    {
        private readonly GameConfig _gameConfig;
        private readonly AsteroidSpritesConfig _asteroidSpritesConfig;

        public ConfigsInstaller(GameConfig gameConfig, AsteroidSpritesConfig asteroidSpritesConfig)
        {
            _gameConfig = gameConfig;
            _asteroidSpritesConfig = asteroidSpritesConfig;
        }

        public override void InstallBindings()
        {
            var shipConfig = JsonConfigLoader.Load<ShipConfig>("ship_config");
            var enemyConfig = JsonConfigLoader.Load<EnemyConfig>("enemy_config");
            var worldConfig = JsonConfigLoader.Load<WorldConfig>("world_config");

            Container.Bind<ConfigService>()
                .AsSingle()
                .WithArguments(shipConfig, enemyConfig, worldConfig);

            Container.Bind<GameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();

            Container.Bind<AsteroidSpritesConfig>()
                .FromInstance(_asteroidSpritesConfig)
                .AsSingle();
        }
    }
}
