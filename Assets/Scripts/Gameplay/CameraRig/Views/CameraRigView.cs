using Unity.Cinemachine;
using UnityEngine;

namespace RunnerShooter.Gameplay.CameraRig
{
    public sealed class CameraRigView : MonoBehaviour
    {
        [SerializeField]
        private Camera m_MainCamera;

        [SerializeField]
        private CinemachineCamera m_FollowCamera;

        public Camera MainCamera => m_MainCamera;

        public void SetFollowTarget(Transform target)
        {
            m_FollowCamera.Target.TrackingTarget = target;
        }
    }
}
