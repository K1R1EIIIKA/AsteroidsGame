using Core.Interfaces;
using Gameplay.Collisions;
using Gameplay.Enemies.Systems;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.InputLogic;
using UnityEngine;
using Zenject;

namespace Gameplay.States
{
    public class GameplayUpdater
    {
        private readonly IInputHandler _input;
        private readonly IShip _ship;
        private readonly IEnemySpawner _spawner;
        private readonly EnemyPool _enemyPool;
        private readonly PhysicsWorld _physicsWorld;
        private readonly InputSwitcher _inputSwitcher;
        private readonly LazyInject<IStateSwitcher> _stateSwitcher;
        private readonly LazyInject<Laser> _laser;

        public GameplayUpdater(
            IInputHandler input,
            IShip ship,
            IEnemySpawner spawner,
            EnemyPool enemyPool,
            PhysicsWorld physicsWorld,
            InputSwitcher inputSwitcher,
            LazyInject<IStateSwitcher> stateSwitcher,
            LazyInject<Laser> laser)
        {
            _input = input;
            _ship = ship;
            _spawner = spawner;
            _enemyPool = enemyPool;
            _physicsWorld = physicsWorld;
            _inputSwitcher = inputSwitcher;
            _stateSwitcher = stateSwitcher;
            _laser = laser;
        }

        public void Tick()
        {
            _inputSwitcher.Tick();
            _input.UpdateInput();

            HandleMovementInput();
            HandleWeaponInput();
            HandlePauseInput();

            _ship.Tick(Time.deltaTime);
            _spawner.Tick(Time.deltaTime);
            _physicsWorld.CheckLaserCollisions(_laser.Value, _enemyPool);
            _physicsWorld.Tick(Time.deltaTime);
        }

        private void HandleMovementInput()
        {
            if (_input.IsThrusting)
            {
                var direction = _inputSwitcher.CurrentType is InputType.Joystick or InputType.Gamepad
                    ? _input.Direction
                    : default;

                _ship.Thrust(Time.deltaTime, direction);
            }

            if (_input.Direction != Vector2.zero &&
                (_inputSwitcher.CurrentType == InputType.Joystick ||
                 _inputSwitcher.CurrentType == InputType.Gamepad))
            {
                var targetAngle = Mathf.Atan2(_input.Direction.x, _input.Direction.y) * Mathf.Rad2Deg;
                _ship.RotateTowards(targetAngle, Time.deltaTime);
            }

            if (_inputSwitcher.CurrentType == InputType.Keyboard && _input.Rotation != 0f)
                _ship.Rotate(_input.Rotation * Time.deltaTime);
        }

        private void HandleWeaponInput()
        {
            if (_input.IsShootPressed) _ship.Shoot();
            if (_input.IsLaserPressed) _ship.ShootLaser();
        }

        private void HandlePauseInput()
        {
            if (_input.IsPausePressed)
                _stateSwitcher.Value.Enter<PauseState>();
        }
    }
}
