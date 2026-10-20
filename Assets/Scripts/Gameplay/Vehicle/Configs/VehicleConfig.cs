using RunnerShooter.Core.Installers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RunnerShooter.Gameplay.Vehicle
{
    [CreateAssetMenu(menuName = "RunnerShooter/Vehicle Config", fileName = "VehicleConfig")]
    public sealed class VehicleConfig : FeatureConfig
    {
        [Header("Health")]
        [SerializeField]
        [Min(0f)]
        private float m_MaxHealth = 100f;

        [SerializeField]
        [Range(0f, 1f)]
        private float m_CriticalHealthShare = 0.3f;

        [Header("Movement")]
        [SerializeField]
        [Min(0f)]
        private float m_MoveSpeed = 8f;

        [SerializeField]
        [Min(0f)]
        private float m_SwayDistance = 1f;

        [SerializeField]
        [Min(0f)]
        private float m_SwaySmoothing = 2f;

        [SerializeField]
        [Min(0.1f)]
        private float m_FinishCoastDistance = 6f;

        [Header("Prefabs")]
        [SerializeField]
        private AssetReferenceGameObject m_Prefab;

        public float MaxHealth => m_MaxHealth;
        public float CriticalHealthShare => m_CriticalHealthShare;
        public float MoveSpeed => m_MoveSpeed;
        public float SwayDistance => m_SwayDistance;
        public float SwaySmoothing => m_SwaySmoothing;
        public float FinishCoastDistance => m_FinishCoastDistance;
        public AssetReferenceGameObject Prefab => m_Prefab;
    }
}
