using System;
using RunnerShooter.Gameplay.Combat;
using RunnerShooter.Gameplay.GameFlow;
using RunnerShooter.Gameplay.Vehicle;
using Zenject;

namespace RunnerShooter.Gameplay.UI
{
    public sealed class VehicleHealthPresenter : IInitializable, IDisposable
    {
        private readonly IVehicleService m_VehicleService;
        private readonly IGameFlowService m_GameFlowService;
        private readonly WorldHealthBarView m_View;

        public VehicleHealthPresenter(IVehicleService vehicleService, IGameFlowService gameFlowService, WorldHealthBarView view)
        {
            m_VehicleService = vehicleService;
            m_GameFlowService = gameFlowService;
            m_View = view;
        }

        public void Initialize()
        {
            m_View.SetHealth(m_VehicleService.CurrentHealth / m_VehicleService.MaxHealth, false);
            OnStateChanged(m_GameFlowService.State);
            m_VehicleService.HealthChanged += OnHealthChanged;
            m_GameFlowService.StateChanged += OnStateChanged;
        }

        public void Dispose()
        {
            m_VehicleService.HealthChanged -= OnHealthChanged;
            m_GameFlowService.StateChanged -= OnStateChanged;
        }

        private void OnHealthChanged(float health)
        {
            m_View.SetHealth(health / m_VehicleService.MaxHealth, true);
        }

        private void OnStateChanged(GameFlowState state)
        {
            m_View.SetVisible(state == GameFlowState.Playing || state == GameFlowState.Won);
        }
    }
}
