using System;
using RunnerShooter.Gameplay.Level;
using Zenject;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class LevelProgressPresenter : IInitializable, IDisposable
    {
        private readonly ILevelService m_LevelService;
        private readonly LevelProgressBarView m_View;

        public LevelProgressPresenter(ILevelService levelService, LevelProgressBarView view)
        {
            m_LevelService = levelService;
            m_View = view;
        }

        public void Initialize()
        {
            m_View.SetProgress(m_LevelService.NormalizedProgress, m_LevelService.Meters);
            m_LevelService.ProgressChanged += OnProgressChanged;
        }

        public void Dispose()
        {
            m_LevelService.ProgressChanged -= OnProgressChanged;
        }

        private void OnProgressChanged(float normalized)
        {
            m_View.SetProgress(normalized, m_LevelService.Meters);
        }
    }
}
