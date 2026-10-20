using System;
using UnityEngine;

namespace RunnerShooter.Gameplay.Level
{
    public sealed class LevelService : ILevelService
    {
        public event Action<float> ProgressChanged;

        public float Length { get; }
        public float FinishZ => LevelConstants.StartZ + Length;
        public float NormalizedProgress { get; private set; }
        public int Meters { get; private set; }

        public LevelService(LevelConfig config)
        {
            Length = config.Length;
        }

        public void SetVehicleZ(float z)
        {
            float travelled = Mathf.Clamp(z - LevelConstants.StartZ, 0f, Length);

            NormalizedProgress = travelled / Length;
            Meters = Mathf.FloorToInt(travelled);
            ProgressChanged?.Invoke(NormalizedProgress);
        }
    }
}
