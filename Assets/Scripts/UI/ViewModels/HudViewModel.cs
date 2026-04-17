using Core.Interfaces;
using UnityEngine;
using Zenject;
using System;
using Core.Enums;
using Core.Signals;

namespace UI.ViewModels
{
    public class HudViewModel : IInitializable, ITickable, IDisposable
    {
        private readonly LazyInject<IShip> _ship;
        private readonly SignalBus _signalBus;

        public Vector2 Position { get; private set; }
        public float Angle { get; private set; }
        public float Speed { get; private set; }
        public int Health { get; private set; }
        public int LaserCharges { get; private set; }
        public float LaserRechargeProgress { get; private set; }
        public int Score { get; private set; }

        public event Action OnChanged;

        public HudViewModel(LazyInject<IShip> ship, SignalBus signalBus)
        {
            _ship = ship;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
            _signalBus.Subscribe<PlayerDamagedSignal>(OnPlayerDamaged);
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
            Health = _ship.Value.Health;
            OnChanged?.Invoke();
        }

        public void Tick()
        {
            Position = _ship.Value.Position;
            Angle = _ship.Value.RotationAngle;
            Speed = _ship.Value.Speed;
            LaserCharges = _ship.Value.LaserCharges;
            LaserRechargeProgress = _ship.Value.LaserRechargeProgress;
            OnChanged?.Invoke();
        }

        private void OnScoreChanged(ScoreChangedSignal signal)
        {
            Score = signal.NewScore;
            OnChanged?.Invoke();
        }

        private void OnPlayerDamaged(PlayerDamagedSignal signal)
        {
            Health = signal.HealthRemaining;
            OnChanged?.Invoke();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ScoreChangedSignal>(OnScoreChanged);
            _signalBus.Unsubscribe<PlayerDamagedSignal>(OnPlayerDamaged);
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void OnGameStateChanged(GameStateChangedSignal signal)
        {
            if (signal.NewState == GameStateType.GameLoop)
            {
                Health = _ship.Value.Health;
                Score = 0;
                LaserCharges = _ship.Value.LaserCharges;
                LaserRechargeProgress = _ship.Value.LaserRechargeProgress;
                OnChanged?.Invoke();
            }
        }
    }
}
