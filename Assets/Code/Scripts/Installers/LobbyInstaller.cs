// Assets/Code/Scripts/Installers/LobbyInstaller.cs
using CodeBase.Services;
using CodeBase.UI;
using UnityEngine;
using Zenject;

public class LobbyInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallUI();
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