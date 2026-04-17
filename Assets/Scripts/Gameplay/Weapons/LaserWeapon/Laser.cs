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
        public float Length => _config.Ship.laser.laserLength;
        public float Radius => _config.Ship.laser.laserRadius;
        public float RechargeProgress => 1f - (_rechargeTimer / _config.Ship.laser.rechargeTime);
        public bool IsActive { get; private set; }
        public Vector2 Origin { get; private set; }
        public float Angle { get; private set; }

        public Laser(ConfigService config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            _charges = config.Ship.laser.maxCharges;
        }   

        public bool TryFire(Vector2 position, float angle)
        {
            if (_charges <= 0 || IsActive) return false;

            Origin = position;
            Angle = angle;
            IsActive = true;
            _duration = _config.Ship.laser.laserDuration;
            _charges--;
            
            if (_rechargeTimer <= 0f)
                _rechargeTimer = _config.Ship.laser.rechargeTime;
            
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

            if (_charges < _config.Ship.laser.maxCharges)
            {
                _rechargeTimer -= delta;
                if (_rechargeTimer <= 0f)
                {
                    _charges++;
                    _rechargeTimer = _charges < _config.Ship.laser.maxCharges
                        ? _config.Ship.laser.rechargeTime
                        : 0f;
                }
            }
        }

        public void Reset()
        {
            _charges = _config.Ship.laser.maxCharges;
            _rechargeTimer = 0f;
            IsActive = false;
        }
    }
}
