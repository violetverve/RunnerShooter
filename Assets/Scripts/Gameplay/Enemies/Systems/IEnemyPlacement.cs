using System.Collections.Generic;

namespace RunnerShooter.Gameplay.Enemies
{
    public interface IEnemyPlacement
    {
        /// <summary>
        /// Picks spaced-out spawn points along the road, from the start zone up to <paramref name="finishZ"/>.
        /// </summary>
        List<EnemySpawn> Generate(float finishZ);
    }
}
