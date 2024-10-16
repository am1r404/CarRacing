using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GarageState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly CarPositionManager _carPositionManager;
        private readonly VehicleSpawner _vehicleSpawner;

        public GarageState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, CarPositionManager carPositionManager, VehicleSpawner vehicleSpawner)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _carPositionManager = carPositionManager;
            _vehicleSpawner = vehicleSpawner;
        }

        public void Enter()
        {
            Debug.Log("Entering Garage State");
            SpawnPlayerCar();
        }

        private void SpawnPlayerCar()
        {
            Transform playerCarPosition = _carPositionManager.GetPlayerCarPosition();
            if (playerCarPosition != null)
            {
                _vehicleSpawner.SpawnVehicle("SportsCar", playerCarPosition.position, playerCarPosition.rotation,OnPlayerCarSpawned);
                Debug.Log("Player car spawned in Garage");
            }
            else
            {
                Debug.LogError("No player car position found in Garage");
            }
        }
        
        private void OnPlayerCarSpawned(GameObject playerCar)
        {
            if (playerCar != null)
            {
                Debug.Log("Player car spawned in Garage and camera assigned");
            }
            else
            {
                Debug.LogError("Failed to spawn player car");
            }
        }
        
        public void Exit()
        {
            Debug.Log("Exiting Garage State");
        }
    }
}