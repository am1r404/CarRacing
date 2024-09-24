using System;

namespace CodeBase.Infrastructure.Services
{
    public interface ISceneLoader
    {
        void LoadSceneAsync(string sceneName, Action onLoaded = null);
        void UnloadSceneAsync(string sceneName, Action onUnloaded = null);
        string ActiveSceneName { get; }
    }
}