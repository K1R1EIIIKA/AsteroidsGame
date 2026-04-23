using System;
using Core.Interfaces;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;
using Zenject;

namespace Analytics.Advertising
{
    public class YandexAdsAdapter : IAdvertising, IInitializable
    {
#if YANDEX_ADS_ENABLED
        private const string InterstitialId = "R-M-19120235-2";
        private const string RewardedId = "R-M-19120235-1";
        private const string BannerId = "R-M-19120235-3";

        private Banner _banner;
        private Interstitial _interstitial;
        private RewardedAd _rewardedAd;

        private InterstitialAdLoader _interstitialLoader;
        private RewardedAdLoader _rewardedLoader;

        private Action _onRewardedCallback;
#endif
        private readonly IAnalytics _analytics;

        public YandexAdsAdapter(IAnalytics analytics)
        {
            _analytics = analytics;
        }

        public void Initialize()
        {
#if YANDEX_ADS_ENABLED
            Debug.Log("[YandexAds] Initialized");

            MobileAds.SetUserConsent(true);

            LoadInterstitial();
            LoadRewarded();
#else
            _isInitialized = true;
            Debug.Log("[YandexAds] Stub initialized");
#endif
        }

        public void ShowInterstitial()
        {
#if YANDEX_ADS_ENABLED
            if (_interstitial != null)
            {
                _analytics.LogAdShown("interstitial");
                _interstitial.Show();
            }
            else
            {
                Debug.LogWarning("[YandexAds] Interstitial not ready, loading...");
                LoadInterstitial();
            }
#else
            Debug.Log("[YandexAds] ShowInterstitial (stub)");
#endif
        }

        public void ShowRewarded(Action onRewarded)
        {
#if YANDEX_ADS_ENABLED
            if (_rewardedAd != null)
            {
                _onRewardedCallback = onRewarded;
                _analytics.LogAdShown("rewarded");
                _rewardedAd.Show();
            }
            else
            {
                Debug.LogWarning("[YandexAds] Rewarded not ready, loading...");
                LoadRewarded();
            }
#else
            Debug.Log("[YandexAds] ShowRewarded (stub)");
            onRewarded?.Invoke();
#endif
        }

#if YANDEX_ADS_ENABLED
        private void LoadInterstitial()
        {
            _interstitialLoader = new InterstitialAdLoader();

            _interstitialLoader.OnAdLoaded += (sender, args) =>
            {
                Debug.Log("[YandexAds] Interstitial loaded");
                _interstitial = args.Interstitial;

                _interstitial.OnAdDismissed += (s, e) =>
                {
                    Debug.Log("[YandexAds] Interstitial dismissed");
                    _interstitial.Destroy();
                    _interstitial = null;
                    LoadInterstitial();
                };

                _interstitial.OnAdFailedToShow += (s, e) =>
                {
                    Debug.LogError($"[YandexAds] Interstitial failed to show: {e.Message}");
                    _interstitial.Destroy();
                    _interstitial = null;
                    LoadInterstitial();
                };
            };

            _interstitialLoader.OnAdFailedToLoad += (sender, args) => { Debug.LogError($"[YandexAds] Interstitial failed to load: {args.Message}"); };

            var config = new AdRequestConfiguration.Builder(InterstitialId).Build();
            _interstitialLoader.LoadAd(config);
        }

        private void LoadRewarded()
        {
            _rewardedLoader = new RewardedAdLoader();

            _rewardedLoader.OnAdLoaded += (sender, args) =>
            {
                Debug.Log("[YandexAds] Rewarded loaded");
                _rewardedAd = args.RewardedAd;

                _rewardedAd.OnRewarded += (s, reward) =>
                {
                    Debug.Log($"[YandexAds] Rewarded earned: {reward.amount} {reward.type}");
                    _analytics.LogAdRewarded("rewarded");
                    _onRewardedCallback?.Invoke();
                    _onRewardedCallback = null;
                };

                _rewardedAd.OnAdDismissed += (s, e) =>
                {
                    Debug.Log("[YandexAds] Rewarded dismissed");
                    _analytics.LogAdSkipped("rewarded");
                    _rewardedAd.Destroy();
                    _rewardedAd = null;
                    LoadRewarded();
                };

                _rewardedAd.OnAdFailedToShow += (s, e) =>
                {
                    Debug.LogError($"[YandexAds] Rewarded failed to show: {e.Message}");
                    _rewardedAd.Destroy();
                    _rewardedAd = null;
                    LoadRewarded();
                };
            };

            _rewardedLoader.OnAdFailedToLoad += (sender, args) => { Debug.LogError($"[YandexAds] Rewarded failed to load: {args.Message}"); };

            var config = new AdRequestConfiguration.Builder(RewardedId).Build();
            _rewardedLoader.LoadAd(config);
        }
#endif


        public void ShowBanner()
        {
#if YANDEX_ADS_ENABLED
            if (_banner == null)
            {
                var adSize = BannerAdSize.StickySize(600);
                _banner = new Banner(BannerId, adSize, AdPosition.TopCenter);

                _banner.OnAdLoaded += (s, e) =>
                    Debug.Log("[YandexAds] Banner loaded");

                _banner.OnAdFailedToLoad += (s, e) =>
                    Debug.LogError($"[YandexAds] Banner failed: {e.Message}");
            }

            _banner.LoadAd(new AdRequest.Builder().Build());
            _banner.Show();
#else
    Debug.Log("[YandexAds] ShowBanner (stub)");
#endif
        }

        public void HideBanner()
        {
#if YANDEX_ADS_ENABLED
            _banner?.Hide();
#else
    Debug.Log("[YandexAds] HideBanner (stub)");
#endif
        }
    }
}
