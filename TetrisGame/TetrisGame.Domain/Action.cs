namespace TetrisGame.Domain
{
    /// <summary>
    /// Represents possible actions that can be taken in the game
    /// </summary>
    public enum Action
    {
        MoveLeft,
        MoveRight,
        RotateClockwise,
        SoftDrop,
        HardDrop,
        None
    }
}