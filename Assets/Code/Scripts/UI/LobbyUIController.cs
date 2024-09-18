// Assets/Code/Scripts/UI/LobbyUIController.cs
using System;
using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using CodeBase.Services;
using TMPro;

namespace CodeBase.UI
{
    public class LobbyUIController : MonoBehaviour
    {
        [SerializeField] private Button gameModeButton;
        [SerializeField] private TextMeshProUGUI gameModeButtonText;
        [SerializeField] private Button playButton;
        [SerializeField] public GameModePanel gameModePanel;

        private IGameStateMachine _gameStateMachine;
        private GameModeService _gameModeService;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, GameModeService gameModeService)
        {
            Debug.Log("LobbyUIController: Construct called");
            _gameStateMachine = gameStateMachine;
            _gameModeService = gameModeService;
        }

        private void Start()
        {
            Debug.Log("LobbyUIController: Start called");
            if (_gameModeService == null)
            {
                Debug.LogError("LobbyUIController: GameModeService is null in Start");
                return;
            }

            gameModeButton.onClick.AddListener(OpenGameModePanel);
            playButton.onClick.AddListener(StartGame);
            gameModePanel.OnGameModeSelected += HandleGameModeSelection;

            // Subscribe to GameModeService events
            _gameModeService.OnGameModeChanged += UpdateGameModeButtonText;

            // Set initial game mode text
            UpdateGameModeButtonText(_gameModeService.GetCurrentGameMode());
        }

        private void OnDestroy()
        {
            if (_gameModeService != null)
            {
                _gameModeService.OnGameModeChanged -= UpdateGameModeButtonText;
            }
            
            if (gameModePanel != null)
            {
                gameModePanel.OnGameModeSelected -= HandleGameModeSelection;
            }
            
            if (gameModeButton != null)
            {
                gameModeButton.onClick.RemoveListener(OpenGameModePanel);
            }
            
            if (playButton != null)
            {
                playButton.onClick.RemoveListener(StartGame);
            }
        }

        private void OpenGameModePanel()
        {
            gameModePanel.Show();
        }

        private void StartGame()
        {
            // _gameStateMachine.Enter<GameplayState>();
        }

        private void UpdateGameModeButtonText(GameMode gameMode)
        {
            gameModeButtonText.text = gameMode.ToString();
        }

        private void HandleGameModeSelection(GameMode gameMode)
        {
            _gameModeService.SetGameMode(gameMode);
            gameModePanel.Hide();
        }
    }
}