using UnityEngine;

namespace RunnerShooter.Gameplay.UI
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform m_RectTransform;
        private Rect m_AppliedSafeArea;
        private Vector2Int m_AppliedScreenSize;

        private void Awake()
        {
            m_RectTransform = (RectTransform)transform;
        }

        private void Update()
        {
            Rect safeArea = Screen.safeArea;
            Vector2Int screenSize = new(Screen.width, Screen.height);

            if (safeArea == m_AppliedSafeArea && screenSize == m_AppliedScreenSize)
            {
                return;
            }

            m_AppliedSafeArea = safeArea;
            m_AppliedScreenSize = screenSize;
            m_RectTransform.anchorMin = new Vector2(safeArea.xMin / screenSize.x, safeArea.yMin / screenSize.y);
            m_RectTransform.anchorMax = new Vector2(safeArea.xMax / screenSize.x, safeArea.yMax / screenSize.y);
        }
    }
}
