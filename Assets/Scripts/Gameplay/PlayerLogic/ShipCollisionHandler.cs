using System;
using Core.Interfaces;
using Core.Signals;
using Gameplay.Enemies;
using Zenject;

namespace Gameplay.PlayerLogic
{
    public class ShipCollisionHandler : IInitializable, IDisposable
    {
        private readonly IShip _ship;
        private readonly SignalBus _signalBus;

        public ShipCollisionHandler(IShip ship, SignalBus signalBus)
        {
            _ship = ship;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ShipCollisionSignal>(OnShipCollision);
        }

        public void Dispose()   
        {
            _signalBus.Unsubscribe<ShipCollisionSignal>(OnShipCollision);
        }

        private void OnShipCollision(ShipCollisionSignal signal)
        {
            _ship.TakeDamage(signal.HitVelocity);

            if (signal.EnemyOwner is Saucer saucer)
            {
                var bounceDirection = (signal.EnemyVelocity).normalized * -1f;
                var bounceVelocity  = bounceDirection * saucer.Config.Enemy.Saucer.MaxChaseSpeed * 2f;
                saucer.ApplyBounce(bounceVelocity);
            }
        }
    }
}
