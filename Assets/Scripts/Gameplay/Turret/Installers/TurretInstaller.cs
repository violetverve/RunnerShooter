using RunnerShooter.Core.Installers;
using Zenject;

namespace RunnerShooter.Gameplay.Turret
{
    public sealed class TurretInstaller : PrefabFeatureInstaller<TurretView>
    {
        private readonly TurretConfig m_Config;

        public TurretInstaller(TurretConfig config) : base(config.Prefab)
        {
            m_Config = config;
        }

        public override void Install(DiContainer container)
        {
            base.Install(container);
            container.BindInstance(m_Config);
            container.BindInterfacesTo<TurretSystem>().AsSingle();
        }
    }
}
