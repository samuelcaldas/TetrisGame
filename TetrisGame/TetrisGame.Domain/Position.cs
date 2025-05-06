namespace TetrisGame.Domain
{
    /// <summary>
    /// Represents a position on the game board
    /// </summary>
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Position Translate(int rowOffset, int columnOffset)
        {
            return new Position(Row + rowOffset, Column + columnOffset);
        }

        public override bool Equals(object obj)
        {
            if (obj is Position other)
            {
                return Row == other.Row && Column == other.Column;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (Row * 397) ^ Column;
        }
    }
}