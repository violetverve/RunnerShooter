using UnityEngine;

namespace RunnerShooter.Gameplay.Enemies
{
    public sealed class EnemyMovement : IEnemyMovement
    {
        private readonly EnemyConfig m_Config;
        private readonly float m_LeftX;
        private readonly float m_RightX;

        private int m_Direction;
        private float m_PauseLeft;
        private bool m_Frozen;

        public EnemyMovement(EnemyConfig config, float leftX, float rightX, int direction)
        {
            m_Config = config;
            m_LeftX = leftX;
            m_RightX = rightX;
            m_Direction = direction;
        }

        public bool IsChasing { get; private set; }

        public void Freeze()
        {
            m_Frozen = true;
        }

        public void TryAggro(Vector3 position, Vector3 vehiclePosition)
        {
            Vector2 offset = new(vehiclePosition.x - position.x, vehiclePosition.z - position.z);

            if (offset.magnitude <= m_Config.AggroDistance)
            {
                IsChasing = true;
            }
        }

        public EnemyPose Tick(Vector3 position, Vector3 vehiclePosition, float deltaTime)
        {
            if (m_Frozen)
            {
                return new EnemyPose(position, Vector3.zero, 0f);
            }

            return IsChasing ? Chase(position, vehiclePosition, deltaTime) : Wander(position, deltaTime);
        }

        private EnemyPose Chase(Vector3 position, Vector3 vehiclePosition, float deltaTime)
        {
            return MoveTowards(position, vehiclePosition, m_Config.RunSpeed, deltaTime);
        }

        private EnemyPose Wander(Vector3 position, float deltaTime)
        {
            if (m_PauseLeft > 0f)
            {
                m_PauseLeft -= deltaTime;

                return new EnemyPose(position, Vector3.zero, 0f);
            }

            float targetX = m_Direction > 0 ? m_RightX : m_LeftX;
            EnemyPose pose = MoveTowards(position, new Vector3(targetX, position.y, position.z), m_Config.WanderSpeed, deltaTime);

            if (Mathf.Approximately(pose.Position.x, targetX))
            {
                m_Direction = -m_Direction;
                m_PauseLeft = m_Config.WanderPause;
            }

            return pose;
        }

        private static EnemyPose MoveTowards(Vector3 position, Vector3 target, float speed, float deltaTime)
        {
            Vector3 offset = target - position;

            offset.y = 0f;

            return new EnemyPose(Vector3.MoveTowards(position, target, speed * deltaTime), offset, speed);
        }
    }
}
