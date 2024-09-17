// Assets/Scripts/Factories/IVehicleFactory.cs
using System;
using UnityEngine;

public interface IVehicleFactory
{
    void CreateVehicle(string vehicleName, Vector3 position, Quaternion rotation, Action<GameObject> onVehicleCreated);
}