using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;
using Zenject;

public class GarageInstaller : MonoInstaller
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

        Container.Bind<GarageState>()
            .AsSingle()
            .NonLazy();

        RegisterGarageState();
    }

    private void RegisterGarageState()
    {
        var garageState = Container.Resolve<GarageState>();
        _gameStateMachine.RegisterStateInstance(garageState);
    }
}