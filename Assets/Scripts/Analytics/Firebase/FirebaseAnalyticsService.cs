using Core.Interfaces;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

namespace Analytics.Firebase
{
    public class FirebaseAnalyticsService : IAnalytics
    {
        private bool _isInitialized;

        public FirebaseAnalyticsService()
        {
            Initialize();
        }

        private void Initialize()
        {
#if FIREBASE_ENABLED
            FirebaseApp.CheckAndFixDependenciesAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.Result == DependencyStatus.Available)
                    {
                        _isInitialized = true;

                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                        Debug.Log("Firebase DebugView enabled");
                #endif

                        Debug.Log("Firebase initialized");
                    }
                    else
                    {
                        Debug.LogError($"Firebase init failed: {task.Result}");
                    }
                });
#else
    _isInitialized = true;
    Debug.Log("[Analytics] Firebase stub initialized");
#endif
        }

        public void LogGameStart()
        {
            LogEvent("game_start");
        }

        public void LogGameOver(int score)
        {
            LogEvent("game_over", ("score", score));
        }

        public void LogEnemyDestroyed(string enemyType)
        {
            LogEvent("enemy_destroyed", ("enemy_type", enemyType));
        }

        public void LogLaserFired()
        {
            LogEvent("laser_fired");
        }

        public void LogEvent(string eventName, params (string key, object value)[] parameters)
        {
            if (!_isInitialized) return;

#if FIREBASE_ENABLED
            if (parameters == null || parameters.Length == 0)
            {
                FirebaseAnalytics.LogEvent(eventName);
                return;
            }

            var firebaseParams = new Parameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var (key, value) = parameters[i];
                firebaseParams[i] = value switch
                    {
                        int intVal => new Parameter(key, intVal),
                        long longVal => new Parameter(key, longVal),
                        float floatVal => new Parameter(key, floatVal),
                        double doubleVal => new Parameter(key, doubleVal),
                        string strVal => new Parameter(key, strVal),
                        _ => new Parameter(key, value.ToString())
                    };
            }

            FirebaseAnalytics.LogEvent(eventName, firebaseParams);
#else
            if (parameters == null || parameters.Length == 0)
            {
                Debug.Log($"[Analytics] {eventName}");
                return;
            }

            var paramsStr = string.Join(", ",
                System.Array.ConvertAll(parameters, p => $"{p.key}={p.value}"));
            Debug.Log($"[Analytics] {eventName} — {paramsStr}");
#endif
        }
        
        public void LogAdShown(string adType)
        {
            LogEvent("ad_shown", ("ad_type", adType));
        }

        public void LogAdRewarded(string adType)
        {
            LogEvent("ad_rewarded", ("ad_type", adType));
        }

        public void LogAdSkipped(string adType)
        {
            LogEvent("ad_skipped", ("ad_type", adType));
        }

        public void LogReviveWithAd()
        {
            LogEvent("revive_with_ad");
        }
    }
}
