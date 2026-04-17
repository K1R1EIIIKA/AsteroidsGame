namespace Core.Interfaces
{
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Tick();
    }
}
