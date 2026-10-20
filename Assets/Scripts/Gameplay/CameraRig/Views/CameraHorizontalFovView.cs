using Unity.Cinemachine;
using UnityEngine;

namespace RunnerShooter.Gameplay.CameraRig
{
    [ExecuteAlways]
    [RequireComponent(typeof(CinemachineCamera))]
    public sealed class CameraHorizontalFovView : MonoBehaviour
    {
        [SerializeField]
        private float m_HorizontalFov = 30f;

        private CinemachineCamera m_Camera;

        private void OnEnable()
        {
            m_Camera = GetComponent<CinemachineCamera>();
        }

        private void Update()
        {
            float aspect = (float)Screen.width / Screen.height;
            float halfHorizontal = m_HorizontalFov * 0.5f * Mathf.Deg2Rad;
            LensSettings lens = m_Camera.Lens;

            lens.FieldOfView = 2f * Mathf.Atan(Mathf.Tan(halfHorizontal) / aspect) * Mathf.Rad2Deg;
            m_Camera.Lens = lens;
        }
    }
}
