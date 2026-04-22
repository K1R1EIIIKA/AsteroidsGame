using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Core.Signals
{
    public struct EnemyDestroyedSignal
    {
        public EnemyType EnemyType;
        public int Reward;
        public Vector2 Position;
        public IEnemy EnemyOwner;
        public Vector2 Velocity;
        public int FragmentCount;
        public float FragmentSpeedMultiplier;
    }

    public struct PlayerDamagedSignal
    {
        public int HealthRemaining;
    }

    public struct PlayerDiedSignal { }

    public struct LaserFiredSignal
    {
        public int ChargesRemaining;
    }

    public struct GameStateChangedSignal
    {
        public GameStateType NewState;
    }

    public struct ScoreChangedSignal
    {
        public int NewScore;
    }

    public struct GameStartedSignal { }

    public struct GamePausedSignal { }

    public struct GameResumedSignal { }
    
    public struct ShipCollisionSignal
    {
        public Vector2 HitVelocity;
        public object EnemyOwner;
        public Vector2 EnemyVelocity;
    }
    
    public struct PlayerInvincibilityEndedSignal { }
    
    public struct RewardedAdCompletedSignal { }
    
    public struct WatchAdRequestedSignal { }
}
