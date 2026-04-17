using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ScoreService : IScoreService
    {
        public int Score { get; private set; }

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Score += amount;
        }

        public void Reset()
        {
            Score = 0;
        }
    }
}
