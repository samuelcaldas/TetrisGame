using System;
using System.Threading;
using TetrisGame.Domain;
using TetrisGame.Infrastructure;

namespace TetrisGame.Application
{
    /// <summary>
    /// Game engine implementation for human players
    /// </summary>
    public class HumanGameEngine : GameEngine
    {
        private readonly IInputHandler _inputHandler;
        private int _gameSpeed;
        
        public HumanGameEngine(IEnvironment environment, IInputHandler inputHandler) 
            : base(environment)
        {
            _inputHandler = inputHandler;
            _gameSpeed = 500; // Start with 500ms per tick
        }
        
        public override void Play()
        {
            // Reset the environment
            var state = Environment.Reset();
            bool isRunning = true;
            
            // Game loop
            while (isRunning)
            {
                // Clear the console
                Console.Clear();
                
                // Render the current state
                Environment.RenderConsole();
                
                // Get the next action from the human player
                Domain.Action action = Domain.Action.None;
                
                // Check for input, non-blocking
                if (Console.KeyAvailable)
                {
                    action = _inputHandler.GetAction();
                }
                
                // Step the environment with the action
                var (newState, reward, done) = Environment.Step(action);
                
                // Apply automatic drop every _gameSpeed milliseconds
                if (action != Domain.Action.SoftDrop && action != Domain.Action.HardDrop)
                {
                    Thread.Sleep(_gameSpeed);
                    Environment.Step(Domain.Action.SoftDrop);
                }
                
                // Adjust speed based on level
                _gameSpeed = Math.Max(100, 500 - (newState.Level - 1) * 50);
                
                // Check if game is over
                if (done)
                {
                    Console.Clear();
                    Environment.RenderConsole();
                    Console.WriteLine("Game Over! Final Score: " + newState.Score);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    isRunning = false;
                }
            }
        }
    }
}