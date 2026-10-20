using UnityEngine;

namespace RunnerShooter.Gameplay.Combat
{
    public sealed class Health
    {
        public float Max { get; }
        public float Current { get; private set; }
        public float Normalized => Current / Max;
        public bool IsDead => Current <= 0f;

        public Health(float max)
        {
            Max = max;
            Current = max;
        }

        /// <returns>True if the damage was applied; amounts of zero or less are ignored.</returns>
        public bool TakeDamage(float amount)
        {
            if (amount <= 0f)
            {
                return false;
            }

            Current = Mathf.Max(0f, Current - amount);

            return true;
        }
    }
}
