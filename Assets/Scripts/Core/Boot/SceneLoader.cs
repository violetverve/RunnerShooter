using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace RunnerShooter.Core.Boot
{
    public sealed class SceneLoader : ISceneLoader, IDisposable
    {
        private readonly ZenjectSceneLoader m_ZenjectSceneLoader;
        private readonly IAssetProvider m_AssetProvider;
        private readonly ILoadingScreen m_LoadingScreen;
        private readonly CancellationTokenSource m_Cts = new();

        private string m_SceneName;

        public SceneLoader(
            ZenjectSceneLoader zenjectSceneLoader,
            IAssetProvider assetProvider,
            ILoadingScreen loadingScreen)
        {
            m_ZenjectSceneLoader = zenjectSceneLoader;
            m_AssetProvider = assetProvider;
            m_LoadingScreen = loadingScreen;
        }

        public void Dispose()
        {
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public async UniTask LoadAsync(string sceneName)
        {
            m_SceneName = sceneName;

            await m_ZenjectSceneLoader.LoadSceneAsync(
                sceneName,
                LoadSceneMode.Additive,
                null,
                LoadSceneRelationship.Child).ToUniTask();

            Scene scene = SceneManager.GetSceneByName(sceneName);

            SceneManager.SetActiveScene(scene);

            SceneContext sceneContext = FindSceneContext(scene);

            await PreloadAsync(sceneContext, m_Cts.Token);

            // The parent container is passed through a static, so no other scene may load before Run.
            sceneContext.Run();
        }

        public async UniTask ReloadAsync()
        {
            await m_LoadingScreen.ShowAsync(m_Cts.Token);
            await SceneManager.UnloadSceneAsync(m_SceneName).ToUniTask();
            await LoadAsync(m_SceneName);
        }

        private async UniTask PreloadAsync(SceneContext sceneContext, CancellationToken cancellationToken)
        {
            List<UniTask> preloads = new();

            foreach (MonoInstaller installer in sceneContext.Installers)
            {
                if (installer is ISceneAssetPreloader preloader)
                {
                    preloads.Add(preloader.PreloadAsync(m_AssetProvider, cancellationToken));
                }
            }

            await UniTask.WhenAll(preloads);
        }

        private static SceneContext FindSceneContext(Scene scene)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                if (root.TryGetComponent(out SceneContext sceneContext))
                {
                    return sceneContext;
                }
            }

            throw new InvalidOperationException($"Scene '{scene.name}' has no root SceneContext.");
        }
    }
}
