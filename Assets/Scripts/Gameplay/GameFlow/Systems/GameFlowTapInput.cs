using RunnerShooter.Gameplay.Input;
using Zenject;

namespace RunnerShooter.Gameplay.GameFlow
{
    public sealed class GameFlowTapInput : ITickable
    {
        private readonly IGameFlowService m_GameFlowService;
        private readonly IPlayerInput m_PlayerInput;

        public GameFlowTapInput(IGameFlowService gameFlowService, IPlayerInput playerInput)
        {
            m_GameFlowService = gameFlowService;
            m_PlayerInput = playerInput;
        }

        public void Tick()
        {
            if (m_PlayerInput.PressedThisFrame)
            {
                m_GameFlowService.Tap();
            }
        }
    }
}
