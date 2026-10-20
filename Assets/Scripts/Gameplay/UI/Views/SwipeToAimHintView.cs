using DG.Tweening;
using UnityEngine;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class SwipeToAimHintView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_Crosshair;

        [SerializeField]
        private float m_SwipeDistance = 220f;

        [SerializeField]
        private float m_SwipeDuration = 0.8f;

        private Tween m_SwipeTween;

        public void Show()
        {
            gameObject.SetActive(true);
            m_SwipeTween?.Kill();
            m_Crosshair.anchoredPosition = new Vector2(-m_SwipeDistance, m_Crosshair.anchoredPosition.y);
            m_SwipeTween = m_Crosshair
                .DOAnchorPosX(m_SwipeDistance, m_SwipeDuration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        public void Hide()
        {
            m_SwipeTween?.Kill();
            gameObject.SetActive(false);
        }
    }
}
