using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisGame.Domain
{
    /// <summary>
    /// Represents the game board
    /// </summary>
    public class Board
    {
        private readonly Cell[][] _cells;
        private readonly int _width;
        private readonly int _height;
        
        public int Width => _width;
        public int Height => _height;

        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            
            // Initialize the board with empty cells
            _cells = new Cell[height][];
            for (int i = 0; i < height; i++)
            {
                _cells[i] = new Cell[width];
                for (int j = 0; j < width; j++)
                {
                    _cells[i][j] = new Cell();
                }
            }
        }

        public bool IsValidPosition(Position position)
        {
            return position.Row >= 0 && position.Row < _height && 
                   position.Column >= 0 && position.Column < _width &&
                   !_cells[position.Row][position.Column].IsFilled;
        }
        
        public bool CanTetrominoFit(Tetromino tetromino)
        {
            foreach (var position in tetromino.GetAbsolutePositions())
            {
                if (!IsValidPosition(position))
                {
                    return false;
                }
            }
            return true;
        }

        public bool PlaceTetromino(Tetromino tetromino)
        {
            if (!CanTetrominoFit(tetromino))
            {
                return false;
            }

            foreach (var position in tetromino.GetAbsolutePositions())
            {
                _cells[position.Row][position.Column].Fill(tetromino.Color);
            }
            
            return true;
        }

        public int ClearLines()
        {
            int linesCleared = 0;
            
            for (int row = _height - 1; row >= 0; row--)
            {
                if (IsLineFull(row))
                {
                    ClearLine(row);
                    ShiftLinesDown(row);
                    row++; // Check the same row again
                    linesCleared++;
                }
            }
            
            return linesCleared;
        }

        private bool IsLineFull(int row)
        {
            for (int col = 0; col < _width; col++)
            {
                if (!_cells[row][col].IsFilled)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearLine(int row)
        {
            for (int col = 0; col < _width; col++)
            {
                _cells[row][col].Clear();
            }
        }

        private void ShiftLinesDown(int clearedRow)
        {
            for (int row = clearedRow - 1; row >= 0; row--)
            {
                for (int col = 0; col < _width; col++)
                {
                    if (_cells[row][col].IsFilled)
                    {
                        var color = _cells[row][col].Color;
                        _cells[row][col].Clear();
                        _cells[row + 1][col].Fill(color);
                    }
                }
            }
        }
        
        public bool IsGameOver()
        {
            // Check if the top row has any filled cells
            for (int col = 0; col < _width; col++)
            {
                if (_cells[0][col].IsFilled)
                {
                    return true;
                }
            }
            return false;
        }
        
        public Cell GetCell(int row, int col)
        {
            if (row < 0 || row >= _height || col < 0 || col >= _width)
            {
                throw new ArgumentOutOfRangeException();
            }
            
            return _cells[row][col];
        }
        
        public int[][] GetBoardRepresentation()
        {
            var representation = new int[_height][];
            
            for (int i = 0; i < _height; i++)
            {
                representation[i] = new int[_width];
                for (int j = 0; j < _width; j++)
                {
                    representation[i][j] = _cells[i][j].IsFilled ? 1 : 0;
                }
            }
            
            return representation;
        }
    }
}