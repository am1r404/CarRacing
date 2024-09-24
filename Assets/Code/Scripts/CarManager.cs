using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Services
{
    public class CarManager : IInitializable, IDisposable
    {
        private readonly GameModeService _gameModeService;
        private readonly DiContainer _container;
        private readonly IVehicleFactory _carPrefabFactory;

        private List<GameObject> _activeCars = new List<GameObject>();

        public CarManager(GameModeService gameModeService, DiContainer container)
        {
            _gameModeService = gameModeService;
            _container = container;
            //_carPrefabFactory = carPrefabFactory;
        }

        public void Initialize()
        {
            // Initialize cars based on the current game mode
            CreateCars(_gameModeService.CurrentGameMode);
        }

        public void Dispose()
        {
            DestroyAllCars();
        }

        private void CreateCars(GameMode gameMode)
        {
            Debug.Log("Creating cars");
        }

        private void DestroyAllCars()
        {
            foreach (var car in _activeCars)
            {
                _container.QueueForInject(car);
                Object.Destroy(car);
            }
            _activeCars.Clear();
            Debug.Log("All cars destroyed.");
        }
    }
}