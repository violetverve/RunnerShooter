namespace RunnerShooter.Gameplay.Enemies
{
    public interface IEnemyMovementFactory
    {
        /// <summary>
        /// Creates movement that wanders between <paramref name="leftX"/> and <paramref name="rightX"/>, starting in <paramref name="direction"/> (-1 or 1).
        /// </summary>
        IEnemyMovement Create(float leftX, float rightX, int direction);
    }
}
