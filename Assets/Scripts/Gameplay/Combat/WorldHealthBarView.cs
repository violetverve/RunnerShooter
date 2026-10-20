using DG.Tweening;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.Combat
{
    public sealed class WorldHealthBarView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_FillPivot;

        [SerializeField]
        private Transform m_TrailPivot;

        [SerializeField]
        private float m_TrailDelay = 0.35f;

        [SerializeField]
        private float m_TrailDuration = 0.4f;

        private Camera m_Camera;
        private Tween m_TrailTween;

        [Inject]
        private void Construct(Camera camera)
        {
            m_Camera = camera;
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// Sets the fill at once. When animated, the lost part stays visible as a trail that shrinks after a short delay.
        /// </summary>
        public void SetHealth(float normalized, bool animate)
        {
            float clamped = Mathf.Clamp01(normalized);

            m_FillPivot.localScale = new Vector3(clamped, 1f, 1f);
            m_TrailTween?.Kill();

            if (!animate)
            {
                m_TrailPivot.localScale = new Vector3(clamped, 1f, 1f);

                return;
            }

            m_TrailTween = m_TrailPivot
                .DOScaleX(clamped, m_TrailDuration)
                .SetDelay(m_TrailDelay)
                .SetEase(Ease.OutQuad)
                .SetLink(gameObject);
        }

        private void LateUpdate()
        {
            Transform cameraTransform = m_Camera.transform;
            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position, cameraTransform.up);
        }
    }
}
