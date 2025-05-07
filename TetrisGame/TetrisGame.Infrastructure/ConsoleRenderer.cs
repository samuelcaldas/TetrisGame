using System;
using TetrisGame.Domain;

namespace TetrisGame.Infrastructure
{
    /// <summary>
    /// Console-based renderer for the Tetris game
    /// </summary>
    public class ConsoleRenderer : IRenderer
    {
        public void Render(GameState gameState)
        {
            var board = gameState.GetBoard();
            var currentTetromino = gameState.GetCurrentTetromino();
            var nextTetromino = gameState.GetNextTetromino();
            
            // Save current tetromino positions for rendering
            var currentPositions = currentTetromino.GetAbsolutePositions();
            
            // Draw border and board
            Console.WriteLine("╔══════════════════════╗    ╔═══════╗");
            
            for (int i = 0; i < board.Height; i++)
            {
                Console.Write("║ ");
                
                for (int j = 0; j < board.Width; j++)
                {
                    // Check if position contains current tetromino
                    var position = new Position(i, j);
                    bool isCurrentTetromino = currentPositions.Exists(p => p.Equals(position));
                    
                    if (isCurrentTetromino)
                    {
                        Console.ForegroundColor = currentTetromino.Color;
                        Console.Write("██");
                        Console.ResetColor();
                    }
                    else if (board.GetCell(i, j).IsFilled)
                    {
                        Console.ForegroundColor = board.GetCell(i, j).Color;
                        Console.Write("██");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                
                // Draw next piece preview and game info
                if (i == 1)
                {
                    Console.Write(" ║    ║ NEXT: ║");
                }
                else if (i >= 3 && i <= 6)
                {
                    RenderNextPieceRow(i - 3, nextTetromino);
                }
                else if (i == 8)
                {
                    Console.Write(" ║    ╚═══════╝");
                }
                else if (i == 10)
                {
                    Console.Write(" ║    SCORE: " + gameState.Score.ToString().PadRight(6));
                }
                else if (i == 11)
                {
                    Console.Write(" ║    LEVEL: " + gameState.Level.ToString().PadRight(6));
                }
                else if (i == 12)
                {
                    Console.Write(" ║    LINES: " + gameState.LinesCleared.ToString().PadRight(6));
                }
                else
                {
                    Console.Write(" ║");
                }
                
                Console.WriteLine();
            }
            
            Console.WriteLine("╚══════════════════════╝");
            
            // Draw controls
            Console.WriteLine("Controls:");
            Console.WriteLine("<- -> : Move   ^ : Rotate   ↓ : Soft Drop   Space : Hard Drop   Q : Quit");
        }
        
        private void RenderNextPieceRow(int row, Tetromino nextTetromino)
        {
            var tempTetromino = new Tetromino(nextTetromino.Type, new Position(1, 1));
            var positions = tempTetromino.GetAbsolutePositions();
            
            Console.Write(" ║    ║ ");
            
            for (int col = 0; col < 4; col++)
            {
                bool containsPiece = positions.Exists(p => p.Row == row && p.Column == col);
                
                if (containsPiece)
                {
                    Console.ForegroundColor = nextTetromino.Color;
                    Console.Write("█");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(" ");
                }
            }
            
            Console.Write(" ║");
        }
    }
}