using Core.Enums;
using Core.Interfaces;
using Core.Signals;
using Gameplay.Enemies.Systems;
using Gameplay.PlayerLogic;
using Gameplay.Weapons.BulletWeapon;
using Gameplay.Weapons.LaserWeapon;
using Infrastructure.InputLogic;
using UnityEngine;
using Zenject;

namespace Gameplay.States
{
    public class GameLoopState : IGameState
    {
        private readonly LazyInject<IStateSwitcher> _stateSwitcher;
        private readonly LazyInject<ShipView> _shipView;
        private readonly IInputHandler _input;
        private readonly IShip _ship;
        private readonly IEnemySpawner _spawner;
        private readonly EnemyPool _enemyPool;
        private readonly PhysicsWorld _physicsWorld;
        private readonly IScoreService _scoreService;
        private readonly SignalBus _signalBus;
        private readonly LazyInject<IUIManager> _uiManager;
        private readonly LazyInject<Laser> _laser;
        private readonly InputSwitcher _inputSwitcher;

        private bool _isFirstEnter = true;

        public GameLoopState(
            LazyInject<IStateSwitcher> stateSwitcher,
            LazyInject<ShipView> shipView,
            IInputHandler input,
            IShip ship,
            InputSwitcher inputSwitcher,
            IEnemySpawner spawner,
            EnemyPool enemyPool,
            PhysicsWorld physicsWorld,
            IScoreService scoreService,
            SignalBus signalBus,
            LazyInject<IUIManager> uiManager,
            LazyInject<Laser> laser)
        {
            _stateSwitcher = stateSwitcher;
            _shipView = shipView;
            _input = input;
            _ship = ship;
            _spawner = spawner;
            _inputSwitcher = inputSwitcher;
            _enemyPool = enemyPool;
            _physicsWorld = physicsWorld;
            _scoreService = scoreService;
            _signalBus = signalBus;
            _uiManager = uiManager;
            _laser = laser;
        }

        public void Enter()
        {
            if (_isFirstEnter)
            {
                _scoreService.Reset();
                _ship.Reset();
                _shipView.Value.gameObject.SetActive(true);
                _isFirstEnter = false;
            }

            _ship.ResetWeapons();
            _spawner.Start();
            _uiManager.Value.ShowHud();

            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Subscribe<GamePausedSignal>(OnPaused);
            _signalBus.Fire(new GameStateChangedSignal { NewState = GameStateType.GameLoop });
        }

        public void Tick()
        {
            _inputSwitcher.Tick();
            _input.UpdateInput();

            if (_input.IsThrusting)
            {
                var direction = _inputSwitcher.CurrentType is InputType.Joystick or InputType.Gamepad ? _input.Direction : default;

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
            {
                _ship.Rotate(_input.Rotation * Time.deltaTime);
            }

            if (_input.IsShootPressed)
                _ship.Shoot();
            if (_input.IsLaserPressed)
                _ship.ShootLaser();
            if (_input.IsPausePressed)
                _stateSwitcher.Value.Enter<PauseState>();

            _ship.Tick(Time.deltaTime);
            _spawner.Tick(Time.deltaTime);
            _physicsWorld.CheckLaserCollisions(_laser.Value, _enemyPool);
            _physicsWorld.Tick(Time.deltaTime);
        }

        public void Exit()
        {
            _spawner.Stop();
            _uiManager.Value.HideHud();

            _signalBus.Unsubscribe<PlayerDiedSignal>(OnPlayerDied);
            _signalBus.Unsubscribe<GamePausedSignal>(OnPaused);
        }

        public void ResetGame()
        {
            _isFirstEnter = true;
        }

        private void OnPlayerDied() =>
            _stateSwitcher.Value.Enter<GameOverState>();


        public void ContinueAfterAd()
        {
            _ship.SetHealth(1);
            _isFirstEnter = false;
        }
        
        private void OnPaused(GamePausedSignal signal) =>
            _stateSwitcher.Value.Enter<PauseState>();
    }
}
