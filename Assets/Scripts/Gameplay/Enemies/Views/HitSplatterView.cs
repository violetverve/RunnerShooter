using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class HitSplatterView : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem m_ParticleSystem;

        [SerializeField]
        private float m_Speed = 3f;

        [SerializeField]
        private float m_Spread = 0.6f;

        [SerializeField]
        private int m_ParticleCount = 6;

        public void Emit(Vector3 position, Vector3 direction, Color color)
        {
            ParticleSystem.EmitParams emitParams = new()
            {
                position = position,
                startColor = color
            };

            for (int i = 0; i < m_ParticleCount; i++)
            {
                emitParams.velocity = (-direction + Random.insideUnitSphere * m_Spread).normalized * m_Speed;
                m_ParticleSystem.Emit(emitParams, 1);
            }
        }
    }
}
