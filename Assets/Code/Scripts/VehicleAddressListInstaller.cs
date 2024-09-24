using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "VehicleAddressListInstaller", menuName = "Installers/Vehicle Address List Installer")]
public class VehicleAddressListInstaller : ScriptableObjectInstaller<VehicleAddressListInstaller>
{
    [SerializeField] private VehicleAddressList vehicleAddressList;

    public override void InstallBindings()
    {
        if (vehicleAddressList == null)
        {
            Debug.LogError("VehicleAddressList is not assigned in VehicleAddressListInstaller.");
            return;
        }

        Container.Bind<VehicleAddressList>().FromInstance(vehicleAddressList).AsSingle();
    }
}