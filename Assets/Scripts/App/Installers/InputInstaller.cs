using Infrastructure.InputLogic;
using Infrastructure.InputLogic.Handlers;
using Core.Interfaces;
using Zenject;

namespace App.Installers
{
    public class InputInstaller : Installer<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<KeyboardMouseInput>().AsSingle();
            Container.Bind<GamepadInput>().AsSingle();
            Container.Bind<VirtualJoystickInput>().AsSingle();
            Container.Bind<InputSwitcher>().AsSingle();
            Container.Bind<IInputHandler>().To<InputHandlerFacade>().AsSingle();
        }
    }
}
