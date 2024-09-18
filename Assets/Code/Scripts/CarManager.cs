using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CodeBase.Services
{
    public class CarManager : IInitializable, IDisposable
    {
        private readonly GameModeService _gameModeService;
        private readonly DiContainer _container;
        private readonly PrefabFactory _carPrefabFactory;

        private List<GameObject> _activeCars = new List<GameObject>();

        public CarManager(GameModeService gameModeService, DiContainer container, PrefabFactory carPrefabFactory)
        {
            _gameModeService = gameModeService;
            _container = container;
            _carPrefabFactory = carPrefabFactory;
        }

        public void Initialize()
        {
            _gameModeService.OnGameModeChanged += OnGameModeChanged;
            // Initialize cars based on the current game mode
            CreateCars(_gameModeService.CurrentGameMode);
        }

        public void Dispose()
        {
            _gameModeService.OnGameModeChanged -= OnGameModeChanged;
            DestroyAllCars();
        }

        private void OnGameModeChanged(GameMode gameMode)
        {
            DestroyAllCars();
            CreateCars(gameMode);
        }

        private void CreateCars(GameMode gameMode)
        {
            int carCount = gameMode switch
            {
                GameMode.Football => 6,
                GameMode.Parkour => 10,
                _ => 3
            };

            for (int i = 0; i < carCount; i++)
            {
                var car = _carPrefabFactory.CreateCar();
                // Configure car based on game mode if needed
                // For Football mode, assign teams
                if (gameMode == GameMode.Football)
                {
                    if (i < 3)
                        car.GetComponent<Car>().AssignTeam(Team.Player);
                    else
                        car.GetComponent<Car>().AssignTeam(Team.Opponent);
                }

                _activeCars.Add(car);
            }

            Debug.Log($"{carCount} cars created for {gameMode} mode.");
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

    // Example Team enumeration
    public enum Team
    {
        Player,
        Opponent
    }

    // Factory for creating car prefabs
    public class PrefabFactory
    {
        private readonly DiContainer _container;
        private readonly GameObject _carPrefab;

        public PrefabFactory(DiContainer container, GameObject carPrefab)
        {
            _container = container;
            _carPrefab = carPrefab;
        }

        public GameObject CreateCar()
        {
            var carInstance = Object.Instantiate(_carPrefab);
            _container.InjectGameObject(carInstance);
            return carInstance;
        }
    }

    // Example Car component
    public class Car : MonoBehaviour
    {
        public void AssignTeam(Team team)
        {
            // Assign team logic
            Debug.Log($"{gameObject.name} assigned to {team} team.");
        }
    }
}