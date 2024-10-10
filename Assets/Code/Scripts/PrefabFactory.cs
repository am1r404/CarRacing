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
        }
    }
}