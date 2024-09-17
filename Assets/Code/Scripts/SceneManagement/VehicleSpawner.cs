// Assets/Scripts/SceneManagement/VehicleSpawner.cs

using NWH.Common.SceneManagement;
using NWH.Common.Vehicles;
using UnityEngine;
using Zenject;

public class VehicleSpawner : MonoBehaviour
{
    private IVehicleFactory _vehicleFactory;

    [Inject]
    private void Construct(IVehicleFactory vehicleFactory)
    {
        _vehicleFactory = vehicleFactory;
    }

    public void SpawnVehicle(string vehicleName, Vector3 position, Quaternion rotation)
    {
        _vehicleFactory.CreateVehicle(vehicleName, position, rotation, OnVehicleCreated);
    }

    private void OnVehicleCreated(GameObject vehicle)
    {
        Debug.Log("Vehicle spawned dynamically: " + vehicle.name);
        // Register with VehicleChanger
        VehicleChanger.Instance.RegisterVehicle(vehicle.GetComponent<Vehicle>());

        // Optionally set it as the active vehicle
        VehicleChanger.Instance.ChangeVehicle(VehicleChanger.Instance.vehicles.IndexOf(vehicle.GetComponent<Vehicle>()));
    }

    // Example usage: spawn a vehicle when pressing the 'V' key
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            string vehicleName = "SUV"; // Must match an entry in VehicleAddressList
            SpawnVehicle(vehicleName, spawnPosition, spawnRotation);
        }
    }
}