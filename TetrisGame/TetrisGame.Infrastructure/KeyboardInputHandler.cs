using System;
using TetrisGame.Domain;

namespace TetrisGame.Infrastructure
{
    /// <summary>
    /// Handles keyboard input for the game
    /// </summary>
    public class KeyboardInputHandler : IInputHandler
    {
        public Domain.Action GetAction()
        {
            if (!Console.KeyAvailable)
            {
                return Domain.Action.None;
            }
            
            var key = Console.ReadKey(true);
            
            switch (key.Key)
            {
                case ConsoleKey.LeftArrow:
                    return Domain.Action.MoveLeft;
                case ConsoleKey.RightArrow:
                    return Domain.Action.MoveRight;
                case ConsoleKey.UpArrow:
                    return Domain.Action.RotateClockwise;
                case ConsoleKey.DownArrow:
                    return Domain.Action.SoftDrop;
                case ConsoleKey.Spacebar:
                    return Domain.Action.HardDrop;
                case ConsoleKey.Q:
                    Environment.Exit(0);
                    break;
            }
            
            return Domain.Action.None;
        }
    }
}