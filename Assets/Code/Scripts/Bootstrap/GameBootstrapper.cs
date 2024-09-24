using CodeBase.Infrastructure.States;
using Zenject;

public class GameBootstrapper : IInitializable
{
    private readonly IGameStateMachine _gameStateMachine;

    [Inject]
    public GameBootstrapper(IGameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public void Initialize()
    {
         _gameStateMachine.RegisterState<FootballState>();
        _gameStateMachine.RegisterState<BootstrapState>();
        _gameStateMachine.Enter<BootstrapState>();
    }
}