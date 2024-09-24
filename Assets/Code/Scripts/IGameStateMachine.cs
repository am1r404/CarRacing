namespace CodeBase.Infrastructure.States
{
    public interface IGameStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void RegisterState<TState>() where TState : IState;
        void RegisterStateInstance<TState>(TState state) where TState : IState;
    }
}