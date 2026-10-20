using UnityEngine;

namespace RunnerShooter.Gameplay.Combat
{
    public readonly struct DamageHit
    {
        public readonly float Amount;
        public readonly Vector3 Point;
        public readonly Vector3 Direction;

        public DamageHit(float amount, Vector3 point, Vector3 direction)
        {
            Amount = amount;
            Point = point;
            Direction = direction;
        }
    }
}
