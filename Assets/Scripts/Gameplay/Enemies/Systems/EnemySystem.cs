using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using RunnerShooter.Gameplay.Combat;
using RunnerShooter.Core.Pooling;
using RunnerShooter.Gameplay.GameFlow;
using RunnerShooter.Gameplay.Level;
using RunnerShooter.Gameplay.Vehicle;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class EnemySystem : IEnemySystem
    {
        private readonly EnemyConfig m_Config;
        private readonly LevelConfig m_LevelConfig;
        private readonly ILevelService m_LevelService;
        private readonly IVehicleService m_VehicleService;
        private readonly IGameFlowService m_GameFlowService;
        private readonly IAssetProvider m_AssetProvider;
        private readonly IInstantiator m_Instantiator;
        private readonly IEnemyPlacement m_Placement;
        private readonly IEnemyMovementFactory m_MovementFactory;
        private readonly HitSplatterView m_HitSplatter;
        private readonly VehicleView m_VehicleView;
        private readonly Dictionary<EnemyView, (Health Health, IEnemyMovement Movement)> m_Enemies = new();

        private Transform m_Root;
        private ViewPool<EnemyView> m_EnemyPool;
        private ViewPool<ExplosionView> m_ExplosionPool;
        private ViewPool<DamageNumberView> m_DamageNumberPool;
        private bool m_Ready;

        public EnemySystem(
            EnemyConfig config,
            LevelConfig levelConfig,
            ILevelService levelService,
            IVehicleService vehicleService,
            IGameFlowService gameFlowService,
            IAssetProvider assetProvider,
            IInstantiator instantiator,
            IEnemyPlacement placement,
            IEnemyMovementFactory movementFactory,
            HitSplatterView hitSplatter,
            VehicleView vehicleView)
        {
            m_Config = config;
            m_LevelConfig = levelConfig;
            m_LevelService = levelService;
            m_VehicleService = vehicleService;
            m_GameFlowService = gameFlowService;
            m_AssetProvider = assetProvider;
            m_Instantiator = instantiator;
            m_Placement = placement;
            m_MovementFactory = movementFactory;
            m_HitSplatter = hitSplatter;
            m_VehicleView = vehicleView;
        }

        public void Initialize()
        {
            m_GameFlowService.StateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            m_GameFlowService.StateChanged -= OnStateChanged;

            foreach (EnemyView enemy in m_Enemies.Keys)
            {
                Unsubscribe(enemy);
            }

            m_EnemyPool?.Dispose();
            m_ExplosionPool?.Dispose();
            m_DamageNumberPool?.Dispose();
        }

        public void Tick()
        {
            if (!m_Ready)
            {
                return;
            }

            Vector3 vehiclePosition = m_VehicleView.Position;
            bool playing = m_GameFlowService.State == GameFlowState.Playing;

            foreach (KeyValuePair<EnemyView, (Health Health, IEnemyMovement Movement)> pair in m_Enemies)
            {
                EnemyView enemy = pair.Key;
                IEnemyMovement movement = pair.Value.Movement;

                if (playing && !movement.IsChasing)
                {
                    movement.TryAggro(enemy.Position, vehiclePosition);
                }

                enemy.SetPose(movement.Tick(enemy.Position, vehiclePosition, Time.deltaTime));
            }
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            GameObject stickman = await m_AssetProvider.LoadAsync<GameObject>(m_Config.StickmanPrefab, cancellationToken);
            GameObject explosion = await m_AssetProvider.LoadAsync<GameObject>(m_Config.ExplosionPrefab, cancellationToken);
            GameObject damageNumber = await m_AssetProvider.LoadAsync<GameObject>(m_Config.DamageNumberPrefab, cancellationToken);

            m_Root = new GameObject(EnemyConstants.RootName).transform;
            m_EnemyPool = new ViewPool<EnemyView>(m_Instantiator, stickman.GetComponent<EnemyView>(), m_Root, m_LevelConfig.EnemyCount);
            m_ExplosionPool = new ViewPool<ExplosionView>(
                m_Instantiator,
                explosion.GetComponent<ExplosionView>(),
                m_Root,
                EnemyConstants.ExplosionPoolSize);
            m_DamageNumberPool = new ViewPool<DamageNumberView>(
                m_Instantiator,
                damageNumber.GetComponent<DamageNumberView>(),
                m_Root,
                EnemyConstants.DamageNumberPoolSize);

            foreach (EnemySpawn spawn in m_Placement.Generate(m_LevelService.FinishZ))
            {
                SpawnEnemy(spawn);
            }

            m_Ready = true;
        }

        private void SpawnEnemy(EnemySpawn spawn)
        {
            EnemyView enemy = m_EnemyPool.Get();

            enemy.Spawn(spawn.Position, Vector3.right * spawn.Direction, m_Config.Color);
            enemy.Hit += OnEnemyHit;
            enemy.ContactedVehicle += OnEnemyContactedVehicle;
            m_Enemies.Add(
                enemy,
                (new Health(m_Config.MaxHealth), m_MovementFactory.Create(spawn.LeftX, spawn.RightX, spawn.Direction)));
        }

        private void OnStateChanged(GameFlowState state)
        {
            if (state != GameFlowState.Won && state != GameFlowState.Lost)
            {
                return;
            }

            foreach ((Health Health, IEnemyMovement Movement) entry in m_Enemies.Values)
            {
                entry.Movement.Freeze();
            }
        }

        private void OnEnemyHit(EnemyView enemy, DamageHit hit)
        {
            if (!m_Enemies.TryGetValue(enemy, out (Health Health, IEnemyMovement Movement) entry) || entry.Health.IsDead)
            {
                return;
            }

            Health health = entry.Health;

            health.TakeDamage(hit.Amount);
            DamageNumberView number = m_DamageNumberPool.Get();

            number.Finished += OnDamageNumberFinished;
            number.Show(hit.Point, hit.Amount);

            if (health.IsDead)
            {
                Kill(enemy);

                return;
            }

            m_HitSplatter.Emit(hit.Point, hit.Direction, m_Config.Color);
            enemy.PlayHit(health.Normalized);
        }

        private void OnEnemyContactedVehicle(EnemyView enemy)
        {
            if (!m_Enemies.ContainsKey(enemy) || m_GameFlowService.State != GameFlowState.Playing)
            {
                return;
            }

            m_VehicleView.PlayImpact(enemy.Position);
            m_VehicleService.TakeDamage(m_Config.ContactDamage);
            Kill(enemy);
        }

        private void Kill(EnemyView enemy)
        {
            ExplosionView explosion = m_ExplosionPool.Get();

            explosion.Finished += OnExplosionFinished;
            explosion.Play(enemy.Position, m_Config.Color);

            Unsubscribe(enemy);
            m_Enemies.Remove(enemy);
            m_EnemyPool.Release(enemy);
        }

        private void OnExplosionFinished(ExplosionView explosion)
        {
            explosion.Finished -= OnExplosionFinished;
            m_ExplosionPool.Release(explosion);
        }

        private void OnDamageNumberFinished(DamageNumberView number)
        {
            number.Finished -= OnDamageNumberFinished;
            m_DamageNumberPool.Release(number);
        }

        private void Unsubscribe(EnemyView enemy)
        {
            enemy.Hit -= OnEnemyHit;
            enemy.ContactedVehicle -= OnEnemyContactedVehicle;
        }
    }
}
