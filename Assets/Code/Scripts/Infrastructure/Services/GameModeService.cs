using System;
using UnityEngine;
using Zenject;

namespace CodeBase.Services
{
    public class GameModeService
    {
        public event Action<GameMode> OnGameModeChanged;

        private const string GameModeKey = "SelectedGameMode";

        public GameMode CurrentGameMode { get; private set; }

        public GameModeService()
        {
            LoadGameMode();
        }

        public void SetGameMode(GameMode gameMode)
        {
            if (CurrentGameMode != gameMode)
            {
                CurrentGameMode = gameMode;
                PlayerPrefs.SetString(GameModeKey, gameMode.ToString());
                PlayerPrefs.Save();
                OnGameModeChanged?.Invoke(gameMode);
                Debug.Log($"GameModeService: Game mode set to {gameMode}");
            }
        }

        private void LoadGameMode()
        {
            string gameModeString = PlayerPrefs.GetString(GameModeKey, GameMode.Football.ToString());
            if(Enum.TryParse(gameModeString, out GameMode gameMode))
            {
                CurrentGameMode = gameMode;
                Debug.Log($"GameModeService: Loaded game mode {gameMode}");
            }
            else
            {
                CurrentGameMode = GameMode.Football;
                Debug.LogWarning("GameModeService: Failed to parse saved game mode. Set to Default.");
            }
        }
    }
}