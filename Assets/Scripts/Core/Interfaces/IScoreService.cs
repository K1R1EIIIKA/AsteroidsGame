namespace Core.Interfaces
{
    public interface IScoreService
    {
        int Score { get; }
        void Add(int amount);
        void Reset();
    }
}
