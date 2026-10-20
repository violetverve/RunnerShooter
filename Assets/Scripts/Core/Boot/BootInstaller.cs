using RunnerShooter.Core.Assets;
using RunnerShooter.Core.Rendering;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace RunnerShooter.Core.Boot
{
    public sealed class BootInstaller : MonoInstaller
    {
        [SerializeField]
        private AssetReferenceGameObject m_LoadingScreenPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AssetProvider>().AsSingle();
            Container.BindInterfacesTo<ShaderWarmup>().AsSingle();
            Container.BindInterfacesTo<LoadingScreen>().AsSingle().WithArguments(m_LoadingScreenPrefab);
            Container.BindInterfacesTo<SceneLoader>().AsSingle();
            Container.BindInterfacesTo<BootEntryPoint>().AsSingle();
        }
    }
}
