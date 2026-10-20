using System;
using Zenject;

namespace RunnerShooter.Gameplay.Vehicle
{
    public interface IVehicleSystem : IInitializable, ITickable, IDisposable
    {
    }
}
