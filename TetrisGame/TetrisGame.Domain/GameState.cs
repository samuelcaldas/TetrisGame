using System;

namespace TetrisGame.Domain
{
    /// <summary>
    /// Maintains the complete state of the game
    /// </summary>
    public class GameState
    {
        private readonly Board _board;
        private Tetromino _currentTetromino;
        private Tetromino _nextTetromino;
        private int _score;
        private int _level;
        private int _linesCleared;
        private bool _isGameOver;
        private Random _random;

        public bool IsGameOver => _isGameOver;
        public int Score => _score;
        public int Level => _level;
        public int LinesCleared => _linesCleared;
        
        public GameState(int boardWidth, int boardHeight, Random random = null)
        {
            _board = new Board(boardWidth, boardHeight);
            _random = random ?? new Random();
            _score = 0;
            _level = 1;
            _linesCleared = 0;
            _isGameOver = false;
            
            // Create initial tetrominos
            _currentTetromino = CreateRandomTetromino();
            _nextTetromino = CreateRandomTetromino();
        }
        
        private Tetromino CreateRandomTetromino()
        {
            var values = Enum.GetValues(typeof(TetrominoType));
            var type = (TetrominoType)values.GetValue(_random.Next(values.Length));
            
            // Start at the top-middle of the board
            var initialPosition = new Position(0, _board.Width / 2);
            return new Tetromino(type, initialPosition);
        }

        public bool MoveCurrentTetromino(Direction direction)
        {
            if (_isGameOver)
            {
                return false;
            }

            var testTetromino = _currentTetromino.GetCopy();
            testTetromino.Move(direction);
            
            if (_board.CanTetrominoFit(testTetromino))
            {
                _currentTetromino.Move(direction);
                return true;
            }
            
            // Handle landing when moving down
            if (direction == Direction.Down)
            {
                LockCurrentTetromino();
            }
            
            return false;
        }
        
        public bool RotateCurrentTetromino()
        {
            if (_isGameOver)
            {
                return false;
            }
            
            var testTetromino = _currentTetromino.GetCopy();
            testTetromino.Rotate();
            
            if (_board.CanTetrominoFit(testTetromino))
            {
                _currentTetromino.Rotate();
                return true;
            }
            
            return false;
        }
        
        public bool HardDrop()
        {
            if (_isGameOver)
            {
                return false;
            }
            
            while (MoveCurrentTetromino(Direction.Down))
            {
                // Continue moving down until it can't anymore
            }
            
            // Locking will happen in MoveCurrentTetromino
            return true;
        }

        private void LockCurrentTetromino()
        {
            _board.PlaceTetromino(_currentTetromino);
            int linesCleared = _board.ClearLines();
            UpdateScore(linesCleared);
            
            // Check if game is over
            if (_board.IsGameOver())
            {
                _isGameOver = true;
                return;
            }
            
            // Get the next tetromino ready
            _currentTetromino = _nextTetromino;
            _nextTetromino = CreateRandomTetromino();
        }
        
        private void UpdateScore(int linesCleared)
        {
            if (linesCleared > 0)
            {
                // Classic Tetris scoring
                int[] points = { 0, 40, 100, 300, 1200 };
                _score += points[linesCleared] * _level;
                _linesCleared += linesCleared;
                
                // Level up every 10 lines
                _level = (_linesCleared / 10) + 1;
            }
        }
        
        public State GetCurrentState()
        {
            return new State(
                _board.GetBoardRepresentation(),
                _currentTetromino.Type,
                _currentTetromino.GetOrigin(),
                _nextTetromino.Type,
                _score,
                _level,
                _linesCleared
            );
        }
        
        public Board GetBoard()
        {
            return _board;
        }
        
        public Tetromino GetCurrentTetromino()
        {
            return _currentTetromino;
        }
        
        public Tetromino GetNextTetromino()
        {
            return _nextTetromino;
        }
    }
}