using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Core.Assets;
using RunnerShooter.Core.Pooling;
using RunnerShooter.Gameplay.GameFlow;
using RunnerShooter.Gameplay.Input;
using RunnerShooter.Gameplay.Vehicle;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.Turret
{
    public sealed class TurretSystem : ITurretSystem
    {
        private readonly TurretConfig m_Config;
        private readonly IGameFlowService m_GameFlowService;
        private readonly IAssetProvider m_AssetProvider;
        private readonly IInstantiator m_Instantiator;
        private readonly IVehicleService m_VehicleService;
        private readonly IPlayerInput m_PlayerInput;
        private readonly TurretView m_View;
        private readonly VehicleView m_VehicleView;

        private Transform m_BulletRoot;
        private ViewPool<BulletView> m_BulletPool;
        private float m_YawAtDragStart;
        private float m_TargetYaw;
        private float m_Yaw;
        private float m_Cooldown;

        public TurretSystem(
            TurretConfig config,
            IGameFlowService gameFlowService,
            IAssetProvider assetProvider,
            IInstantiator instantiator,
            IVehicleService vehicleService,
            IPlayerInput playerInput,
            TurretView view,
            VehicleView vehicleView)
        {
            m_Config = config;
            m_GameFlowService = gameFlowService;
            m_AssetProvider = assetProvider;
            m_Instantiator = instantiator;
            m_VehicleService = vehicleService;
            m_PlayerInput = playerInput;
            m_View = view;
            m_VehicleView = vehicleView;
        }

        public void Initialize()
        {
            m_View.AttachTo(m_VehicleView.TurretMount);
            m_GameFlowService.StateChanged += OnStateChanged;
            m_View.SetLaserLength(m_Config.LaserLength);
            UpdateLaser(m_GameFlowService.State);
        }

        public void Dispose()
        {
            m_GameFlowService.StateChanged -= OnStateChanged;
            m_BulletPool?.Dispose();
        }

        public void Tick()
        {
            if (m_GameFlowService.State != GameFlowState.Playing)
            {
                return;
            }

            bool pressed = m_PlayerInput.IsPressed;

            if (m_PlayerInput.PressedThisFrame)
            {
                m_YawAtDragStart = m_TargetYaw;
            }

            if (pressed)
            {
                Aim();
            }

            m_Yaw = Mathf.Lerp(m_Yaw, m_TargetYaw, 1f - Mathf.Exp(-m_Config.AimSmoothing * Time.deltaTime));
            m_View.SetYaw(m_Yaw);

            m_VehicleService.SetSteer(-m_Yaw / m_Config.MaxYawAngle);

            m_Cooldown = Mathf.Max(0f, m_Cooldown - Time.deltaTime);

            if (pressed && m_Cooldown <= 0f)
            {
                Fire();
            }
        }

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            GameObject prefab = await m_AssetProvider.LoadAsync<GameObject>(m_Config.BulletPrefab, cancellationToken);

            m_BulletRoot = new GameObject(TurretConstants.BulletRootName).transform;
            m_BulletPool = new ViewPool<BulletView>(
                m_Instantiator,
                prefab.GetComponent<BulletView>(),
                m_BulletRoot,
                TurretConstants.BulletPoolSize);
        }

        private void Aim()
        {
            float yaw = m_YawAtDragStart + m_PlayerInput.DragDelta * m_Config.DragDegreesPerScreenWidth;

            m_TargetYaw = Mathf.Clamp(yaw, -m_Config.MaxYawAngle, m_Config.MaxYawAngle);
        }

        private void Fire()
        {
            if (m_BulletPool == null)
            {
                return;
            }

            BulletView bullet = m_BulletPool.Get();

            bullet.Expired += OnBulletExpired;
            bullet.Launch(m_View.MuzzlePosition, m_View.MuzzleForward, m_Config.BulletSpeed, m_Config.BulletLifetime, m_Config.BulletDamage);
            m_View.PlayMuzzleFlash();
            m_Cooldown += 1f / m_Config.ShotsPerSecond;
        }

        private void OnBulletExpired(BulletView bullet)
        {
            bullet.Expired -= OnBulletExpired;
            m_BulletPool.Release(bullet);
        }

        private void OnStateChanged(GameFlowState state)
        {
            UpdateLaser(state);
        }

        private void UpdateLaser(GameFlowState state)
        {
            m_View.SetLaserVisible(state == GameFlowState.Playing);
        }
    }
}
