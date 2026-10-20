namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class EnemyMovementFactory : IEnemyMovementFactory
    {
        private readonly EnemyConfig m_Config;

        public EnemyMovementFactory(EnemyConfig config)
        {
            m_Config = config;
        }

        public IEnemyMovement Create(float leftX, float rightX, int direction)
        {
            return new EnemyMovement(m_Config, leftX, rightX, direction);
        }
    }
}
