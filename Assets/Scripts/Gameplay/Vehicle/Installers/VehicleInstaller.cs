using RunnerShooter.Core.Installers;
using Zenject;

namespace RunnerShooter.Gameplay.Vehicle
{
    public sealed class VehicleInstaller : PrefabFeatureInstaller<VehicleView>
    {
        private readonly VehicleConfig m_Config;

        public VehicleInstaller(VehicleConfig config) : base(config.Prefab)
        {
            m_Config = config;
        }

        public override void Install(DiContainer container)
        {
            base.Install(container);
            container.BindInstance(m_Config);
            container.BindInterfacesTo<VehicleService>().AsSingle();
            container.BindInterfacesTo<VehicleSystem>().AsSingle();
        }
    }
}
