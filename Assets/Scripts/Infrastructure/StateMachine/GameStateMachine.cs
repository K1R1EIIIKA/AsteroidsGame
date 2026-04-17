using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GameStateMachine : IStateSwitcher, IGameStateMachine 
    {
        private IGameState _current;
        private readonly Dictionary<Type, IGameState> _states;

        public GameStateMachine(List<IGameState> states)
        {
            _states = states.ToDictionary(s => s.GetType());
        }

        public void Enter<T>() where T : IGameState
        {
            _current?.Exit();
            _current = _states[typeof(T)];
            _current.Enter();
        }

        public void Tick() 
        {
            _current?.Tick();
        }

        public void Exit()
        {
            _current?.Exit();
            _current = null;
        }
    }
}
