using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class LevelProgressBarView : MonoBehaviour
    {
        private const string MetersFormat = "{0}<size=70%>m</size>";

        [SerializeField]
        private Image m_Fill;

        [SerializeField]
        private RectTransform m_Marker;

        [SerializeField]
        private TMP_Text m_MetersLabel;

        public void SetProgress(float normalized, int meters)
        {
            float clamped = Mathf.Clamp01(normalized);

            m_Fill.fillAmount = clamped;
            m_Marker.anchorMin = new Vector2(m_Marker.anchorMin.x, clamped);
            m_Marker.anchorMax = new Vector2(m_Marker.anchorMax.x, clamped);
            m_MetersLabel.SetText(MetersFormat, meters);
        }
    }
}
