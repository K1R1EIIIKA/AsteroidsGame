using Core.Interfaces;
using Infrastructure.Services;
using Zenject;

namespace App.Installers
{
    public class ServicesInstaller : Installer<ServicesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IScoreService>().To<ScoreService>().AsSingle();
            Container.Bind<IGameTimeService>().To<GameTimeService>().AsSingle();
            Container.Bind<RewardDictionary>().AsSingle();
        }
    }
}
