// Assets/Code/Scripts/Installers/GameInstaller.cs
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.UI;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private VehicleAddressList vehicleAddressList;

    public override void InstallBindings()
    {
        InstallInfrastructure();
        InstallVehicleFactory();
        InstallStates();
        InstallGameBootstrapper();
    }

    private void InstallInfrastructure()
    {
        // Bind GameStateMachine and ensure Initialize is called
        Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
    }

    private void InstallVehicleFactory()
    {
        Container.Bind<VehicleAddressList>()
            .FromScriptableObject(vehicleAddressList)
            .AsSingle();

        Container.BindInterfacesAndSelfTo<AddressableVehicleFactory>()
            .AsSingle()
            .NonLazy();
    }

    private void InstallStates()
    {
        // Bind states
        Container.Bind<BootstrapState>().AsSingle();
        Container.Bind<LobbyState>().AsSingle();

        // Bind additional states here if necessary
        // Container.Bind<AnotherState>().AsSingle();
    }

    private void InstallGameBootstrapper()
    {
        Container.BindInterfacesAndSelfTo<GameBootstrapper>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
    }
}