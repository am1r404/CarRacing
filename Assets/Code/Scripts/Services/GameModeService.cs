using System;
using Zenject;

namespace CodeBase.Services
{
    public class GameModeService
    {
        public event Action<GameMode> OnGameModeChanged;

        private GameMode _currentGameMode;

        public GameMode CurrentGameMode => _currentGameMode;

        public void SetGameMode(GameMode gameMode)
        {
            if (_currentGameMode != gameMode)
            {
                _currentGameMode = gameMode;
                OnGameModeChanged?.Invoke(gameMode);
            }
        }

        public GameMode GetCurrentGameMode()
        {
            return _currentGameMode;
        }
    }
}