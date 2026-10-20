using RunnerShooter.Core.Installers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RunnerShooter.Gameplay.UI
{
    [CreateAssetMenu(menuName = "RunnerShooter/UI Config", fileName = "UiConfig")]
    public sealed class UiConfig : FeatureConfig
    {
        [Header("Overlay")]
        [SerializeField]
        [Min(0f)]
        private float m_LoseOverlayDelay = 1f;

        [Header("Prefabs")]
        [SerializeField]
        private AssetReferenceGameObject m_CanvasPrefab;

        public float LoseOverlayDelay => m_LoseOverlayDelay;
        public AssetReferenceGameObject CanvasPrefab => m_CanvasPrefab;
    }
}
