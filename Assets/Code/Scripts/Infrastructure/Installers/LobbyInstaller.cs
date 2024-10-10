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
        Container.Bind<CarPositionManager>()
                 .FromComponentInHierarchy()
                 .AsSingle()
                 .NonLazy();

        Container.Bind<LobbyState>()
                 .AsSingle()
                 .NonLazy();

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
        Container.BindInterfacesAndSelfTo<LobbyUIController>()
                 .FromComponentInHierarchy()
                 .AsSingle()
                 .NonLazy();
    }
}