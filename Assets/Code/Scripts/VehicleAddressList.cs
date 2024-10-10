using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VehicleAddressList", menuName = "Config/Vehicle Address List")]
public class VehicleAddressList : ScriptableObject
{
    [System.Serializable]
    public struct VehicleInfo
    {
        public string vehicleName;
        public string addressableKey;
    }

    public List<VehicleInfo> vehicles = new List<VehicleInfo>();

    public string GetAddressableKey(string name)
    {
        var vehicle = vehicles.Find(v => v.vehicleName.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        if (vehicle.Equals(default(VehicleInfo)))
        {
            Debug.LogWarning($"Vehicle name '{name}' not found in VehicleAddressList.");
            return null;
        }
        return vehicle.addressableKey;
    }
}