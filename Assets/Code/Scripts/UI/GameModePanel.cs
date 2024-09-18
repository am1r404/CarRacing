// Assets/Code/Scripts/UI/GameModePanel.cs
using System;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI
{
    public class GameModePanel : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button closeButton;
        [SerializeField] private GameModeButton[] gameModeButtons;

        private GameModeService _gameModeService;
        public event Action<GameMode> OnGameModeSelected;

        [Inject]
        private void Construct(GameModeService gameModeService)
        {
            _gameModeService = gameModeService;
        }

        private void Start()
        {
            closeButton.onClick.AddListener(Hide);

            foreach (var button in gameModeButtons)
            {
                button.OnClicked += HandleGameModeSelection;
            }
        }

        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }

        private void HandleGameModeSelection(GameMode gameMode)
        {
            _gameModeService.SetGameMode(gameMode);
            OnGameModeSelected?.Invoke(gameMode);
            Hide();
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Hide);

            foreach (var button in gameModeButtons)
            {
                button.OnClicked -= HandleGameModeSelection;
            }
        }
    }
}