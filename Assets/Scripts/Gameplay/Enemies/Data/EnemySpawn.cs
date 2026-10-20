using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    public readonly struct EnemySpawn
    {
        public EnemySpawn(Vector3 position, Vector2 anchor, float leftX, float rightX, int direction)
        {
            Position = position;
            Anchor = anchor;
            LeftX = leftX;
            RightX = rightX;
            Direction = direction;
        }

        public Vector3 Position { get; }

        /// <summary>
        /// The point (x, z) that minimum spacing is enforced on.
        /// </summary>
        public Vector2 Anchor { get; }

        public float LeftX { get; }
        public float RightX { get; }
        public int Direction { get; }
    }
}
