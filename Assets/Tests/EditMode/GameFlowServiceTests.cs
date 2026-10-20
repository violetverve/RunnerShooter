using NUnit.Framework;
using RunnerShooter.Gameplay.GameFlow;

namespace RunnerShooter.Tests.EditMode
{
    public sealed class GameFlowServiceTests
    {
        private GameFlowService m_Service;

        [SetUp]
        public void SetUp()
        {
            m_Service = new GameFlowService();
        }

        [Test]
        public void State_OnCreate_IsLoading()
        {
            Assert.AreEqual(GameFlowState.Loading, m_Service.State);
        }

        [Test]
        public void Tap_WhileLoading_IsIgnored()
        {
            m_Service.Tap();

            Assert.AreEqual(GameFlowState.Loading, m_Service.State);
        }

        [Test]
        public void FinishLoading_WhileLoading_MovesToWaitingForTap()
        {
            m_Service.FinishLoading();

            Assert.AreEqual(GameFlowState.WaitingForTap, m_Service.State);
        }

        [Test]
        public void Tap_WhileWaitingForTap_MovesToPlaying()
        {
            m_Service.FinishLoading();

            m_Service.Tap();

            Assert.AreEqual(GameFlowState.Playing, m_Service.State);
        }

        [Test]
        public void Win_WhilePlaying_MovesToWon()
        {
            StartPlaying();

            m_Service.Win();

            Assert.AreEqual(GameFlowState.Won, m_Service.State);
        }

        [Test]
        public void Lose_WhilePlaying_MovesToLost()
        {
            StartPlaying();

            m_Service.Lose();

            Assert.AreEqual(GameFlowState.Lost, m_Service.State);
        }

        [Test]
        public void Lose_AfterWon_IsIgnored()
        {
            StartPlaying();
            m_Service.Win();

            m_Service.Lose();

            Assert.AreEqual(GameFlowState.Won, m_Service.State);
        }

        [Test]
        public void Win_WhileWaitingForTap_IsIgnored()
        {
            m_Service.FinishLoading();

            m_Service.Win();

            Assert.AreEqual(GameFlowState.WaitingForTap, m_Service.State);
        }

        [Test]
        public void Tap_AfterWon_RaisesRestartRequested()
        {
            int restartCount = 0;
            m_Service.RestartRequested += () => restartCount++;
            StartPlaying();
            m_Service.Win();

            m_Service.Tap();

            Assert.AreEqual(1, restartCount);
        }

        [Test]
        public void StateChanged_OnTransition_ReportsNewState()
        {
            GameFlowState reported = GameFlowState.Loading;
            m_Service.StateChanged += state => reported = state;

            m_Service.FinishLoading();

            Assert.AreEqual(GameFlowState.WaitingForTap, reported);
        }

        private void StartPlaying()
        {
            m_Service.FinishLoading();
            m_Service.Tap();
        }
    }
}
