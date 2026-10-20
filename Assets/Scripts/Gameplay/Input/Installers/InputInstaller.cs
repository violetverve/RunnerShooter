using RunnerShooter.Core.Installers;
using Zenject;

namespace RunnerShooter.Gameplay.Input
{
    public sealed class InputInstaller : FeatureInstaller
    {
        public override void Install(DiContainer container)
        {
            container.BindInterfacesTo<PlayerInput>().AsSingle();
        }
    }
}
