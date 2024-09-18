// Assets/Code/Scripts/GameStateMachine.cs
using System;
using System.Collections.Generic;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine, IInitializable
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _activeState;
        private readonly DiContainer _container;

        public GameStateMachine(DiContainer container)
        {
            _container = container;
            _states = new Dictionary<Type, IState>();
        }

        // This method will be called automatically by Zenject
        public void Initialize()
        {
            RegisterState<BootstrapState>();
            RegisterState<LobbyState>();
            // Register additional states here if needed
        }

        public void RegisterState<TState>() where TState : IState
        {
            var state = _container.Instantiate<TState>();
            _states[typeof(TState)] = state;
        }

        public void Enter<TState>() where TState : class, IState
        {
            _activeState?.Exit();
            _activeState = _states[typeof(TState)];
            _activeState.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            _activeState?.Exit();
            var state = _states[typeof(TState)] as IPayloadState<TPayload>;
            _activeState = state;
            state.Enter(payload);
        }
    }
}