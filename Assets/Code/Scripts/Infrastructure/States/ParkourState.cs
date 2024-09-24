// Assets/Code/Scripts/States/ParkourState.cs
using CodeBase.Infrastructure.Services;
using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class ParkourState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly GameModeService _gameModeService;
        private CarPositionManager _carPositionManager;
        private VehicleSpawner _vehicleSpawner;

        public ParkourState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, GameModeService gameModeService, CarPositionManager carPositionManager, VehicleSpawner vehicleSpawner)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameModeService = gameModeService;
            _carPositionManager = carPositionManager;
            _vehicleSpawner = vehicleSpawner;
        }

        public void Enter()
        {
            Debug.Log("Entering Parkour State");
            SpawnPlayerCar();
        }
        
        private void SpawnPlayerCar()
        {
            Transform playerCarPosition = _carPositionManager.GetPlayerCarPosition();
            if (playerCarPosition != null)
            {
                _vehicleSpawner.SpawnVehicle("SportsCar", playerCarPosition.position, playerCarPosition.rotation);
            }
            else
            {
                Debug.LogError("No player car position found");
            }
        }

        private void OnParkourSceneLoaded()
        {
            Debug.Log("Parkour Scene Loaded");
            
            InitializeParkourGame();
        }

        private void InitializeParkourGame()
        {
            // Initialize parkour-specific game elements here
            // e.g., spawn the player, set up obstacles, timers, etc.
        }

        public void Exit()
        {
            Debug.Log("Exiting Parkour State");
            // Cleanup parkour-specific game elements here
            // e.g., destroy obstacles, reset timers, etc.
        }
    }
}