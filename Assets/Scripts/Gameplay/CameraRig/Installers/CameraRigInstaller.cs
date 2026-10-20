using RunnerShooter.Core.Installers;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.CameraRig
{
    public sealed class CameraRigInstaller : PrefabFeatureInstaller<CameraRigView>
    {
        private readonly CameraRigConfig m_Config;

        public CameraRigInstaller(CameraRigConfig config) : base(config.Prefab)
        {
            m_Config = config;
        }

        public override void Install(DiContainer container)
        {
            base.Install(container);
            container.BindInstance(m_Config);
            container.Bind<Camera>().FromResolveGetter<CameraRigView>(view => view.MainCamera).AsSingle();
            container.BindInterfacesTo<CameraRigSystem>().AsSingle();
        }
    }
}
