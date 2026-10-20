using System;
using RunnerShooter.Gameplay.Combat;
using UnityEngine;

namespace RunnerShooter.Gameplay.Turret
{
    public sealed class BulletView : MonoBehaviour
    {
        [SerializeField]
        private TrailRenderer m_Trail;

        [SerializeField]
        private LayerMask m_HitMask;

        private Vector3 m_Velocity;
        private float m_TimeLeft;
        private float m_Damage;

        public event Action<BulletView> Expired;

        public void Launch(Vector3 position, Vector3 direction, float speed, float lifetime, float damage)
        {
            transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
            m_Velocity = direction * speed;
            m_TimeLeft = lifetime;
            m_Damage = damage;
            m_Trail.Clear();
        }

        private void Update()
        {
            Vector3 step = m_Velocity * Time.deltaTime;

            if (TryHit(step))
            {
                Expired?.Invoke(this);

                return;
            }

            transform.position += step;
            m_TimeLeft -= Time.deltaTime;

            if (m_TimeLeft <= 0f)
            {
                Expired?.Invoke(this);
            }
        }

        private bool TryHit(Vector3 step)
        {
            if (!Physics.Raycast(transform.position, step, out RaycastHit hit, step.magnitude, m_HitMask, QueryTriggerInteraction.Collide))
            {
                return false;
            }

            IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();

            damageable?.TakeDamage(new DamageHit(m_Damage, hit.point, step.normalized));

            return true;
        }
    }
}
