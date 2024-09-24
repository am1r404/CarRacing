using UnityEngine;
using Zenject;
using CodeBase.Services;
namespace CodeBase.Infrastructure.States
{
    public class LobbyState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly CarManager _carManager;
        private readonly VehicleSpawner _vehicleSpawner;
        private readonly CarPositionManager _carPositionManager;

        [Inject]
        public LobbyState(IGameStateMachine gameStateMachine, CarManager carManager, VehicleSpawner vehicleSpawner, CarPositionManager carPositionManager)
        {
            _gameStateMachine = gameStateMachine;
            _carManager = carManager;
            _vehicleSpawner = vehicleSpawner;
            _carPositionManager = carPositionManager;
        }

        public void Enter()
        {
            Debug.Log("Entering Lobby State");
            InitializeLobby();
        }

        private void InitializeLobby()
        {
            SpawnPlayerCar();
            PrepareFriendCarPositions();
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

        private void PrepareFriendCarPositions()
        {
            Transform friendCarPosition;
            // while ((friendCarPosition = _carPositionManager.GetNextFriendCarPosition()) != null)
            // {
            //     // _carManager.AddFriendCarPosition(friendCarPosition.position, friendCarPosition.rotation);
            // }
        }

        public void Exit()
        {
            Debug.Log("Exiting Lobby State");
            _carPositionManager.ResetFriendCarIndex();
            // Other clean up logic if needed
        }
    }
}