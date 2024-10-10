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

        public void Initialize()
        {
        }

        public void RegisterState<TState>() where TState : IState
        {
            var state = _container.Instantiate<TState>();
            _states[typeof(TState)] = state;
        }

        public void RegisterStateInstance<TState>(TState state) where TState : IState
        {
            _states[typeof(TState)] = state;
        }

        public void Enter<TState>() where TState : class, IState
        {
            _activeState?.Exit();
            if (_states.TryGetValue(typeof(TState), out var state))
            {
                _activeState = state;
                _activeState.Enter();
            }
            else
            {
                throw new Exception($"State {typeof(TState)} not registered.");
            }
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            _activeState?.Exit();
            if (_states.TryGetValue(typeof(TState), out var state))
            {
                var payloadState = state as IPayloadState<TPayload>;
                if (payloadState == null)
                {
                    throw new Exception($"State {typeof(TState)} does not implement IPayloadState<{typeof(TPayload)}>");
                }
                _activeState = payloadState;
                payloadState.Enter(payload);
            }
            else
            {
                throw new Exception($"State {typeof(TState)} not registered.");
            }
        }
    }
}