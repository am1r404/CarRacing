   // Assets/Code/Installers/NetworkInstaller.cs
   using Zenject;
   using Unity.Netcode;

   public class NetworkInstaller : MonoInstaller
   {
       public override void InstallBindings()
       {
           Container.Bind<NetworkManager>().FromInstance(NetworkManager.Singleton).AsSingle().NonLazy();
       }
   }