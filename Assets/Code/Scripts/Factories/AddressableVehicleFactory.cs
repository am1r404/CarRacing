using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableVehicleFactory : IVehicleFactory
{
    private readonly VehicleAddressList _vehicleAddressList;

    public AddressableVehicleFactory(VehicleAddressList vehicleAddressList)
    {
        _vehicleAddressList = vehicleAddressList;
    }

    public void CreateVehicle(string vehicleName, Vector3 position, Quaternion rotation, Action<GameObject> onVehicleCreated)
    {
        string addressableKey = _vehicleAddressList.GetAddressableKey(vehicleName);
        if (string.IsNullOrEmpty(addressableKey))
        {
            Debug.LogError($"No Addressable Key found for vehicle name: {vehicleName}");
            return;
        }

        Addressables.InstantiateAsync(addressableKey, position, rotation).Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                onVehicleCreated?.Invoke(op.Result);
            }
            else
            {
                Debug.LogError($"Failed to load vehicle from address: {addressableKey}");
            }
        };
    }
}