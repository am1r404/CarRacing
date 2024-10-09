using NWH.Common.SceneManagement;
using NWH.Common.Vehicles;
using UnityEngine;
using Zenject;
using System;

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

    public void SpawnVehicle(string vehicleName, Vector3 position, Quaternion rotation, Action<GameObject> callback)
    {
        string address = _vehicleAddressList.GetAddressableKey(vehicleName);
        if (string.IsNullOrEmpty(address))
        {
            Debug.LogError($"Vehicle name '{vehicleName}' not found in VehicleAddressList.");
            callback?.Invoke(null);
            return;
        }

        _vehicleFactory.CreateVehicle(address, position, rotation, (vehicle) =>
        {
            OnVehicleCreated(vehicle, callback);
        });
    }

    private void OnVehicleCreated(GameObject vehicle, Action<GameObject> callback)
    {
        if (vehicle != null)
        {
            Debug.Log("Vehicle spawned dynamically: " + vehicle.name);
        }
        else
        {
            Debug.LogError("Failed to create vehicle.");
        }

        callback?.Invoke(vehicle);
    }
}
