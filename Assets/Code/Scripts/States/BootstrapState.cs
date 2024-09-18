using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;

        [Inject]
        public BootstrapState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            InitializeGame();
            LoadLobbyScene();
        }

        private void InitializeGame()
        {
            Debug.Log("Initializing game...");
            // Add any other initialization logic here
        }

           private void LoadLobbyScene()
        {
            SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive).completed += OnLobbySceneLoaded;
        }

        private void OnLobbySceneLoaded(AsyncOperation obj)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Lobby"));
            _gameStateMachine.Enter<LobbyState>();
        }

        public void Exit()
        {
            Debug.Log("Exiting BootstrapState");
        }
    }
}