using System;

namespace RunnerShooter.Gameplay.Vehicle
{
    public interface IVehicleService
    {
        /// <summary>
        /// Raised with the new health after damage is taken.
        /// </summary>
        event Action<float> HealthChanged;

        event Action Destroyed;

        float MaxHealth { get; }

        float CurrentHealth { get; }

        /// <summary>
        /// Sideways steering input from -1 (left) to 1 (right).
        /// </summary>
        float Steer { get; }

        void TakeDamage(float amount);

        void SetSteer(float steer);
    }
}
