namespace TetrisGame.Domain
{
    /// <summary>
    /// Represents the complete game state for RL environment
    /// </summary>
    public class State
    {
        public int[][] BoardRepresentation { get; }
        public TetrominoType CurrentTetrominoType { get; }
        public Position CurrentTetrominoPosition { get; }
        public TetrominoType NextTetrominoType { get; }
        public int Score { get; }
        public int Level { get; }
        public int LinesCleared { get; }

        public State(
            int[][] boardRepresentation, 
            TetrominoType currentTetrominoType,
            Position currentTetrominoPosition,
            TetrominoType nextTetrominoType,
            int score,
            int level,
            int linesCleared)
        {
            BoardRepresentation = boardRepresentation;
            CurrentTetrominoType = currentTetrominoType;
            CurrentTetrominoPosition = currentTetrominoPosition;
            NextTetrominoType = nextTetrominoType;
            Score = score;
            Level = level;
            LinesCleared = linesCleared;
        }
    }
}