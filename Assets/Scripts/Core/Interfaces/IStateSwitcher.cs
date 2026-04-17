using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IStateSwitcher
    {
        void Enter<T>() where T : IGameState;
        void Exit();
        void Tick();
    }
}
