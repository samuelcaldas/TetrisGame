using System;
using TetrisGame.Domain;
using TetrisGame.Infrastructure;

namespace TetrisGame.Application
{
    /// <summary>
    /// Implementation of the Tetris game environment for RL
    /// </summary>
    public class TetrisEnvironment : IEnvironment
    {
        private GameState _gameState;
        private readonly Random _random;
        private readonly int _boardWidth;
        private readonly int _boardHeight;
        private readonly IRenderer _renderer;
        
        public TetrisEnvironment(int boardWidth = 10, int boardHeight = 20, Random random = null, IRenderer renderer = null)
        {
            _boardWidth = boardWidth;
            _boardHeight = boardHeight;
            _random = random ?? new Random();
            _renderer = renderer;
            
            Reset();
        }
        
        public State Reset()
        {
            _gameState = new GameState(_boardWidth, _boardHeight, _random);
            return _gameState.GetCurrentState();
        }

        public (State NewState, double Reward, bool Done) Step(Domain.Action action)
        {
            // Store old state for reward calculation
            var oldState = _gameState.GetCurrentState();
            
            // Apply the action
            ApplyAction(action);
            
            // Get the new state
            var newState = _gameState.GetCurrentState();
            
            // Calculate reward
            double reward = CalculateReward(oldState, newState);
            
            return (newState, reward, _gameState.IsGameOver);
        }
        
        private void ApplyAction(Domain.Action action)
        {
            switch (action)
            {
                case Domain.Action.MoveLeft:
                    _gameState.MoveCurrentTetromino(Direction.Left);
                    break;
                case Domain.Action.MoveRight:
                    _gameState.MoveCurrentTetromino(Direction.Right);
                    break;
                case Domain.Action.RotateClockwise:
                    _gameState.RotateCurrentTetromino();
                    break;
                case Domain.Action.SoftDrop:
                    _gameState.MoveCurrentTetromino(Direction.Down);
                    break;
                case Domain.Action.HardDrop:
                    _gameState.HardDrop();
                    break;
                default:
                    // Do nothing for None action
                    break;
            }
        }
        
        private double CalculateReward(State oldState, State newState)
        {
            double reward = 0;
            
            // Reward for clearing lines (increasing with more lines)
            int linesDifference = newState.LinesCleared - oldState.LinesCleared;
            if (linesDifference > 0)
            {
                double[] lineRewards = { 0, 1.0, 3.0, 5.0, 8.0 }; // Rewards for 0, 1, 2, 3, 4 lines
                reward += lineRewards[linesDifference];
            }
            
            // Reward for scoring points
            int scoreDifference = newState.Score - oldState.Score;
            if (scoreDifference > 0)
            {
                reward += scoreDifference / 100.0; // Normalize score to reasonable reward value
            }
            
            // Penalty for game over
            if (_gameState.IsGameOver)
            {
                reward -= 10.0;
            }
            
            return reward;
        }
        
        public void RenderConsole()
        {
            _renderer?.Render(_gameState);
        }
    }
}