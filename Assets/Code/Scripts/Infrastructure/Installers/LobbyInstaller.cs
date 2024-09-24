using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.UI;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    private IGameStateMachine _gameStateMachine;    

    [Inject]
    private void Construct(IGameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }

    public override void InstallBindings()
    {
        // Bind CarPositionManager first
        Container.Bind<CarPositionManager>()
                 .FromComponentInHierarchy()
                 .AsSingle()
                 .NonLazy();

        // Bind LobbyState
        Container.Bind<LobbyState>()
                 .AsSingle()
                 .NonLazy();

        // Install UI bindings
        InstallUI();

        RegisterLobbyState();
    }

    private void RegisterLobbyState()
    {
        var lobbyState = Container.Resolve<LobbyState>();
        _gameStateMachine.RegisterStateInstance(lobbyState);
    }

    private void InstallUI()
    {
        // Bind LobbyUIController with interfaces to ensure Initialize is called
        Container.BindInterfacesAndSelfTo<LobbyUIController>()
                 .FromComponentInHierarchy()
                 .AsSingle()
                 .NonLazy();
    }
}