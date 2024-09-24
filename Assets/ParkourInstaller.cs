using CodeBase.Infrastructure.States;
using CodeBase.Services;
using Zenject;

public class ParkourInstaller : MonoInstaller
{
    private IGameStateMachine _gameStateMachine;
    
    [Inject]
    private void Construct(IGameStateMachine gameStateMachine)
    {
        _gameStateMachine = gameStateMachine;
    }
    
    public override void InstallBindings()
    {   
        Container.Bind<CarPositionManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ParkourState>().AsSingle().NonLazy();
        RegisterParkourState();
    }
    
    private void RegisterParkourState()
    {
        var parkourState = Container.Resolve<ParkourState>();
        _gameStateMachine.RegisterStateInstance(parkourState);
        _gameStateMachine.Enter<ParkourState>();
    }
}
