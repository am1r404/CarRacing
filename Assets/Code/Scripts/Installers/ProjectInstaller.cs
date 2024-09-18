using CodeBase.Infrastructure.States;
using Zenject;
using CodeBase.Services;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallServices();
    }

    private void InstallServices()
    {
        Container.BindInterfacesAndSelfTo<GameModeService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
    }
}