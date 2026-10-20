using RunnerShooter.Core.Installers;
using Zenject;

namespace RunnerShooter.Gameplay.Level
{
    public sealed class LevelInstaller : FeatureInstaller
    {
        private readonly LevelConfig m_Config;

        public LevelInstaller(LevelConfig config)
        {
            m_Config = config;
        }

        public override void Install(DiContainer container)
        {
            container.BindInstance(m_Config);
            container.BindInterfacesTo<LevelService>().AsSingle();
            container.BindInterfacesTo<LevelSystem>().AsSingle();
        }
    }
}
