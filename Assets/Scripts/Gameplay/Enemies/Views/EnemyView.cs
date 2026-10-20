using System;
using DG.Tweening;
using RunnerShooter.Gameplay.Combat;
using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    [RequireComponent(typeof(Collider))]
    public sealed class EnemyView : MonoBehaviour, IDamageable
    {
        private const int HitLayerIndex = 1;

        private static readonly int m_SpeedHash = Animator.StringToHash("Speed");
        private static readonly int m_HitReactionHash = Animator.StringToHash("HitReaction");
        private static readonly int m_BaseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField]
        private Animator m_Animator;

        [SerializeField]
        private Renderer m_Renderer;

        [SerializeField]
        private WorldHealthBarView m_HealthBar;

        [SerializeField]
        private LayerMask m_VehicleMask;

        [SerializeField]
        private float m_HitFlashDuration = 0.15f;

        [SerializeField]
        private float m_TurnSpeed = 540f;

        private MaterialPropertyBlock m_PropertyBlock;
        private Tween m_FlashTween;
        private Color m_Color;

        public event Action<EnemyView, DamageHit> Hit;
        public event Action<EnemyView> ContactedVehicle;

        public Vector3 Position => transform.position;

        public void Spawn(Vector3 position, Vector3 facing, Color color)
        {
            m_Color = color;

            transform.SetPositionAndRotation(position, Quaternion.LookRotation(facing));
            m_HealthBar.SetHealth(1f, false);
            m_HealthBar.SetVisible(false);

            m_FlashTween?.Kill();
            m_PropertyBlock ??= new MaterialPropertyBlock();
            SetFlash(0f);
        }

        public void SetPose(EnemyPose pose)
        {
            if (pose.Forward.sqrMagnitude > EnemyConstants.MinTurnDistanceSqr)
            {
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    Quaternion.LookRotation(pose.Forward),
                    m_TurnSpeed * Time.deltaTime);
            }

            transform.position = pose.Position;
            m_Animator.SetFloat(m_SpeedHash, pose.AnimationSpeed);
        }

        public void PlayHit(float normalizedHealth)
        {
            m_HealthBar.SetVisible(true);
            m_HealthBar.SetHealth(normalizedHealth, true);
            m_Animator.Play(m_HitReactionHash, HitLayerIndex, 0f);

            m_FlashTween?.Kill();
            m_FlashTween = DOVirtual
                .Float(1f, 0f, m_HitFlashDuration, SetFlash)
                .SetEase(Ease.OutQuad)
                .SetLink(gameObject);
        }

        public void TakeDamage(DamageHit hit)
        {
            Hit?.Invoke(this, hit);
        }

        private void OnDisable()
        {
            m_FlashTween?.Kill();
        }

        private void SetFlash(float amount)
        {
            m_PropertyBlock.SetColor(m_BaseColorId, Color.Lerp(m_Color, Color.white, amount));
            m_Renderer.SetPropertyBlock(m_PropertyBlock);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((m_VehicleMask.value & (1 << other.gameObject.layer)) != 0)
            {
                ContactedVehicle?.Invoke(this);
            }
        }
    }
}
