using System;

namespace RunnerShooter.Gameplay.GameFlow
{
    public sealed class GameFlowService : IGameFlowService
    {
        public event Action<GameFlowState> StateChanged;
        public event Action RestartRequested;

        public GameFlowState State { get; private set; } = GameFlowState.Loading;

        public void FinishLoading()
        {
            if (State == GameFlowState.Loading)
            {
                SetState(GameFlowState.WaitingForTap);
            }
        }

        public void Tap()
        {
            switch (State)
            {
                case GameFlowState.WaitingForTap:
                    SetState(GameFlowState.Playing);

                    break;
                case GameFlowState.Won:
                case GameFlowState.Lost:
                    RestartRequested?.Invoke();

                    break;
            }
        }

        public void Win()
        {
            if (State == GameFlowState.Playing)
            {
                SetState(GameFlowState.Won);
            }
        }

        public void Lose()
        {
            if (State == GameFlowState.Playing)
            {
                SetState(GameFlowState.Lost);
            }
        }

        private void SetState(GameFlowState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
