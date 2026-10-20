using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Boot;
using RunnerShooter.Core.Assets;
using RunnerShooter.Core.Installers;
using RunnerShooter.Gameplay.CameraRig;
using RunnerShooter.Gameplay.Enemies;
using RunnerShooter.Gameplay.Input;
using RunnerShooter.Gameplay.Level;
using RunnerShooter.Gameplay.Turret;
using RunnerShooter.Gameplay.UI;
using RunnerShooter.Gameplay.Vehicle;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.GameFlow
{
    public sealed class GameplayInstaller : MonoInstaller, ISceneAssetPreloader
    {
        [SerializeField]
        private GameConfig m_Configs;

        private FeatureInstaller[] m_Features;

        public UniTask PreloadAsync(IAssetProvider assetProvider, CancellationToken cancellationToken)
        {
            m_Features = CreateFeatures();

            List<UniTask> preloads = new();

            foreach (FeatureInstaller feature in m_Features)
            {
                preloads.Add(feature.PreloadAsync(assetProvider, cancellationToken));
            }

            return UniTask.WhenAll(preloads);
        }

        public override void InstallBindings()
        {
            foreach (FeatureInstaller feature in m_Features)
            {
                feature.Install(Container);
            }
        }

        private FeatureInstaller[] CreateFeatures() => new FeatureInstaller[]
        {
            new GameFlowInstaller(),
            new InputInstaller(),
            new LevelInstaller(m_Configs.Get<LevelConfig>()),
            new VehicleInstaller(m_Configs.Get<VehicleConfig>()),
            new TurretInstaller(m_Configs.Get<TurretConfig>()),
            new EnemiesInstaller(m_Configs.Get<EnemyConfig>()),
            new CameraRigInstaller(m_Configs.Get<CameraRigConfig>()),
            new UiInstaller(m_Configs.Get<UiConfig>()),
        };
    }
}
