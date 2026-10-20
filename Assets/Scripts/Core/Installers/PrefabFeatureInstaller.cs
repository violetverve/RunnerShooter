using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace RunnerShooter.Core.Installers
{
    public abstract class PrefabFeatureInstaller<TView> : FeatureInstaller where TView : Component
    {
        private readonly AssetReferenceGameObject m_Prefab;

        private GameObject m_LoadedPrefab;

        protected PrefabFeatureInstaller(AssetReferenceGameObject prefab)
        {
            m_Prefab = prefab;
        }

        public override async UniTask PreloadAsync(IAssetProvider assetProvider, CancellationToken cancellationToken)
        {
            m_LoadedPrefab = await assetProvider.LoadAsync<GameObject>(m_Prefab, cancellationToken);
        }

        public override void Install(DiContainer container)
        {
            container.Bind<TView>()
                .FromComponentInNewPrefab(m_LoadedPrefab)
                .WithGameObjectName(m_LoadedPrefab.name)
                .AsSingle();
        }
    }
}
