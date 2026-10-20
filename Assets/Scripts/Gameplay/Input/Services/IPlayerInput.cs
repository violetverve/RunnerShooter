namespace RunnerShooter.Gameplay.Input
{
    public interface IPlayerInput
    {
        bool IsPressed { get; }

        bool PressedThisFrame { get; }

        /// <summary>
        /// Horizontal pointer movement since the press started, in screen widths.
        /// </summary>
        float DragDelta { get; }
    }
}
