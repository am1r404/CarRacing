using CodeBase.Infrastructure.States;
using Zenject;
using CodeBase.Services;
using UnityEngine;
using CodeBase.Infrastructure.Services;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("ProjectInstaller: Installing services.");
        InstallServices();

        Debug.Log("ProjectInstaller: Installing factories.");
        InstallFactories();

        Debug.Log("ProjectInstaller: Installing managers.");
        InstallManagers();

        Debug.Log("ProjectInstaller: Binding GameStateMachine.");
        BindStateMachine();

        Debug.Log("ProjectInstaller: Binding GameBootstrapper.");
        BindGameBootstrapper();
    }

    private void InstallServices()
    {
        Container.BindInterfacesAndSelfTo<GameModeService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle().NonLazy();
        // Container.BindInterfacesTo<UnityLobbyService>().AsSingle();
    }

    private void BindStateMachine()
    {   
        Container.Bind<IGameStateMachine>()
                 .To<GameStateMachine>()
                 .AsSingle()
                 .NonLazy();
    }

    private void InstallFactories()
    {
        Container.BindInterfacesAndSelfTo<AddressableVehicleFactory>().AsSingle().NonLazy();
    }

    private void InstallManagers()
    {
        Container.BindInterfacesAndSelfTo<CarManager>().AsSingle().NonLazy();
        Container.Bind<VehicleSpawner>().AsSingle().NonLazy();
    }

    private void BindGameBootstrapper()
    {
        Container.BindInterfacesAndSelfTo<GameBootstrapper>()
                 .AsSingle()
                 .NonLazy();
    }
}