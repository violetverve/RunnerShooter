using UnityEngine;

namespace RunnerShooter.Gameplay.Vehicle
{
    public sealed class VehicleWreckPiece : MonoBehaviour
    {
        [SerializeField]
        private float m_MinDistance = 1f;

        [SerializeField]
        private float m_MaxDistance = 3f;

        [SerializeField]
        private float m_JumpHeight = 2f;

        [SerializeField]
        private float m_LandHeight;

        [SerializeField]
        private Vector3 m_MaxSpin = new(360f, 360f, 360f);

        public float JumpHeight => m_JumpHeight;

        public Vector3 PickLandingOffset()
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(m_MinDistance, m_MaxDistance);

            return new Vector3(direction.x * distance, 0f, direction.y * distance);
        }

        public float LandY(float groundY)
        {
            return groundY + m_LandHeight;
        }

        public Vector3 PickSpin()
        {
            return new Vector3(
                Random.Range(-m_MaxSpin.x, m_MaxSpin.x),
                Random.Range(-m_MaxSpin.y, m_MaxSpin.y),
                Random.Range(-m_MaxSpin.z, m_MaxSpin.z));
        }
    }
}
