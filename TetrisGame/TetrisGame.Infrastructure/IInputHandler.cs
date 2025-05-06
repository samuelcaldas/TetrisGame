using TetrisGame.Domain;

namespace TetrisGame.Infrastructure
{
    /// <summary>
    /// Interface for handling user input
    /// </summary>
    public interface IInputHandler
    {
        Action GetAction();
    }
}