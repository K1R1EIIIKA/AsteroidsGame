namespace Core.Interfaces
{
    public interface IEnemySpawner
    {
        void Start();
        void Stop();
        void Tick(float delta);
    }
}
