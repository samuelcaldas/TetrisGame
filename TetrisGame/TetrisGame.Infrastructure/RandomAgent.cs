using System;
using TetrisGame.Domain;

namespace TetrisGame.Infrastructure
{
    /// <summary>
    /// A simple agent that takes random actions
    /// </summary>
    public class RandomAgent : IAgent
    {
        private readonly Random _random;
        
        public RandomAgent(Random random = null)
        {
            _random = random ?? new Random();
        }
        
        public Domain.Action GetAction(State state)
        {
            // Get all possible actions
            var actions = (Domain.Action[])Enum.GetValues(typeof(Domain.Action));
            
            // Filter out "None" action
            var validActions = new Domain.Action[actions.Length - 1];
            int index = 0;
            
            foreach (var action in actions)
            {
                if (action != Domain.Action.None)
                {
                    validActions[index++] = action;
                }
            }
            
            // Return a random valid action
            return validActions[_random.Next(validActions.Length)];
        }
    }
}