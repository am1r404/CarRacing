using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.Services
{
    public class SceneLoader : ISceneLoader
    {
        private string _activeSceneName;
        public string ActiveSceneName => _activeSceneName;

        public void LoadSceneAsync(string sceneName, Action onLoaded = null)
        {
            if (_activeSceneName == sceneName)
            {
                Debug.LogWarning($"Scene '{sceneName}' is already active.");
                onLoaded?.Invoke();
                return;
            }

            if (!string.IsNullOrEmpty(_activeSceneName))
            {
                UnloadSceneAsync(_activeSceneName, () => LoadNewScene(sceneName, onLoaded));
            }
            else
            {
                LoadNewScene(sceneName, onLoaded);
            }
        }

        private void LoadNewScene(string sceneName, Action onLoaded)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).completed += (asyncOperation) =>
            {
                Scene loadedScene = SceneManager.GetSceneByName(sceneName);
                if (loadedScene.IsValid())
                {
                    SceneManager.SetActiveScene(loadedScene);
                    _activeSceneName = sceneName;
                    Debug.Log($"Scene '{sceneName}' loaded and set as active.");
                    onLoaded?.Invoke();
                }
                else
                {
                    Debug.LogError($"Failed to load scene: {sceneName}");
                }
            };
        }

        public void UnloadSceneAsync(string sceneName, Action onUnloaded = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("No scene name provided for unloading.");
                onUnloaded?.Invoke();
                return;
            }

            Scene sceneToUnload = SceneManager.GetSceneByName(sceneName);
            if (!sceneToUnload.IsValid() || !sceneToUnload.isLoaded)
            {
                Debug.LogWarning($"Scene '{sceneName}' is not loaded.");
                onUnloaded?.Invoke();
                return;
            }

            SceneManager.UnloadSceneAsync(sceneToUnload).completed += (asyncOperation) =>
            {
                Debug.Log($"Scene '{sceneName}' unloaded.");
                onUnloaded?.Invoke();
            };
        }
    }
}