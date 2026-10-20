using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RunnerShooter.Gameplay.Combat;
using Unity.Cinemachine;
using UnityEngine;

namespace RunnerShooter.Gameplay.Vehicle
{
    public sealed class VehicleView : MonoBehaviour
    {
        [SerializeField]
        private TrailRenderer[] m_Trails;

        [SerializeField]
        private ParticleSystem m_Smoke;

        [SerializeField]
        private ParticleSystem m_ImpactSparks;

        [SerializeField]
        private ParticleSystem m_Explosion;

        [SerializeField]
        private ParticleSystem m_Fire;

        [SerializeField]
        private ParticleSystem m_CriticalDamage;

        [SerializeField]
        private CinemachineImpulseSource m_ImpulseSource;

        [SerializeField]
        private GameObject[] m_IntactParts;

        [SerializeField]
        private VehicleWreckView m_Wreck;

        [SerializeField]
        private Transform m_TurretMount;

        [SerializeField]
        private WorldHealthBarView m_HealthBar;

        [SerializeField]
        private Material m_HitFlashMaterial;

        [Header("Smoke")]
        [SerializeField]
        private float m_IdleSmokeRate = 4f;

        [SerializeField]
        private float m_DrivingSmokeRate = 18f;

        [SerializeField]
        private float m_DestroyedSmokeRate = 40f;

        [SerializeField]
        private Color m_NormalSmokeColor = new(0.14f, 0.14f, 0.14f, 0.9f);

        [SerializeField]
        private Color m_DestroyedSmokeColor = new(0.05f, 0.05f, 0.05f, 1f);

        [Header("Impact")]
        [SerializeField]
        private int m_ImpactSparkCount = 14;

        [SerializeField]
        private float m_ImpactHeight = 0.6f;

        [SerializeField]
        private float m_ImpactImpulse = 0.06f;

        [SerializeField]
        private float m_PunchStrength = 0.06f;

        [SerializeField]
        private float m_PunchDuration = 0.25f;

        [SerializeField]
        private float m_HitFlashDuration = 0.08f;

        [Header("Destruction")]
        [SerializeField]
        private float m_DestroyedImpulseScale = 2.5f;

        [SerializeField]
        private float m_WreckScatterDuration = 0.9f;

        [SerializeField]
        private float m_FireDelay = 0.6f;

        private MeshRenderer[] m_FlashRenderers;
        private Material[] m_OriginalMaterials;
        private Tween m_FlashTween;

        public float Z => transform.position.z;
        public Vector3 Position => transform.position;
        public Transform TurretMount => m_TurretMount;
        public WorldHealthBarView HealthBar => m_HealthBar;

        public void SetPosition(float x, float z)
        {
            Vector3 position = transform.position;

            position.x = x;
            position.z = z;
            transform.position = position;
        }

        public void SetMoving(bool moving)
        {
            foreach (TrailRenderer trail in m_Trails)
            {
                trail.emitting = moving;
            }
        }

        public void SetSmoke(VehicleSmokeLevel level)
        {
            ParticleSystem.MainModule main = m_Smoke.main;
            ParticleSystem.EmissionModule emission = m_Smoke.emission;

            main.startColor = level == VehicleSmokeLevel.Destroyed
                ? m_DestroyedSmokeColor
                : m_NormalSmokeColor;
            emission.rateOverTime = level switch
            {
                VehicleSmokeLevel.Driving => m_DrivingSmokeRate,
                VehicleSmokeLevel.Destroyed => m_DestroyedSmokeRate,
                _ => m_IdleSmokeRate
            };
        }

        public void PlayImpact(Vector3 contactPoint)
        {
            contactPoint.y = transform.position.y + m_ImpactHeight;
            m_ImpactSparks.transform.position = contactPoint;
            m_ImpactSparks.Emit(m_ImpactSparkCount);
            m_ImpulseSource.GenerateImpulse(m_ImpactImpulse);

            foreach (GameObject part in m_IntactParts)
            {
                part.transform.DOComplete();
                part.transform
                    .DOPunchScale(Vector3.one * m_PunchStrength, m_PunchDuration)
                    .SetLink(part);
            }

            SetFlash(true);
            m_FlashTween?.Kill();
            m_FlashTween = DOVirtual
                .DelayedCall(m_HitFlashDuration, () => SetFlash(false))
                .SetLink(gameObject);
        }

        public void PlayCriticalDamage()
        {
            m_CriticalDamage.Play(true);
        }

        public async UniTask PlayDestroyedAsync(CancellationToken cancellationToken)
        {
            m_FlashTween?.Kill();

            foreach (TrailRenderer trail in m_Trails)
            {
                trail.gameObject.SetActive(false);
            }

            foreach (GameObject part in m_IntactParts)
            {
                part.transform.DOKill(true);
                part.SetActive(false);
            }

            m_Wreck.gameObject.SetActive(true);
            m_Explosion.Play(true);
            m_CriticalDamage.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            m_ImpulseSource.GenerateImpulse(m_ImpactImpulse * m_DestroyedImpulseScale);

            m_Smoke.transform.SetParent(m_Wreck.SmokeAnchor, false);
            m_Smoke.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            SetSmoke(VehicleSmokeLevel.Destroyed);

            await UniTask.WhenAll(
                m_Wreck.ScatterAsync(m_WreckScatterDuration, transform.position.y, cancellationToken),
                PlayFireAsync(cancellationToken));
        }

        private async UniTask PlayFireAsync(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(m_FireDelay), cancellationToken: cancellationToken);
            m_Fire.Play(true);
        }

        private void SetFlash(bool flashing)
        {
            // Collected lazily because the turret is attached to the mount at runtime.
            if (m_FlashRenderers == null)
            {
                m_FlashRenderers = m_IntactParts.SelectMany(part => part.GetComponentsInChildren<MeshRenderer>()).ToArray();
                m_OriginalMaterials = m_FlashRenderers.Select(flashRenderer => flashRenderer.sharedMaterial).ToArray();
            }

            for (int i = 0; i < m_FlashRenderers.Length; i++)
            {
                m_FlashRenderers[i].sharedMaterial = flashing ? m_HitFlashMaterial : m_OriginalMaterials[i];
            }
        }
    }
}
