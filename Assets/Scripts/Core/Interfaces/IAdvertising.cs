using System;

namespace Core.Interfaces
{
    public interface IAdvertising
    {
        void ShowInterstitial();
        void ShowRewarded(Action onRewarded);
        void Initialize();
        void ShowBanner();  
        void HideBanner(); 
    }
}
