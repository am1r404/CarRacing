using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
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
            SceneManager.LoadScene("Lobby");
        }

        public void Exit()
        {
            Debug.Log("Exiting BootstrapState");
        }
    }
}