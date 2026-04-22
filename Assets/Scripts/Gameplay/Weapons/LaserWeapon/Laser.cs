using Core.Signals;
using Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons.LaserWeapon
{
    public class Laser
    {
        private readonly ConfigService _config;
        private readonly SignalBus _signalBus;

        private float _duration;
        private float _rechargeTimer;
        private int _charges;

        public int Charges => _charges;
        public float Length => _config.Ship.Laser.LaserLength;
        public float Radius => _config.Ship.Laser.LaserRadius;
        public float RechargeProgress => 1f - (_rechargeTimer / _config.Ship.Laser.RechargeTime);
        public bool IsActive { get; private set; }
        public Vector2 Origin { get; private set; }
        public float Angle { get; private set; }

        public Laser(ConfigService config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            _charges = config.Ship.Laser.MaxCharges;
        }   

        public bool TryFire(Vector2 position, float angle)
        {
            if (_charges <= 0 || IsActive) return false;

            Origin = position;
            Angle = angle;
            IsActive = true;
            _duration = _config.Ship.Laser.LaserDuration;
            _charges--;
            
            if (_rechargeTimer <= 0f)
                _rechargeTimer = _config.Ship.Laser.RechargeTime;
            
            _signalBus.Fire(new LaserFiredSignal { ChargesRemaining = _charges });
            return true;
        }

        public void Tick(float delta)
        {
            if (IsActive)
            {
                _duration -= delta;
                if (_duration <= 0f)
                    IsActive = false;
            }

            if (_charges < _config.Ship.Laser.MaxCharges)
            {
                _rechargeTimer -= delta;
                if (_rechargeTimer <= 0f)
                {
                    _charges++;
                    _rechargeTimer = _charges < _config.Ship.Laser.MaxCharges
                        ? _config.Ship.Laser.RechargeTime
                        : 0f;
                }
            }
        }

        public void Reset()
        {
            _charges = _config.Ship.Laser.MaxCharges;
            _rechargeTimer = 0f;
            IsActive = false;
        }
    }
}
