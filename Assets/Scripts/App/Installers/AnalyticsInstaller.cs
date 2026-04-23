using Analytics.Advertising;
using Analytics.Firebase;
using Core.Interfaces;
using Zenject;

namespace App.Installers
{
    public class AnalyticsInstaller : Installer<AnalyticsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAnalytics>().To<FirebaseAnalyticsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<YandexAdsAdapter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AnalyticsScoreObserver>().AsSingle();
            Container.BindInterfacesAndSelfTo<ObserverScoreTracker>().AsSingle();
        }
    }
}
