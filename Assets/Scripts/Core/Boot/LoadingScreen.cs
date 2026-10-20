using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using RunnerShooter.Core.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace RunnerShooter.Core.Boot
{
    public sealed class LoadingScreen : ILoadingScreen, IDisposable
    {
        private readonly IAssetProvider m_AssetProvider;
        private readonly IInstantiator m_Instantiator;
        private readonly IShaderWarmup m_ShaderWarmup;
        private readonly AssetReferenceGameObject m_Prefab;
        private readonly CancellationTokenSource m_Cts = new();

        private LoadingScreenView m_View;

        public LoadingScreen(IAssetProvider assetProvider, IInstantiator instantiator, IShaderWarmup shaderWarmup,
            AssetReferenceGameObject prefab)
        {
            m_AssetProvider = assetProvider;
            m_Instantiator = instantiator;
            m_ShaderWarmup = shaderWarmup;
            m_Prefab = prefab;
        }

        public void Dispose()
        {
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            GameObject prefab = await m_AssetProvider.LoadAsync<GameObject>(m_Prefab, cancellationToken);

            m_View = m_Instantiator.InstantiatePrefabForComponent<LoadingScreenView>(prefab);
            m_View.name = prefab.name;
        }

        public UniTask ShowAsync(CancellationToken cancellationToken)
        {
            return m_View.ShowAsync(cancellationToken);
        }

        public void Hide()
        {
            HideAsync(m_Cts.Token).Forget();
        }

        private async UniTaskVoid HideAsync(CancellationToken cancellationToken)
        {
            await m_ShaderWarmup.WaitForCompilationAsync(cancellationToken);
            m_View.Hide();
        }
    }
}
