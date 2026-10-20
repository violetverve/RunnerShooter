using System;
using RunnerShooter.Gameplay.GameFlow;
using Zenject;

namespace RunnerShooter.Gameplay.Enemies
{
    public interface IEnemySystem : IInitializable, ILoadable, ITickable, IDisposable
    {
    }
}
