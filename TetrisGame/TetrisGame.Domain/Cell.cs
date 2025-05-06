using System;

namespace TetrisGame.Domain
{
    /// <summary>
    /// Represents a single cell on the game board
    /// </summary>
    public class Cell
    {
        public bool IsFilled { get; private set; }
        public ConsoleColor Color { get; private set; }

        public Cell()
        {
            IsFilled = false;
            Color = ConsoleColor.Black;
        }

        public void Fill(ConsoleColor color)
        {
            IsFilled = true;
            Color = color;
        }

        public void Clear()
        {
            IsFilled = false;
            Color = ConsoleColor.Black;
        }
    }
}