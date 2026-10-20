using RunnerShooter.Core.Installers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RunnerShooter.Gameplay.Turret
{
    [CreateAssetMenu(menuName = "RunnerShooter/Turret Config", fileName = "TurretConfig")]
    public sealed class TurretConfig : FeatureConfig
    {
        [Header("Aiming")]
        [SerializeField]
        [Min(0f)]
        private float m_MaxYawAngle = 35f;

        [SerializeField]
        [Min(0f)]
        private float m_AimSmoothing = 12f;

        [SerializeField]
        [Min(0f)]
        private float m_DragDegreesPerScreenWidth = 90f;

        [SerializeField]
        [Min(0f)]
        private float m_LaserLength = 30f;

        [Header("Firing")]
        [SerializeField]
        [Min(0f)]
        private float m_ShotsPerSecond = 8f;

        [SerializeField]
        [Min(0f)]
        private float m_BulletSpeed = 40f;

        [SerializeField]
        [Min(0f)]
        private float m_BulletDamage = 10f;

        [SerializeField]
        [Min(0f)]
        private float m_BulletLifetime = 2f;

        [Header("Prefabs")]
        [SerializeField]
        private AssetReferenceGameObject m_Prefab;

        [SerializeField]
        private AssetReferenceGameObject m_BulletPrefab;

        public float MaxYawAngle => m_MaxYawAngle;
        public float AimSmoothing => m_AimSmoothing;
        public float DragDegreesPerScreenWidth => m_DragDegreesPerScreenWidth;
        public float LaserLength => m_LaserLength;
        public float ShotsPerSecond => m_ShotsPerSecond;
        public float BulletSpeed => m_BulletSpeed;
        public float BulletDamage => m_BulletDamage;
        public float BulletLifetime => m_BulletLifetime;
        public AssetReferenceGameObject Prefab => m_Prefab;
        public AssetReferenceGameObject BulletPrefab => m_BulletPrefab;
    }
}
