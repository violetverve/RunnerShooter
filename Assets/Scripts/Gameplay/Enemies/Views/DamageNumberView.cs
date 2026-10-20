using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class DamageNumberView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro m_Text;

        [SerializeField]
        private float m_Rise = 1f;

        [SerializeField]
        private float m_Duration = 0.6f;

        private Camera m_Camera;

        public event Action<DamageNumberView> Finished;

        [Inject]
        private void Construct(Camera camera)
        {
            m_Camera = camera;
        }

        public void Show(Vector3 position, float amount)
        {
            transform.position = position;
            m_Text.text = Mathf.RoundToInt(amount).ToString(CultureInfo.InvariantCulture);
            m_Text.alpha = 1f;
            PlayAsync(position.y + m_Rise).Forget();
        }

        private async UniTaskVoid PlayAsync(float targetY)
        {
            Sequence sequence = DOTween.Sequence()
                .Join(transform.DOMoveY(targetY, m_Duration).SetEase(Ease.OutQuad))
                .Join(DOTween.To(() => m_Text.alpha, alpha => m_Text.alpha = alpha, 0f, m_Duration).SetEase(Ease.InQuad))
                .SetLink(gameObject);

            await sequence.ToUniTask(cancellationToken: destroyCancellationToken);
            Finished?.Invoke(this);
        }

        private void LateUpdate()
        {
            transform.rotation = m_Camera.transform.rotation;
        }
    }
}
