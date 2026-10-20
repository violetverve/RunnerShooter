using System;
using RunnerShooter.Gameplay.GameFlow;
using Zenject;

namespace RunnerShooter.Gameplay.Turret
{
    public interface ITurretSystem : IInitializable, ILoadable, ITickable, IDisposable
    {
    }
}
