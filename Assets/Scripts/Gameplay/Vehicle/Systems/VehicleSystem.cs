using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RunnerShooter.Gameplay.GameFlow;
using RunnerShooter.Gameplay.Level;
using UnityEngine;
using Zenject;

namespace RunnerShooter.Gameplay.Vehicle
{
    public sealed class VehicleSystem : IVehicleSystem
    {
        private readonly VehicleConfig m_Config;
        private readonly IVehicleService m_VehicleService;
        private readonly ILevelService m_LevelService;
        private readonly IGameFlowService m_GameFlowService;
        private readonly VehicleView m_View;
        private readonly CancellationTokenSource m_Cts = new();
        private bool m_IsCritical;
        private float m_X;
        private float m_Z;
        private float m_CoastSpeed;

        public VehicleSystem(
            VehicleConfig config,
            IVehicleService vehicleService,
            ILevelService levelService,
            IGameFlowService gameFlowService,
            VehicleView view)
        {
            m_Config = config;
            m_VehicleService = vehicleService;
            m_LevelService = levelService;
            m_GameFlowService = gameFlowService;
            m_View = view;
        }

        public void Initialize()
        {
            m_Z = LevelConstants.StartZ;
            m_View.SetPosition(m_X, m_Z);
            m_View.SetMoving(false);
            m_View.SetSmoke(VehicleSmokeLevel.Idle);
            m_LevelService.SetVehicleZ(m_View.Z);
            m_VehicleService.HealthChanged += OnHealthChanged;
            m_VehicleService.Destroyed += OnDestroyed;
            m_GameFlowService.StateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            m_VehicleService.HealthChanged -= OnHealthChanged;
            m_VehicleService.Destroyed -= OnDestroyed;
            m_GameFlowService.StateChanged -= OnStateChanged;
            m_Cts.Cancel();
            m_Cts.Dispose();
        }

        public void Tick()
        {
            switch (m_GameFlowService.State)
            {
                case GameFlowState.Playing:
                    Drive();

                    break;
                case GameFlowState.Won:
                    Coast();

                    break;
            }
        }

        private void Drive()
        {
            float targetX = m_VehicleService.Steer * m_Config.SwayDistance;

            m_X = Mathf.Lerp(m_X, targetX, 1f - Mathf.Exp(-m_Config.SwaySmoothing * Time.deltaTime));
            m_Z += m_Config.MoveSpeed * Time.deltaTime;
            m_View.SetPosition(m_X, m_Z);
            m_LevelService.SetVehicleZ(m_Z);

            if (m_Z >= m_LevelService.FinishZ)
            {
                m_GameFlowService.Win();
            }
        }

        private void Coast()
        {
            if (m_CoastSpeed <= 0f)
            {
                return;
            }

            float deceleration = m_Config.MoveSpeed * m_Config.MoveSpeed / (2f * m_Config.FinishCoastDistance);

            m_CoastSpeed = Mathf.Max(m_CoastSpeed - deceleration * Time.deltaTime, 0f);
            m_Z += m_CoastSpeed * Time.deltaTime;
            m_View.SetPosition(m_X, m_Z);

            if (m_CoastSpeed <= 0f)
            {
                m_View.SetMoving(false);
                m_View.SetSmoke(VehicleSmokeLevel.Idle);
            }
        }

        private void OnStateChanged(GameFlowState state)
        {
            switch (state)
            {
                case GameFlowState.Playing:
                    m_View.SetMoving(true);
                    m_View.SetSmoke(VehicleSmokeLevel.Driving);

                    break;
                case GameFlowState.Won:
                    m_CoastSpeed = m_Config.MoveSpeed;

                    break;
            }
        }

        private void OnHealthChanged(float health)
        {
            if (m_IsCritical || health <= 0f || health > m_VehicleService.MaxHealth * m_Config.CriticalHealthShare)
            {
                return;
            }

            m_IsCritical = true;
            m_View.PlayCriticalDamage();
        }

        private void OnDestroyed()
        {
            m_GameFlowService.Lose();
            m_View.PlayDestroyedAsync(m_Cts.Token).Forget();
        }
    }
}
