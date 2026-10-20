using System;

namespace RunnerShooter.Gameplay.GameFlow
{
    public interface IGameFlowService
    {
        event Action<GameFlowState> StateChanged;

        /// <summary>
        /// Raised when the player taps after the game has ended.
        /// </summary>
        event Action RestartRequested;

        GameFlowState State { get; }

        /// <summary>
        /// Loading to WaitingForTap, once all assets are loaded and pools are warmed.
        /// </summary>
        void FinishLoading();

        /// <summary>
        /// Starts the game from WaitingForTap, or requests a restart from Won / Lost. Ignored otherwise.
        /// </summary>
        void Tap();

        void Win();

        void Lose();
    }
}
