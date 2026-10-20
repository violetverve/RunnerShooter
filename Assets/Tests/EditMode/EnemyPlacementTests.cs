using System.Collections.Generic;
using NUnit.Framework;
using RunnerShooter.Gameplay.Enemies;
using RunnerShooter.Gameplay.Level;
using UnityEngine;

namespace RunnerShooter.Tests.EditMode
{
    public sealed class EnemyPlacementTests
    {
        private const float FinishZ = 200f;

        private EnemyConfig m_EnemyConfig;
        private LevelConfig m_LevelConfig;
        private List<EnemySpawn> m_Spawns;

        [SetUp]
        public void SetUp()
        {
            m_EnemyConfig = ScriptableObject.CreateInstance<EnemyConfig>();
            m_LevelConfig = ScriptableObject.CreateInstance<LevelConfig>();
            m_Spawns = new EnemyPlacement(m_EnemyConfig, m_LevelConfig).Generate(FinishZ);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_EnemyConfig);
            Object.DestroyImmediate(m_LevelConfig);
        }

        [Test]
        public void Generate_Default_ProducesSpawns()
        {
            Assert.IsNotEmpty(m_Spawns);
        }

        [Test]
        public void Generate_Default_KeepsSpawnsInsideRoadAndBetweenStartAndFinish()
        {
            float roadHalf = m_LevelConfig.RoadWidth * 0.5f;
            float minZ = LevelConstants.StartZ + m_LevelConfig.EnemySpawnStartDistance;

            foreach (EnemySpawn spawn in m_Spawns)
            {
                Assert.LessOrEqual(Mathf.Abs(spawn.Position.x), roadHalf);
                Assert.GreaterOrEqual(spawn.Position.z, minZ);
                Assert.LessOrEqual(spawn.Position.z, FinishZ);
            }
        }

        [Test]
        public void Generate_Default_KeepsMinimumSpacingBetweenSpawns()
        {
            for (int i = 0; i < m_Spawns.Count; i++)
            {
                for (int j = i + 1; j < m_Spawns.Count; j++)
                {
                    Assert.GreaterOrEqual(Vector2.Distance(m_Spawns[i].Anchor, m_Spawns[j].Anchor), m_LevelConfig.EnemyMinSpacing);
                }
            }
        }

        [Test]
        public void Generate_Default_KeepsWanderRangeOnRoad()
        {
            float roadHalf = m_LevelConfig.RoadWidth * 0.5f;

            foreach (EnemySpawn spawn in m_Spawns)
            {
                Assert.GreaterOrEqual(spawn.LeftX, -roadHalf);
                Assert.LessOrEqual(spawn.RightX, roadHalf);
                Assert.LessOrEqual(spawn.LeftX, spawn.Position.x);
                Assert.GreaterOrEqual(spawn.RightX, spawn.Position.x);
            }
        }
    }
}
