using System;
using RunnerShooter.Gameplay.GameFlow;
using Zenject;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class SwipeToAimHintPresenter : IInitializable, IDisposable
    {
        private readonly IGameFlowService m_GameFlowService;
        private readonly SwipeToAimHintView m_View;

        public SwipeToAimHintPresenter(IGameFlowService gameFlowService, SwipeToAimHintView view)
        {
            m_GameFlowService = gameFlowService;
            m_View = view;
        }

        public void Initialize()
        {
            OnStateChanged(m_GameFlowService.State);
            m_GameFlowService.StateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            m_GameFlowService.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameFlowState state)
        {
            if (state == GameFlowState.WaitingForTap)
            {
                m_View.Show();
            }
            else
            {
                m_View.Hide();
            }
        }
    }
}
