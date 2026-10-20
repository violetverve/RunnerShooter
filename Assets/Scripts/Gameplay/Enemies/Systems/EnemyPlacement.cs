using System.Collections.Generic;
using RunnerShooter.Gameplay.Level;
using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class EnemyPlacement : IEnemyPlacement
    {
        private readonly EnemyConfig m_Config;
        private readonly LevelConfig m_LevelConfig;

        public EnemyPlacement(EnemyConfig config, LevelConfig levelConfig)
        {
            m_Config = config;
            m_LevelConfig = levelConfig;
        }

        public List<EnemySpawn> Generate(float finishZ)
        {
            float roadHalf = m_LevelConfig.RoadWidth * 0.5f - EnemyConstants.RoadEdgeMargin;
            float minZ = LevelConstants.StartZ + m_LevelConfig.EnemySpawnStartDistance;
            float maxZ = finishZ - m_LevelConfig.EnemyMinSpacing;
            List<EnemySpawn> spawns = new();

            for (int i = 0; i < m_LevelConfig.EnemyCount; i++)
            {
                if (TryFindSpot(minZ, maxZ, roadHalf, spawns, out Vector2 spot))
                {
                    spawns.Add(CreateSpawn(spot, roadHalf));
                }
            }

            return spawns;
        }

        private bool TryFindSpot(float minZ, float maxZ, float roadHalf, List<EnemySpawn> placed, out Vector2 spot)
        {
            for (int attempt = 0; attempt < EnemyConstants.PlacementAttemptsPerEnemy; attempt++)
            {
                spot = new Vector2(Random.Range(-roadHalf, roadHalf), Random.Range(minZ, maxZ));

                if (IsFarFromOthers(spot, placed))
                {
                    return true;
                }
            }

            spot = default;

            return false;
        }

        private bool IsFarFromOthers(Vector2 spot, List<EnemySpawn> placed)
        {
            foreach (EnemySpawn other in placed)
            {
                if (Vector2.Distance(spot, other.Anchor) < m_LevelConfig.EnemyMinSpacing)
                {
                    return false;
                }
            }

            return true;
        }

        private EnemySpawn CreateSpawn(Vector2 spot, float roadHalf)
        {
            float leftX = Mathf.Max(-roadHalf, spot.x - m_Config.WanderHalfWidth);
            float rightX = Mathf.Min(roadHalf, spot.x + m_Config.WanderHalfWidth);
            float startX = Random.Range(leftX, rightX);
            int direction = Random.value < 0.5f ? -1 : 1;

            return new EnemySpawn(new Vector3(startX, 0f, spot.y), spot, leftX, rightX, direction);
        }
    }
}
