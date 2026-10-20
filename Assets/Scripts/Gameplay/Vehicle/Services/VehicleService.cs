using System;
using RunnerShooter.Gameplay.Combat;
using UnityEngine;

namespace RunnerShooter.Gameplay.Vehicle
{
    public sealed class VehicleService : IVehicleService
    {
        private readonly Health m_Health;

        public event Action<float> HealthChanged;
        public event Action Destroyed;

        public float MaxHealth => m_Health.Max;
        public float CurrentHealth => m_Health.Current;
        public float Steer { get; private set; }

        public VehicleService(VehicleConfig config)
        {
            m_Health = new Health(config.MaxHealth);
        }

        public void SetSteer(float steer)
        {
            Steer = Mathf.Clamp(steer, -1f, 1f);
        }

        public void TakeDamage(float amount)
        {
            if (m_Health.IsDead || !m_Health.TakeDamage(amount))
            {
                return;
            }

            HealthChanged?.Invoke(m_Health.Current);

            if (m_Health.IsDead)
            {
                Destroyed?.Invoke();
            }
        }
    }
}
