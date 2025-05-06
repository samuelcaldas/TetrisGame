using TetrisGame.Domain;

namespace TetrisGame.Application
{
    /// <summary>
    /// Interface for the game environment that supports reinforcement learning
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Resets the environment to an initial state
        /// </summary>
        /// <returns>The initial state</returns>
        State Reset();
        
        /// <summary>
        /// Takes an action in the environment
        /// </summary>
        /// <param name="action">The action to take</param>
        /// <returns>The new state, reward, and whether the episode is done</returns>
        (State NewState, double Reward, bool Done) Step(Action action);
        
        /// <summary>
        /// Renders the current state to the console
        /// </summary>
        void RenderConsole();
    }
}