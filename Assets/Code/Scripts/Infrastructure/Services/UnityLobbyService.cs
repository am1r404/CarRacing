/*using System;
using Cysharp.Threading.Tasks;
using UniRx;
using Unity.Services.Core;
using UnityEngine;

namespace CodeBase.Services
{
    public class UnityLobbyService : IUnityLobbyService
    {
        private readonly Subject<Lobby> _onLobbyUpdated = new();
        private readonly Subject<Player> _onPlayerJoined = new();
        private readonly Subject<Player> _onPlayerLeft = new();

        private Lobby _currentLobby;

        public IObservable<Lobby> OnLobbyUpdated => _onLobbyUpdated;
        public IObservable<Player> OnPlayerJoined => _onPlayerJoined;
        public IObservable<Player> OnPlayerLeft => _onPlayerLeft;

        public UnityLobbyService()
        {
            InitializeLobbyService().Forget();
        }

        private async UniTaskVoid InitializeLobbyService()
        {
            try
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Services Initialized");
            }
            catch (Exception e)
            {
                Debug.LogError($"Unity Services Initialization Failed: {e.Message}");
            }
        }

        public async UniTask<Lobby> CreateLobbyAsync(string lobbyName, int maxPlayers)
        {
            try
            {
                var options = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = new Player { Data = new System.Collections.Generic.Dictionary<string, PlayerDataObject>() }
                };
                _currentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
                Debug.Log($"Lobby created with ID: {_currentLobby.Id}");

                SubscribeToLobbyEvents(_currentLobby.Id);
                return _currentLobby;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create lobby: {e.Message}");
                throw;
            }
        }

        public async UniTask<Lobby> JoinLobbyAsync(string lobbyId)
        {
            try
            {
                _currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
                Debug.Log($"Joined lobby with ID: {_currentLobby.Id}");

                SubscribeToLobbyEvents(_currentLobby.Id);
                return _currentLobby;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to join lobby: {e.Message}");
                throw;
            }
        }

        public async UniTask LeaveLobbyAsync()
        {
            if (_currentLobby == null)
            {
                Debug.LogWarning("No lobby to leave.");
                return;
            }

            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_currentLobby.Id, _currentLobby.CurrentPlayer.Id);
                Debug.Log("Left the lobby successfully.");
                _currentLobby = null;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to leave lobby: {e.Message}");
                throw;
            }
        }

        private void SubscribeToLobbyEvents(string lobbyId)
        {
            LobbyService.Instance.OnLobbyUpdate += HandleLobbyUpdate;
            LobbyService.Instance.OnPlayerJoined += HandlePlayerJoined;
            LobbyService.Instance.OnPlayerLeft += HandlePlayerLeft;
        }

        private void UnsubscribeFromLobbyEvents()
        {
            LobbyService.Instance.OnLobbyUpdate -= HandleLobbyUpdate;
            LobbyService.Instance.OnPlayerJoined -= HandlePlayerJoined;
            LobbyService.Instance.OnPlayerLeft -= HandlePlayerLeft;
        }

        private void HandleLobbyUpdate(Lobby lobby)
        {
            if (lobby.Id != _currentLobby.Id) return;
            _currentLobby = lobby;
            _onLobbyUpdated.OnNext(lobby);
        }

        private void HandlePlayerJoined(Player player)
        {
            if (_currentLobby == null || player.LobbyId != _currentLobby.Id) return;
            _onPlayerJoined.OnNext(player);
        }

        private void HandlePlayerLeft(Player player)
        {
            if (_currentLobby == null || player.LobbyId != _currentLobby.Id) return;
            _onPlayerLeft.OnNext(player);
        }

        public void Dispose()
        {
            UnsubscribeFromLobbyEvents();
            _onLobbyUpdated?.Dispose();
            _onPlayerJoined?.Dispose();
            _onPlayerLeft?.Dispose();
        }
    }

    public interface IUnityLobbyService : IDisposable
    {
        UniTask<Lobby> CreateLobbyAsync(string lobbyName, int maxPlayers);
        UniTask<Lobby> JoinLobbyAsync(string lobbyId);
        UniTask LeaveLobbyAsync();

        IObservable<Lobby> OnLobbyUpdated { get; }
        IObservable<Player> OnPlayerJoined { get; }
        IObservable<Player> OnPlayerLeft { get; }
    }
}*/