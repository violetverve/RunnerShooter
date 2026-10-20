using NUnit.Framework;
using RunnerShooter.Gameplay.Vehicle;
using UnityEngine;

namespace RunnerShooter.Tests.EditMode
{
    public sealed class VehicleServiceTests
    {
        private VehicleConfig m_Config;
        private VehicleService m_Service;

        [SetUp]
        public void SetUp()
        {
            m_Config = ScriptableObject.CreateInstance<VehicleConfig>();
            m_Service = new VehicleService(m_Config);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(m_Config);
        }

        [Test]
        public void Constructor_Default_StartsAtMaxHealth()
        {
            Assert.AreEqual(m_Service.MaxHealth, m_Service.CurrentHealth);
        }

        [TestCase(5f, 1f)]
        [TestCase(-5f, -1f)]
        [TestCase(0.25f, 0.25f)]
        public void SetSteer_AnyValue_ClampsToUnitRange(float steer, float expected)
        {
            m_Service.SetSteer(steer);

            Assert.AreEqual(expected, m_Service.Steer);
        }

        [Test]
        public void TakeDamage_PositiveAmount_ReducesHealthAndRaisesHealthChanged()
        {
            float raised = -1f;

            m_Service.HealthChanged += health => raised = health;
            m_Service.TakeDamage(10f);

            Assert.AreEqual(m_Service.MaxHealth - 10f, m_Service.CurrentHealth);
            Assert.AreEqual(m_Service.CurrentHealth, raised);
        }

        [Test]
        public void TakeDamage_MoreThanHealth_ClampsToZeroAndRaisesDestroyedOnce()
        {
            int destroyedCount = 0;

            m_Service.Destroyed += () => destroyedCount++;
            m_Service.TakeDamage(m_Service.MaxHealth * 2f);
            m_Service.TakeDamage(1f);

            Assert.AreEqual(0f, m_Service.CurrentHealth);
            Assert.AreEqual(1, destroyedCount);
        }

        [Test]
        public void TakeDamage_NonPositiveAmount_IsIgnored()
        {
            bool raised = false;

            m_Service.HealthChanged += _ => raised = true;
            m_Service.TakeDamage(0f);
            m_Service.TakeDamage(-5f);

            Assert.AreEqual(m_Service.MaxHealth, m_Service.CurrentHealth);
            Assert.IsFalse(raised);
        }
    }
}
