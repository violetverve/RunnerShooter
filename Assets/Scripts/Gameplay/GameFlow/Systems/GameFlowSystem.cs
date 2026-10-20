using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Boot;
using UnityEngine;

namespace RunnerShooter.Gameplay.GameFlow
{
    public sealed class GameFlowSystem : IGameFlowSystem
    {
        private readonly IGameFlowService m_GameFlowService;
        private readonly ISceneLoader m_SceneLoader;
        private readonly List<ILoadable> m_Loadables;
        private readonly CancellationTokenSource m_Cts = new();

        public GameFlowSystem(IGameFlowService gameFlowService, ISceneLoader sceneLoader, List<ILoadable> loadables)
        {
            m_GameFlowService = gameFlowService;
            m_SceneLoader = sceneLoader;
            m_Loadables = loadables;
        }

        public void Initialize()
        {
            m_GameFlowService.RestartRequested += OnRestartRequested;
            LoadAsync(m_Cts.Token).Forget();
        }

        public void Dispose()
        {
            m_GameFlowService.RestartRequested -= OnRestartRequested;
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        private async UniTaskVoid LoadAsync(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.WhenAll(m_Loadables.Select(loadable => loadable.LoadAsync(cancellationToken)));
                m_GameFlowService.FinishLoading();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogError($"Gameplay failed to load: {exception}");
            }
        }

        private void OnRestartRequested()
        {
            m_GameFlowService.RestartRequested -= OnRestartRequested;
            m_SceneLoader.ReloadAsync().Forget();
        }
    }
}
