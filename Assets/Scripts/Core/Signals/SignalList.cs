using Core.Enums;
using UnityEngine;

namespace Core.Signals
{
    public struct EnemyDestroyedSignal
    {
        public EnemyType EnemyType;
        public int Reward;
        public Vector2 Position;
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

    public struct PlayerRespawnedSignal
    {
        public Vector2 Position;
    }

    public struct EnemySpawnedSignal
    {
        public EnemyType EnemyType;
        public Vector2 Position;
    }
    
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
