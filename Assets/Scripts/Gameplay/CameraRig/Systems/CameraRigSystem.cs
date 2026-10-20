using RunnerShooter.Gameplay.Vehicle;
using Zenject;

namespace RunnerShooter.Gameplay.CameraRig
{
    public sealed class CameraRigSystem : ICameraRigSystem
    {
        private readonly CameraRigView m_View;
        private readonly VehicleView m_VehicleView;

        public CameraRigSystem(CameraRigView view, VehicleView vehicleView)
        {
            m_View = view;
            m_VehicleView = vehicleView;
        }

        public void Initialize()
        {
            m_View.SetFollowTarget(m_VehicleView.transform);
        }
    }
}
