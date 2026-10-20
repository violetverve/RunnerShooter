using NUnit.Framework;
using RunnerShooter.Gameplay.Combat;

namespace RunnerShooter.Tests.EditMode
{
    public sealed class HealthTests
    {
        [Test]
        public void Constructor_Default_StartsFull()
        {
            Health health = new(30f);

            Assert.AreEqual(30f, health.Current);
            Assert.AreEqual(1f, health.Normalized);
            Assert.IsFalse(health.IsDead);
        }

        [Test]
        public void TakeDamage_PositiveAmount_ReducesHealth()
        {
            Health health = new(30f);

            health.TakeDamage(10f);

            Assert.AreEqual(20f, health.Current);
            Assert.AreEqual(20f / 30f, health.Normalized, 0.0001f);
        }

        [Test]
        public void TakeDamage_MoreThanHealth_ClampsToZeroAndIsDead()
        {
            Health health = new(30f);

            health.TakeDamage(100f);

            Assert.AreEqual(0f, health.Current);
            Assert.IsTrue(health.IsDead);
        }

        [Test]
        public void TakeDamage_PositiveAmount_ReturnsTrue()
        {
            Health health = new(30f);

            Assert.IsTrue(health.TakeDamage(10f));
        }

        [Test]
        public void TakeDamage_NonPositiveAmount_ReturnsFalse()
        {
            Health health = new(30f);

            Assert.IsFalse(health.TakeDamage(0f));
            Assert.IsFalse(health.TakeDamage(-5f));
        }

        [Test]
        public void TakeDamage_NonPositiveAmount_IsIgnored()
        {
            Health health = new(30f);

            health.TakeDamage(0f);
            health.TakeDamage(-5f);

            Assert.AreEqual(30f, health.Current);
        }
    }
}
