using RunnerShooter.Core.Installers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RunnerShooter.Gameplay.CameraRig
{
    [CreateAssetMenu(menuName = "RunnerShooter/Camera Rig Config", fileName = "CameraRigConfig")]
    public sealed class CameraRigConfig : FeatureConfig
    {
        [Header("Prefabs")]
        [SerializeField]
        private AssetReferenceGameObject m_Prefab;

        public AssetReferenceGameObject Prefab => m_Prefab;
    }
}
