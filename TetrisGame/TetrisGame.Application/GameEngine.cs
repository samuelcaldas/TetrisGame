using System;
using System.Threading;
using TetrisGame.Domain;
using TetrisGame.Infrastructure;

namespace TetrisGame.Application
{
    /// <summary>
    /// Main game engine that controls game flow
    /// </summary>
    public class GameEngine
    {
        private readonly IEnvironment _environment;
        private readonly IInputHandler _inputHandler;
        private int _gameSpeed;

        public GameEngine(IEnvironment environment, IInputHandler inputHandler)
        {
            _environment = environment;
            _inputHandler = inputHandler;
            _gameSpeed = 500; // Start with 500ms per tick
        }
        
        public void PlayHuman()
        {
            // Reset the environment
            var state = _environment.Reset();
            bool isRunning = true;
            
            // Game loop
            while (isRunning)
            {
                // Clear the console
                Console.Clear();
                
                // Render the current state
                _environment.RenderConsole();
                
                // Get the next action from the human player
                Domain.Action action = Domain.Action.None;
                
                // Check for input, non-blocking
                if (Console.KeyAvailable)
                {
                    action = _inputHandler.GetAction();
                }
                
                // Step the environment with the action
                var (newState, reward, done) = _environment.Step(action);
                
                // Apply automatic drop every _gameSpeed milliseconds
                if (action != Domain.Action.SoftDrop && action != Domain.Action.HardDrop)
                {
                    Thread.Sleep(_gameSpeed);
                    _environment.Step(Domain.Action.SoftDrop);
                }
                
                // Adjust speed based on level
                _gameSpeed = Math.Max(100, 500 - (newState.Level - 1) * 50);
                
                // Check if game is over
                if (done)
                {
                    Console.Clear();
                    _environment.RenderConsole();
                    Console.WriteLine("Game Over! Final Score: " + newState.Score);
                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    isRunning = false;
                }
            }
        }
        
        public void PlayAgent(IAgent agent, bool render = true, int maxSteps = 1000)
        {
            // Reset the environment
            var state = _environment.Reset();
            int steps = 0;
            double totalReward = 0;
            
            // Game loop
            while (steps < maxSteps)
            {
                // Get action from agent
                Domain.Action action = agent.GetAction(state);
                
                // Step the environment with the action
                var (newState, reward, done) = _environment.Step(action);
                totalReward += reward;
                
                // Apply automatic drop every few steps
                if (steps % 5 == 0 && action != Domain.Action.SoftDrop && action != Domain.Action.HardDrop)
                {
                    var (dropState, dropReward, dropDone) = _environment.Step(Domain.Action.SoftDrop);
                    newState = dropState;
                    reward += dropReward;
                    done = dropDone;
                    totalReward += dropReward;
                }
                
                // Render if requested
                if (render && steps % 5 == 0)
                {
                    Console.Clear();
                    _environment.RenderConsole();
                    Console.WriteLine($"Step: {steps}, Total Reward: {totalReward:F2}");
                    Thread.Sleep(100); // Slow down rendering
                }
                
                // Update the state
                state = newState;
                steps++;
                
                // Check if game is over
                if (done)
                {
                    if (render)
                    {
                        Console.Clear();
                        _environment.RenderConsole();
                        Console.WriteLine($"Game Over! Final Score: {state.Score}, Total Reward: {totalReward:F2}");
                        Thread.Sleep(2000);
                    }
                    break;
                }
            }
        }
    }
}