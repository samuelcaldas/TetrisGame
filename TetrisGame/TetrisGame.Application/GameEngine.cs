using System;
using System.Threading;
using TetrisGame.Domain;
using TetrisGame.Infrastructure;

namespace TetrisGame.Application
{
    /// <summary>
    /// Abstract base class for game engines that control game flow
    /// </summary>
    public abstract class GameEngine
    {
        protected readonly IEnvironment Environment;
        
        protected GameEngine(IEnvironment environment)
        {
            Environment = environment;
        }
        
        /// <summary>
        /// Start playing the game
        /// </summary>
        public abstract void Play();
    }
}