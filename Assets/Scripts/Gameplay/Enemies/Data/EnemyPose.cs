using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    public readonly struct EnemyPose
    {
        public EnemyPose(Vector3 position, Vector3 forward, float animationSpeed)
        {
            Position = position;
            Forward = forward;
            AnimationSpeed = animationSpeed;
        }

        public Vector3 Position { get; }

        /// <summary>
        /// Planar direction to face; zero when the enemy should keep its current facing.
        /// </summary>
        public Vector3 Forward { get; }

        public float AnimationSpeed { get; }
    }
}
