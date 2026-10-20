using UnityEngine;

namespace RunnerShooter.Gameplay.Level
{
    public sealed class FinishLineView : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] m_Confetti;

        public void PlayConfetti()
        {
            foreach (ParticleSystem confetti in m_Confetti)
            {
                confetti.Play(true);
            }
        }
    }
}
