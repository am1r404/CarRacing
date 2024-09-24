// Assets/Code/Scripts/Installers/GameInstaller.cs
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using CodeBase.UI;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallVehicleFactory();
    }

    private void InstallVehicleFactory()
    {
        Container.BindInterfacesAndSelfTo<AddressableVehicleFactory>()
            .AsSingle()
            .NonLazy();
    }
}