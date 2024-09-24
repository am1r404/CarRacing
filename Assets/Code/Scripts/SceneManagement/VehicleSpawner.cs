using NWH.Common.SceneManagement;
using NWH.Common.Vehicles;
using UnityEngine;
using Zenject;

public class VehicleSpawner
{
    private IVehicleFactory _vehicleFactory;
    private VehicleAddressList _vehicleAddressList;
    [Inject]
    private void Construct(IVehicleFactory vehicleFactory, VehicleAddressList vehicleAddressList)
    {
        _vehicleFactory = vehicleFactory;
        _vehicleAddressList = vehicleAddressList;
    }

    public void SpawnVehicle(string vehicleName, Vector3 position, Quaternion rotation)
    {
        string address = _vehicleAddressList.GetAddressableKey(vehicleName);
        if (string.IsNullOrEmpty(address))
        {
            Debug.LogError($"Vehicle name '{vehicleName}' not found in VehicleAddressList.");
            return;
        }

        _vehicleFactory.CreateVehicle(address, position, rotation, OnVehicleCreated);
    }

    private void OnVehicleCreated(GameObject vehicle)
    {
        Debug.Log("Vehicle spawned dynamically: " + vehicle.name);
        // Register with VehicleChanger
        //VehicleChanger.Instance.RegisterVehicle(vehicle.GetComponent<Vehicle>());

        // Optionally set it as the active vehicle
        //VehicleChanger.Instance.ChangeVehicle(VehicleChanger.Instance.vehicles.IndexOf(vehicle.GetComponent<Vehicle>()));
    }
}