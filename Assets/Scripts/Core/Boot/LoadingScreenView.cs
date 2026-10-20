using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RunnerShooter.Core.Boot
{
    public sealed class LoadingScreenView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_CanvasGroup;

        [SerializeField]
        private float m_FadeDuration = 0.3f;

        private Tween m_Tween;

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            m_Tween?.Kill();
            gameObject.SetActive(true);
            m_Tween = m_CanvasGroup.DOFade(1f, m_FadeDuration).SetLink(gameObject);
            await m_Tween.ToUniTask(cancellationToken: cancellationToken);
        }

        public void Hide()
        {
            m_Tween?.Kill();
            m_Tween = m_CanvasGroup.DOFade(0f, m_FadeDuration)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}
