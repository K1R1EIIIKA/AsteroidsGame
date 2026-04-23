using Core.Configs;
using Gameplay.Configs;
using UnityEngine;
using Zenject;

namespace App.Installers.Mono
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private AsteroidSpritesConfig _asteroidSpritesConfig;

        public override void InstallBindings()
        {
            SignalsInstaller.Install(Container);
            ConfigsInstaller.Install(Container, _gameConfig, _asteroidSpritesConfig);
            PhysicsInstaller.Install(Container);
            ServicesInstaller.Install(Container);
            PoolsInstaller.Install(Container);
            ShipInstaller.Install(Container);
            EnemiesInstaller.Install(Container);
            WeaponsInstaller.Install(Container);
            AnalyticsInstaller.Install(Container);
        }
    }
}
