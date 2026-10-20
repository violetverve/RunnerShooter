using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Gameplay.GameFlow;
using Zenject;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class OverlayPresenter : IInitializable, IDisposable
    {
        private readonly IGameFlowService m_GameFlowService;
        private readonly UiConfig m_Config;
        private readonly OverlayView m_View;
        private readonly CancellationTokenSource m_Cts = new();

        public OverlayPresenter(IGameFlowService gameFlowService, UiConfig config, OverlayView view)
        {
            m_GameFlowService = gameFlowService;
            m_Config = config;
            m_View = view;
        }

        public void Initialize()
        {
            m_View.Hide();
            m_GameFlowService.StateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            m_GameFlowService.StateChanged -= OnStateChanged;
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        private void OnStateChanged(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.Won:
                    m_View.Show(OverlayType.Win);

                    break;
                case GameFlowState.Lost:
                    ShowLoseAsync(m_Cts.Token).Forget();

                    break;
            }
        }

        private async UniTaskVoid ShowLoseAsync(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(m_Config.LoseOverlayDelay), cancellationToken: cancellationToken);
            m_View.Show(OverlayType.Lose);
        }
    }
}
