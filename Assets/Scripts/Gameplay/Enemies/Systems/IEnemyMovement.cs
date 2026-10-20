using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    public interface IEnemyMovement
    {
        bool IsChasing { get; }

        /// <summary>
        /// Stops the enemy in place for the rest of the run.
        /// </summary>
        void Freeze();

        /// <summary>
        /// Switches to chasing once the vehicle is within aggro distance.
        /// </summary>
        void TryAggro(Vector3 position, Vector3 vehiclePosition);

        EnemyPose Tick(Vector3 position, Vector3 vehiclePosition, float deltaTime);
    }
}
