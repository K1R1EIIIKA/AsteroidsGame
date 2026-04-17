namespace Core.Interfaces
{
    public interface IGameTimeService
    {
        float DeltaTime { get; }
        float Time { get; }
        bool IsPaused { get; }
        void Pause();
        void Resume();
    }
}
