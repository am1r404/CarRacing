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
        [SerializeField] private Button footballButton;
        [SerializeField] private Button parkourButton;

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

            if (footballButton != null)
                footballButton.onClick.AddListener(() => SelectGameMode(GameMode.Football));

            if (parkourButton != null)
                parkourButton.onClick.AddListener(() => SelectGameMode(GameMode.Parkour));
        }

        public void Show()
        {
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }

        private void SelectGameMode(GameMode gameMode)
        {
            OnGameModeSelected?.Invoke(gameMode);
            Hide();
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Hide);

            if (footballButton != null)
                footballButton.onClick.RemoveAllListeners();

            if (parkourButton != null)
                parkourButton.onClick.RemoveAllListeners();
        }
    }
}