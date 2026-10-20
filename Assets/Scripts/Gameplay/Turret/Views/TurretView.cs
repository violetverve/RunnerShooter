using UnityEngine;

namespace RunnerShooter.Gameplay.Turret
{
    public sealed class TurretView : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Muzzle;

        [SerializeField]
        private LineRenderer m_Laser;

        [SerializeField]
        private ParticleSystem m_MuzzleFlash;

        private float m_LaserLength;

        public Vector3 MuzzlePosition => m_Muzzle.position;
        public Vector3 MuzzleForward => m_Muzzle.forward;

        public void AttachTo(Transform mount)
        {
            transform.SetParent(mount, false);
        }

        public void SetYaw(float yaw)
        {
            transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
        }

        public void SetLaserVisible(bool visible)
        {
            m_Laser.enabled = visible;
        }

        public void SetLaserLength(float length)
        {
            m_LaserLength = length;
        }

        public void PlayMuzzleFlash()
        {
            m_MuzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            m_MuzzleFlash.Play();
        }

        private void LateUpdate()
        {
            if (!m_Laser.enabled)
            {
                return;
            }

            m_Laser.SetPosition(0, m_Muzzle.position);
            m_Laser.SetPosition(1, m_Muzzle.position + m_Muzzle.forward * m_LaserLength);
        }
    }
}
