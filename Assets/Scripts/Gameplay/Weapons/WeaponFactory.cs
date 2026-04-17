using System.Linq;
using Core.Interfaces;
using Gameplay.Configs;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly DiContainer _container;
        private readonly ConfigService _config;
        private readonly BulletPool _bulletPool;
        private readonly GameConfig _gameConfig;
        private readonly Laser _laser;

        public WeaponFactory(
            DiContainer container,
            ConfigService config,
            BulletPool bulletPool,
            GameConfig gameConfig,
            Laser laser)
        {
            _container = container;
            _config = config;
            _bulletPool = bulletPool;
            _gameConfig = gameConfig;
            _laser = laser;
        }

        public IWeaponController Create()
        {
            var laserGo = Object.Instantiate(_gameConfig.LaserPrefab);
            var laserView = laserGo.GetComponent<LaserView>();
            laserView.Initialize(_laser);

            return _container.Instantiate<WeaponController>(
                new object[] { _laser, _bulletPool, _config });
        }
    }
}
