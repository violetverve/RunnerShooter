using System;

namespace RunnerShooter.Gameplay.Level
{
    public interface ILevelService
    {
        event Action<float> ProgressChanged;

        float Length { get; }

        float FinishZ { get; }

        /// <summary>
        /// Distance travelled from the start, divided by the level length, in the 0..1 range.
        /// </summary>
        float NormalizedProgress { get; }

        int Meters { get; }

        void SetVehicleZ(float z);
    }
}
