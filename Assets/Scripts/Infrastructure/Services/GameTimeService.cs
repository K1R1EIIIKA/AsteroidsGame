using Core.Interfaces;

namespace Infrastructure.Services
{
    public class GameTimeService : IGameTimeService
    {
        public float DeltaTime => IsPaused ? 0f : UnityEngine.Time.deltaTime;
        public float Time => UnityEngine.Time.time;
        public bool IsPaused { get; private set; }

        public void Pause() => IsPaused = true;
        public void Resume() => IsPaused = false;
    }
}
