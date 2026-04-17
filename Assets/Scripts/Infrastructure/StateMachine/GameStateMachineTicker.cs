using Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure.StateMachine
{
    public class GameStateMachineTicker : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;

        [Inject]
        public void Construct(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        private void Update() => _stateMachine.Tick();
    }
}
