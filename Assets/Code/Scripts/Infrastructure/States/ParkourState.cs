// Assets/Code/Scripts/States/ParkourState.cs
using CodeBase.Infrastructure.Services;
using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class ParkourState : IState
    {
        private CarPositionManager _carPositionManager;
        private VehicleSpawner _vehicleSpawner;
        private CameraFollow _cameraFollow;

        public ParkourState(CarPositionManager carPositionManager, VehicleSpawner vehicleSpawner,CameraFollow cameraFollow)
        {
            _carPositionManager = carPositionManager;
            _vehicleSpawner = vehicleSpawner;
            _cameraFollow = cameraFollow;
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
                _vehicleSpawner.SpawnVehicle("SportsCar", playerCarPosition.position, playerCarPosition.rotation,OnPlayerCarSpawned);
                _cameraFollow.SetTarget(playerCarPosition);
            }
            else
            {
                Debug.LogError("No player car position found");
            }
        }
        
        private void OnPlayerCarSpawned(GameObject playerCar)
        {
            if (playerCar != null)
            {
                // Assign the spawned vehicle to the camera
                _cameraFollow.SetTarget(playerCar.transform);
                Debug.Log("Player car spawned in Garage and camera assigned");
            }
            else
            {
                Debug.LogError("Failed to spawn player car");
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