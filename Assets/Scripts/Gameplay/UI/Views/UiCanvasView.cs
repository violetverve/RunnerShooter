using UnityEngine;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class UiCanvasView : MonoBehaviour
    {
        [SerializeField]
        private LevelProgressBarView m_LevelProgressBar;

        [SerializeField]
        private SwipeToAimHintView m_SwipeToAimHint;

        [SerializeField]
        private OverlayView m_Overlay;

        public LevelProgressBarView LevelProgressBar => m_LevelProgressBar;
        public SwipeToAimHintView SwipeToAimHint => m_SwipeToAimHint;
        public OverlayView Overlay => m_Overlay;
    }
}
