using RunnerShooter.Core.Installers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RunnerShooter.Gameplay.Level
{
    [CreateAssetMenu(menuName = "RunnerShooter/Level Config", fileName = "LevelConfig")]
    public sealed class LevelConfig : FeatureConfig
    {
        [Header("Track")]
        [SerializeField]
        [Min(0f)]
        private float m_Length = 200f;

        [SerializeField]
        [Min(0f)]
        private float m_GroundMargin = 40f;

        [SerializeField]
        [Min(0f)]
        private float m_RoadWidth = 8f;

        [Header("Enemy layout")]
        [SerializeField]
        [Min(0)]
        private int m_EnemyCount = 15;

        [SerializeField]
        [Min(0f)]
        private float m_EnemyMinSpacing = 4f;

        [SerializeField]
        [Min(0f)]
        private float m_EnemySpawnStartDistance = 30f;

        [Header("Prefabs")]
        [SerializeField]
        private AssetReferenceGameObject m_GroundTile;

        [SerializeField]
        private AssetReferenceGameObject m_FinishLine;

        public float Length => m_Length;
        public float GroundMargin => m_GroundMargin;
        public float RoadWidth => m_RoadWidth;
        public int EnemyCount => m_EnemyCount;
        public float EnemyMinSpacing => m_EnemyMinSpacing;
        public float EnemySpawnStartDistance => m_EnemySpawnStartDistance;
        public AssetReferenceGameObject GroundTile => m_GroundTile;
        public AssetReferenceGameObject FinishLine => m_FinishLine;
    }
}
