using RunnerShooter.Core.Installers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RunnerShooter.Gameplay.Enemies
{
    [CreateAssetMenu(menuName = "RunnerShooter/Enemy Config", fileName = "EnemyConfig")]
    public sealed class EnemyConfig : FeatureConfig
    {
        [Header("Health")]
        [SerializeField]
        [Min(0f)]
        private float m_MaxHealth = 30f;

        [SerializeField]
        [Min(0f)]
        private float m_ContactDamage = 20f;

        [Header("Movement")]
        [SerializeField]
        [Min(0f)]
        private float m_AggroDistance = 18f;

        [SerializeField]
        [Min(0f)]
        private float m_WanderHalfWidth = 1.5f;

        [SerializeField]
        [Min(0f)]
        private float m_WanderSpeed = 1.2f;

        [SerializeField]
        [Min(0f)]
        private float m_WanderPause = 0.8f;

        [SerializeField]
        [Min(0f)]
        private float m_RunSpeed = 4f;

        [Header("Look")]
        [SerializeField]
        private Color m_Color = new(0.2f, 0.6f, 1f);

        [Header("Prefabs")]
        [SerializeField]
        private AssetReferenceGameObject m_StickmanPrefab;

        [SerializeField]
        private AssetReferenceGameObject m_ExplosionPrefab;

        [SerializeField]
        private AssetReferenceGameObject m_DamageNumberPrefab;

        [SerializeField]
        private AssetReferenceGameObject m_HitSplatterPrefab;

        public float MaxHealth => m_MaxHealth;
        public float ContactDamage => m_ContactDamage;
        public float AggroDistance => m_AggroDistance;
        public float WanderHalfWidth => m_WanderHalfWidth;
        public float WanderSpeed => m_WanderSpeed;
        public float WanderPause => m_WanderPause;
        public float RunSpeed => m_RunSpeed;
        public Color Color => m_Color;
        public AssetReferenceGameObject StickmanPrefab => m_StickmanPrefab;
        public AssetReferenceGameObject ExplosionPrefab => m_ExplosionPrefab;
        public AssetReferenceGameObject DamageNumberPrefab => m_DamageNumberPrefab;
        public AssetReferenceGameObject HitSplatterPrefab => m_HitSplatterPrefab;
    }
}
