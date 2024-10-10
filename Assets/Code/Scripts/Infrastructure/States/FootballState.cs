// Assets/Code/Scripts/States/FootballState.cs
using CodeBase.Infrastructure.Services;
using CodeBase.Services;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class FootballState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly GameModeService _gameModeService;

        public FootballState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, GameModeService gameModeService)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameModeService = gameModeService;
        }

        public void Enter()
        {
            Debug.Log("Entering Football State");
            LoadFootballScene();
        }

        private void LoadFootballScene()
        {
            _sceneLoader.LoadSceneAsync("Football", OnFootballSceneLoaded);
        }

        private void OnFootballSceneLoaded()
        {
            Debug.Log("Football Scene Loaded");
            InitializeFootballGame();
        }

        private void InitializeFootballGame()
        {
        }

        public void Exit()
        {
            Debug.Log("Exiting Football State");
        }
    }
}