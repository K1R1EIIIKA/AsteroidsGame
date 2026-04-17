namespace Core.Interfaces
{
    public interface IAnalytics
    {
        void LogGameStart();
        void LogGameOver(int score);
        void LogEnemyDestroyed(string enemyType);
        void LogLaserFired();
        void LogEvent(string eventName, params (string key, object value)[] parameters);

        void LogAdShown(string adType);
        void LogAdRewarded(string adType);
        void LogAdSkipped(string adType);
        void LogReviveWithAd();
    }
}
