using RunnerShooter.Gameplay.Combat;
using RunnerShooter.Core.Installers;
using RunnerShooter.Gameplay.Vehicle;
using Zenject;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class UiInstaller : PrefabFeatureInstaller<UiCanvasView>
    {
        private readonly UiConfig m_Config;

        public UiInstaller(UiConfig config) : base(config.CanvasPrefab)
        {
            m_Config = config;
        }

        public override void Install(DiContainer container)
        {
            base.Install(container);
            container.BindInstance(m_Config);
            container.Bind<WorldHealthBarView>().FromResolveGetter<VehicleView>(view => view.HealthBar).AsSingle();
            container.Bind<LevelProgressBarView>().FromResolveGetter<UiCanvasView>(view => view.LevelProgressBar).AsSingle();
            container.Bind<SwipeToAimHintView>().FromResolveGetter<UiCanvasView>(view => view.SwipeToAimHint).AsSingle();
            container.Bind<OverlayView>().FromResolveGetter<UiCanvasView>(view => view.Overlay).AsSingle();
            container.BindInterfacesTo<VehicleHealthPresenter>().AsSingle();
            container.BindInterfacesTo<LevelProgressPresenter>().AsSingle();
            container.BindInterfacesTo<SwipeToAimHintPresenter>().AsSingle();
            container.BindInterfacesTo<OverlayPresenter>().AsSingle();
            container.BindInterfacesTo<LoadingScreenPresenter>().AsSingle();
        }
    }
}
