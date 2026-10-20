using NUnit.Framework;
using RunnerShooter.Gameplay.Enemies;
using UnityEngine;

namespace RunnerShooter.Tests.EditMode
{
    public sealed class EnemyMovementTests
    {
        private const float DeltaTime = 0.1f;

        private EnemyConfig m_Config;
        private EnemyMovement m_Movement;
        private Vector3 m_Position;

        [SetUp]
        public void SetUp()
        {
            m_Config = ScriptableObject.CreateInstance<EnemyConfig>();
            m_Movement = new EnemyMovement(m_Config, -1f, 1f, 1);
            m_Position = new Vector3(0f, 0f, 50f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_Config);
        }

        private EnemyPose Step(Vector3 vehiclePosition)
        {
            EnemyPose pose = m_Movement.Tick(m_Position, vehiclePosition, DeltaTime);

            m_Position = pose.Position;

            return pose;
        }

        private void WanderUntilPaused()
        {
            for (int i = 0; i < 200; i++)
            {
                if (Step(Vector3.zero).AnimationSpeed == 0f)
                {
                    return;
                }
            }

            Assert.Fail("The enemy never paused.");
        }

        [Test]
        public void Tick_WanderingToRightEnd_PausesThereThenWalksBack()
        {
            WanderUntilPaused();

            Assert.AreEqual(1f, m_Position.x, 0.0001f);

            float pausedX = m_Position.x;

            Step(Vector3.zero);
            Assert.AreEqual(pausedX, m_Position.x);

            for (int i = 0; i < 20; i++)
            {
                Step(Vector3.zero);
            }

            Assert.Less(m_Position.x, pausedX);
        }

        [Test]
        public void TryAggro_VehicleInsideAggroDistance_StartsChasing()
        {
            m_Movement.TryAggro(m_Position, m_Position + Vector3.forward * (m_Config.AggroDistance - 1f));

            Assert.IsTrue(m_Movement.IsChasing);
        }

        [Test]
        public void TryAggro_VehicleOutsideAggroDistance_KeepsWandering()
        {
            m_Movement.TryAggro(m_Position, m_Position + Vector3.forward * (m_Config.AggroDistance + 1f));

            Assert.IsFalse(m_Movement.IsChasing);
        }

        [Test]
        public void Tick_Chasing_MovesTowardVehicleAtRunSpeed()
        {
            Vector3 vehicle = m_Position + Vector3.back * 10f;

            m_Movement.TryAggro(m_Position, vehicle);

            EnemyPose pose = Step(vehicle);

            Assert.AreEqual(m_Config.RunSpeed, pose.AnimationSpeed);
            Assert.AreEqual(50f - m_Config.RunSpeed * DeltaTime, pose.Position.z, 0.0001f);
        }

        [Test]
        public void Tick_Frozen_DoesNotMove()
        {
            m_Movement.Freeze();

            EnemyPose pose = Step(Vector3.zero);

            Assert.AreEqual(new Vector3(0f, 0f, 50f), pose.Position);
            Assert.AreEqual(0f, pose.AnimationSpeed);
        }
    }
}
