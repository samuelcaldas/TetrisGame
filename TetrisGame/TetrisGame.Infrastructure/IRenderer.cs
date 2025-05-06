using TetrisGame.Domain;

namespace TetrisGame.Infrastructure
{
    /// <summary>
    /// Interface for rendering the game state
    /// </summary>
    public interface IRenderer
    {
        void Render(GameState gameState);
    }
}