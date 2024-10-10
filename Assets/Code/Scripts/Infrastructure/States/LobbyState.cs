using System;
using UnityEngine;
using Zenject;
using CodeBase.Services;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity.Services.Matchmaker.Models;

namespace CodeBase.Infrastructure.States
{
    public class LobbyState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly CarManager _carManager;
        private readonly VehicleSpawner _vehicleSpawner;
        private readonly CarPositionManager _carPositionManager;
        
        private CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        public LobbyState(IGameStateMachine gameStateMachine, CarManager carManager, VehicleSpawner vehicleSpawner, CarPositionManager carPositionManager)
        {
            _gameStateMachine = gameStateMachine;
            _carManager = carManager;
            _vehicleSpawner = vehicleSpawner;
            _carPositionManager = carPositionManager;
        }

        public async  void Enter()
        {
            Debug.Log("Entering Lobby State");
            await InitializeLobbyAsync();   
            // SubscribeToLobbyEvents();
        }

        private async UniTask InitializeLobbyAsync()
        {
            try
            {

                SpawnPlayerCar();
                PrepareFriendCarPositions();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize lobby: {e.Message}");
            }
        }

        private void SpawnPlayerCar()
        {
            Transform playerCarPosition = _carPositionManager.GetPlayerCarPosition();
            if (playerCarPosition != null)
            {
                _vehicleSpawner.SpawnVehicle("SportsCar", playerCarPosition.position, playerCarPosition.rotation, OnPlayerCarSpawned);
            }
            else
            {
                Debug.LogError("No player car position found");
            }
        }
        
        private void OnPlayerCarSpawned(GameObject playerCar)
        {
            
        }

        private void PrepareFriendCarPositions()
        {
            Transform friendCarPosition;
            // while ((friendCarPosition = _carPositionManager.GetNextFriendCarPosition()) != null)
            // {
            //     // _carManager.AddFriendCarPosition(friendCarPosition.position, friendCarPosition.rotation);
            // }
        }
        
        // private void SubscribeToLobbyEvents()
        // {
        //     _unityLobbyService.OnPlayerJoined
        //         .Subscribe(OnPlayerJoined)
        //         .AddTo(_disposables);
        //
        //     _unityLobbyService.OnPlayerLeft
        //         .Subscribe(OnPlayerLeft)
        //         .AddTo(_disposables);
        //
        //     _unityLobbyService.OnLobbyUpdated
        //         .Subscribe(OnLobbyUpdated)
        //         .AddTo(_disposables);
        // }


        // private void OnPlayerJoined(Player player)
        // {
        //     Debug.Log($"Player joined: {player.Data["PlayerName"].Value}");
        // }
        //
        // private void OnPlayerLeft(Player player)
        // {
        //     Debug.Log($"Player left: {player.Data["PlayerName"].Value}");
        // }
        //
        // private void OnLobbyUpdated(Lobby lobby)
        // {
        //     Debug.Log($"Lobby updated: {lobby.Id}");
        // }

        public void Exit()
        {
            Debug.Log("Exiting Lobby State");
            _carPositionManager.ResetFriendCarIndex();
            // LeaveLobbyAsync().Forget();
            _disposables.Clear();
        }

        // private async UniTaskVoid LeaveLobbyAsync()
        // {
        //     try
        //     {
        //         await _unityLobbyService.LeaveLobbyAsync();
        //         Debug.Log("Left the lobby successfully.");
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogError($"Failed to leave lobby: {e.Message}");
        //     }
        // }
    }
}