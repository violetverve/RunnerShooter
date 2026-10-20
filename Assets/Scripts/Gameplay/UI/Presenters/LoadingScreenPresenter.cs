using System;
using RunnerShooter.Core.Boot;
using RunnerShooter.Gameplay.GameFlow;
using Zenject;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class LoadingScreenPresenter : IInitializable, IDisposable
    {
        private readonly IGameFlowService m_GameFlowService;
        private readonly ILoadingScreen m_LoadingScreen;

        public LoadingScreenPresenter(IGameFlowService gameFlowService, ILoadingScreen loadingScreen)
        {
            m_GameFlowService = gameFlowService;
            m_LoadingScreen = loadingScreen;
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
                m_LoadingScreen.Hide();
            }
        }
    }
}
