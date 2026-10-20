using System;
using NUnit.Framework;
using RunnerShooter.Core.Installers;
using RunnerShooter.Gameplay.GameFlow;
using RunnerShooter.Gameplay.Turret;
using RunnerShooter.Gameplay.Vehicle;
using UnityEditor;
using UnityEngine;

namespace RunnerShooter.Tests.EditMode
{
    public sealed class GameConfigTests
    {
        private static GameConfig CreateGameConfig(params FeatureConfig[] configs)
        {
            GameConfig gameConfig = ScriptableObject.CreateInstance<GameConfig>();
            SerializedObject serialized = new(gameConfig);
            SerializedProperty list = serialized.FindProperty("m_Configs");
            list.arraySize = configs.Length;

            for (int i = 0; i < configs.Length; i++)
            {
                list.GetArrayElementAtIndex(i).objectReferenceValue = configs[i];
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            return gameConfig;
        }

        [Test]
        public void Get_ConfigPresent_ReturnsIt()
        {
            TurretConfig turret = ScriptableObject.CreateInstance<TurretConfig>();
            GameConfig gameConfig = CreateGameConfig(ScriptableObject.CreateInstance<VehicleConfig>(), turret);

            Assert.AreSame(turret, gameConfig.Get<TurretConfig>());
        }

        [Test]
        public void Get_ConfigMissing_Throws()
        {
            GameConfig gameConfig = CreateGameConfig(ScriptableObject.CreateInstance<VehicleConfig>());

            Assert.Throws<InvalidOperationException>(() => gameConfig.Get<TurretConfig>());
        }

        [Test]
        public void Get_ConfigDuplicated_Throws()
        {
            GameConfig gameConfig = CreateGameConfig(
                ScriptableObject.CreateInstance<TurretConfig>(),
                ScriptableObject.CreateInstance<TurretConfig>());

            Assert.Throws<InvalidOperationException>(() => gameConfig.Get<TurretConfig>());
        }
    }
}
