using System;
using System.Threading;
using TetrisGame.Domain;
using TetrisGame.Infrastructure;

namespace TetrisGame.Application
{
    /// <summary>
    /// Game engine implementation for AI agents
    /// </summary>
    public class AgentGameEngine : GameEngine
    {
        private readonly IAgent _agent;
        private readonly bool _renderEnabled;
        private readonly int _maxSteps;
        
        public AgentGameEngine(
            IEnvironment environment, 
            IAgent agent,
            bool renderEnabled = true,
            int maxSteps = 1000) 
            : base(environment)
        {
            _agent = agent;
            _renderEnabled = renderEnabled;
            _maxSteps = maxSteps;
        }
        
        public override void Play()
        {
            // Reset the environment
            var state = Environment.Reset();
            int steps = 0;
            double totalReward = 0;
            
            // Game loop
            while (steps < _maxSteps)
            {
                // Get action from agent
                Domain.Action action = _agent.GetAction(state);
                
                // Step the environment with the action
                var (newState, reward, done) = Environment.Step(action);
                totalReward += reward;
                
                // Apply automatic drop every few steps
                if (steps % 5 == 0 && action != Domain.Action.SoftDrop && action != Domain.Action.HardDrop)
                {
                    var (dropState, dropReward, dropDone) = Environment.Step(Domain.Action.SoftDrop);
                    newState = dropState;
                    reward += dropReward;
                    done = dropDone;
                    totalReward += dropReward;
                }
                
                // Render if requested
                if (_renderEnabled && steps % 5 == 0)
                {
                    Console.Clear();
                    Environment.RenderConsole();
                    Console.WriteLine($"Step: {steps}, Total Reward: {totalReward:F2}");
                    Thread.Sleep(100); // Slow down rendering
                }
                
                // Update the state
                state = newState;
                steps++;
                
                // Check if game is over
                if (done)
                {
                    if (_renderEnabled)
                    {
                        Console.Clear();
                        Environment.RenderConsole();
                        Console.WriteLine($"Game Over! Final Score: {state.Score}, Total Reward: {totalReward:F2}");
                        Thread.Sleep(2000);
                    }
                    break;
                }
            }
        }
    }
}