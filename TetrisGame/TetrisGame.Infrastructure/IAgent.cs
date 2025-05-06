using TetrisGame.Domain;

namespace TetrisGame.Infrastructure
{
    /// <summary>
    /// Interface for agents that can play the game
    /// </summary>
    public interface IAgent
    {
        Action GetAction(State state);
    }
}