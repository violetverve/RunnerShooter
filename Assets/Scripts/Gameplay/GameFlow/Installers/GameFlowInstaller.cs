using RunnerShooter.Core.Installers;
using Zenject;

namespace RunnerShooter.Gameplay.GameFlow
{
    public sealed class GameFlowInstaller : FeatureInstaller
    {
        public override void Install(DiContainer container)
        {
            container.BindInterfacesTo<GameFlowService>().AsSingle();
            container.BindInterfacesTo<GameFlowSystem>().AsSingle();
            container.BindInterfacesTo<GameFlowTapInput>().AsSingle();
        }
    }
}
