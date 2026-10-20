using System;
using RunnerShooter.Gameplay.GameFlow;
using Zenject;

namespace RunnerShooter.Gameplay.Level
{
    public interface ILevelSystem : IInitializable, ILoadable, IDisposable
    {
    }
}
