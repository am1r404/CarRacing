using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class GameModeButton : MonoBehaviour
    {
        private Button button;
        [SerializeField] private GameMode gameMode;

        public event Action<GameMode> OnClicked;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            OnClicked?.Invoke(gameMode);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(HandleClick);
        }
    }
}