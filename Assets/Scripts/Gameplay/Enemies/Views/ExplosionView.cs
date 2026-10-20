using System;
using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class ExplosionView : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem m_ParticleSystem;

        [SerializeField]
        private float m_HeightOffset = 1f;

        public event Action<ExplosionView> Finished;

        public void Play(Vector3 position, Color color)
        {
            transform.position = position + Vector3.up * m_HeightOffset;

            ParticleSystem.MainModule main = m_ParticleSystem.main;

            main.startColor = color;
            m_ParticleSystem.Play(true);
        }

        private void OnParticleSystemStopped()
        {
            Finished?.Invoke(this);
        }
    }
}
