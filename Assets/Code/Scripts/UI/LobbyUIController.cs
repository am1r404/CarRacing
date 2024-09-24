// Assets/Code/Scripts/UI/LobbyUIController.cs
using System;
using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using CodeBase.Services;
using TMPro;
using UnityEngine.SceneManagement;
using CodeBase.Infrastructure.Services;

namespace CodeBase.UI
{
    public class LobbyUIController : MonoBehaviour
    {
        [SerializeField] private Button gameModeButton;
        [SerializeField] private TextMeshProUGUI gameModeButtonText;
        [SerializeField] private Button playButton;
        [SerializeField] public GameModePanel gameModePanel;
        [SerializeField] private Button garageButton;

        private IGameStateMachine _gameStateMachine;
        private GameModeService _gameModeService;
        private ISceneLoader _sceneLoader;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, GameModeService gameModeService, ISceneLoader sceneLoader)
        {
            Debug.Log("LobbyUIController: Construct called");
            _gameStateMachine = gameStateMachine;
            _gameModeService = gameModeService;
            _sceneLoader = sceneLoader;
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
            garageButton.onClick.AddListener(LoadGarageScene);
            gameModePanel.OnGameModeSelected += HandleGameModeSelection;

            _gameModeService.OnGameModeChanged += UpdateGameModeButtonText;

            UpdateGameModeButtonText(_gameModeService.CurrentGameMode);
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

            if (garageButton != null)
            {
                garageButton.onClick.RemoveListener(LoadGarageScene);
            }
        }

        private void OpenGameModePanel()
        {
            gameModePanel.Show();
        }

        private void StartGame()
        {
            Debug.Log("LobbyUIController: StartGame called");
            switch(_gameModeService.CurrentGameMode)
            {
                case GameMode.Football:
                    _gameStateMachine.Enter<FootballState>();
                    break;
                case GameMode.Parkour:
                    _sceneLoader.LoadSceneAsync("Parkour");
                    break;
                default:
                    Debug.LogError("LobbyUIController: Unsupported Game Mode");
                    break;
            }
        }

        private void LoadGarageScene()
        {
            _sceneLoader.LoadSceneAsync("Garage", OnGarageSceneLoaded);
        }    

        private void OnGarageSceneLoaded()
        {
            _gameStateMachine.Enter<GarageState>();
        }

        private void UpdateGameModeButtonText(GameMode gameMode)
        {
            gameModeButtonText.text = gameMode.ToString();
        }

        private void HandleGameModeSelection(GameMode gameMode)
        {
            _gameModeService.SetGameMode(gameMode);
            UpdateGameModeButtonText(gameMode);
            gameModePanel.Hide();
        }
    }
}