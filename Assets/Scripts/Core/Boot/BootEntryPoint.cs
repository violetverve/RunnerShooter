using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Rendering;
using Zenject;

namespace RunnerShooter.Core.Boot
{
    public sealed class BootEntryPoint : IInitializable, IDisposable
    {
        private readonly ILoadingScreen m_LoadingScreen;
        private readonly ISceneLoader m_SceneLoader;
        private readonly IShaderWarmup m_ShaderWarmup;
        private readonly CancellationTokenSource m_Cts = new();

        public BootEntryPoint(ILoadingScreen loadingScreen, ISceneLoader sceneLoader, IShaderWarmup shaderWarmup)
        {
            m_LoadingScreen = loadingScreen;
            m_SceneLoader = sceneLoader;
            m_ShaderWarmup = shaderWarmup;
        }

        public void Initialize()
        {
            StartAsync(m_Cts.Token).Forget();
        }

        public void Dispose()
        {
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        private async UniTaskVoid StartAsync(CancellationToken cancellationToken)
        {
            await m_LoadingScreen.LoadAsync(cancellationToken);
            await m_ShaderWarmup.WarmUpAsync(cancellationToken);
            await m_SceneLoader.LoadAsync(BootConstants.GameplaySceneName);
        }
    }
}
