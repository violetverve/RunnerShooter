using DG.Tweening;
using UnityEngine;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class OverlayView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_YouWin;

        [SerializeField]
        private CanvasGroup m_YouLose;

        [SerializeField]
        private float m_FadeDuration = 0.4f;

        private Tween m_Tween;

        public void Show(OverlayType type)
        {
            Hide();

            CanvasGroup panel = type == OverlayType.Win ? m_YouWin : m_YouLose;

            panel.gameObject.SetActive(true);
            panel.alpha = 0f;
            m_Tween = panel.DOFade(1f, m_FadeDuration).SetLink(gameObject);
        }

        public void Hide()
        {
            m_Tween?.Kill();
            m_YouWin.gameObject.SetActive(false);
            m_YouLose.gameObject.SetActive(false);
        }
    }
}
