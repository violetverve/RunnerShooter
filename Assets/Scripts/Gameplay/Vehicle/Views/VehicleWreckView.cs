using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace RunnerShooter.Gameplay.Vehicle
{
    public sealed class VehicleWreckView : MonoBehaviour
    {
        [SerializeField]
        private VehicleWreckPiece[] m_Pieces;

        [SerializeField]
        private Transform m_SmokeAnchor;

        [SerializeField]
        private float m_FirstJumpShare = 0.7f;

        [SerializeField]
        private float m_FirstJumpDistanceShare = 0.8f;

        [SerializeField]
        private float m_HopHeightShare = 0.25f;

        public Transform SmokeAnchor => m_SmokeAnchor;

        public async UniTask ScatterAsync(float duration, float groundY, CancellationToken cancellationToken)
        {
            Sequence sequence = DOTween.Sequence().SetLink(gameObject);
            float firstDuration = duration * m_FirstJumpShare;
            float secondDuration = duration - firstDuration;

            foreach (VehicleWreckPiece piece in m_Pieces)
            {
                Transform pieceTransform = piece.transform;
                Vector3 start = pieceTransform.position;
                Vector3 landing = start + piece.PickLandingOffset();

                landing.y = piece.LandY(groundY);

                Vector3 midpoint = Vector3.Lerp(start, landing, m_FirstJumpDistanceShare);

                midpoint.y = landing.y;

                Sequence pieceMove = DOTween.Sequence();

                _ = pieceMove.Append(pieceTransform.DOJump(midpoint, piece.JumpHeight, 1, firstDuration));
                _ = pieceMove.Append(pieceTransform.DOJump(
                    landing,
                    piece.JumpHeight * m_HopHeightShare,
                    1,
                    secondDuration));

                _ = sequence.Insert(0f, pieceMove);
                _ = sequence.Insert(
                    0f,
                    pieceTransform.DORotate(piece.PickSpin(), duration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad));
            }

            await sequence.ToUniTask(cancellationToken: cancellationToken);
        }
    }
}
