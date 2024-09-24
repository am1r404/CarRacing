using UnityEngine;
using Zenject;

namespace CodeBase.Services
{
    public class PrefabFactory : IVehicleFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _carPrefab;

        public PrefabFactory(DiContainer container, GameObject carPrefab)
        {
            _container = container;
            _carPrefab = carPrefab;
        }

        public void CreateVehicle(string address, Vector3 position, Quaternion rotation, System.Action<GameObject> callback)
        {
            // Here, you can utilize the address to fetch the correct prefab if necessary.
            // For simplicity, we'll use the provided _carPrefab. Adjust as needed.
            //var vehicleInstance = _container.InstantiatePrefab(_carPrefab, position, rotation);
            //callback?.Invoke(vehicleInstance);
        }
    }
}