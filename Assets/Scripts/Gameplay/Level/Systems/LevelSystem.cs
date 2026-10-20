using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using RunnerShooter.Gameplay.GameFlow;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.Level
{
    public sealed class LevelSystem : ILevelSystem
    {
        private readonly LevelConfig m_Config;
        private readonly ILevelService m_LevelService;
        private readonly IGameFlowService m_GameFlowService;
        private readonly IAssetProvider m_AssetProvider;
        private readonly IInstantiator m_Instantiator;
        private Transform m_Root;
        private FinishLineView m_FinishLine;

        public LevelSystem(
            LevelConfig config,
            ILevelService levelService,
            IGameFlowService gameFlowService,
            IAssetProvider assetProvider,
            IInstantiator instantiator)
        {
            m_Config = config;
            m_LevelService = levelService;
            m_GameFlowService = gameFlowService;
            m_AssetProvider = assetProvider;
            m_Instantiator = instantiator;
        }

        public void Initialize()
        {
            m_GameFlowService.StateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            m_GameFlowService.StateChanged -= OnStateChanged;
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            GameObject tilePrefab = await m_AssetProvider.LoadAsync<GameObject>(m_Config.GroundTile, cancellationToken);
            GameObject finishPrefab = await m_AssetProvider.LoadAsync<GameObject>(m_Config.FinishLine, cancellationToken);

            m_Root = new GameObject(LevelConstants.RootName).transform;
            BuildGround(tilePrefab);
            m_FinishLine = m_Instantiator.InstantiatePrefabForComponent<FinishLineView>(
                finishPrefab,
                new Vector3(0f, 0f, m_LevelService.FinishZ),
                Quaternion.identity,
                m_Root);
        }

        private void OnStateChanged(GameFlowState state)
        {
            if (state == GameFlowState.Won)
            {
                m_FinishLine.PlayConfetti();
            }
        }

        private void BuildGround(GameObject tilePrefab)
        {
            GameObject first = m_Instantiator.InstantiatePrefab(tilePrefab, m_Root);
            Bounds bounds = CalculateBounds(first);
            float tileLength = bounds.size.z;

            if (tileLength <= 0f)
            {
                throw new InvalidOperationException($"Ground tile '{tilePrefab.name}' has no renderer bounds along Z.");
            }

            float pivotToMinZ = first.transform.position.z - bounds.min.z;
            float coveredLength = m_LevelService.Length + m_Config.GroundMargin;
            int tileCount = Mathf.CeilToInt(coveredLength / tileLength) + LevelConstants.TilesBehindStart;

            PlaceTile(first, 0, tileLength, pivotToMinZ);

            for (int i = 1; i < tileCount; i++)
            {
                GameObject tile = m_Instantiator.InstantiatePrefab(tilePrefab, m_Root);
                PlaceTile(tile, i, tileLength, pivotToMinZ);
            }
        }

        private static void PlaceTile(GameObject tile, int index, float tileLength, float pivotToMinZ)
        {
            float minZ = LevelConstants.StartZ + (index - LevelConstants.TilesBehindStart) * tileLength;

            tile.transform.position = new Vector3(0f, 0f, minZ + pivotToMinZ);
        }

        private static Bounds CalculateBounds(GameObject instance)
        {
            Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }
    }
}
