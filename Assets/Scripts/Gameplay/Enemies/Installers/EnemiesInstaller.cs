using RunnerShooter.Core.Installers;
using Zenject;

namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class EnemiesInstaller : PrefabFeatureInstaller<HitSplatterView>
    {
        private readonly EnemyConfig m_Config;

        public EnemiesInstaller(EnemyConfig config) : base(config.HitSplatterPrefab)
        {
            m_Config = config;
        }

        public override void Install(DiContainer container)
        {
            base.Install(container);
            container.BindInstance(m_Config);
            container.Bind<IEnemyPlacement>().To<EnemyPlacement>().AsSingle();
            container.Bind<IEnemyMovementFactory>().To<EnemyMovementFactory>().AsSingle();
            container.BindInterfacesTo<EnemySystem>().AsSingle();
        }
    }
}
