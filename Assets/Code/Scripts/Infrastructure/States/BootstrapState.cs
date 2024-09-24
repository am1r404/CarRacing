using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;

        [Inject]
        public BootstrapState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            InitializeGame();
            LoadLobbyScene();
        }

        private void InitializeGame()
        {
            Debug.Log("Initializing game...");
        }

        private void LoadLobbyScene()
        {
            _sceneLoader.LoadSceneAsync("Lobby", OnLobbySceneLoaded);
        }

        private void OnLobbySceneLoaded()
        {
            _gameStateMachine.Enter<LobbyState>();
        }

        public void Exit()
        {
            Debug.Log("Exiting BootstrapState");
        }
    }
}