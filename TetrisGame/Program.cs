using System;
using TetrisGame.Application;
using TetrisGame.Domain;
using TetrisGame.Infrastructure;

namespace TetrisGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Console Tetris";
            Console.CursorVisible = false;
            
            // Ask user if they want human or machine gameplay
            Console.WriteLine("Welcome to Tetris!");
            Console.WriteLine("1. Human player");
            Console.WriteLine("2. AI player (random agent)");
            Console.Write("Choose option (1/2): ");
            
            string choice = Console.ReadLine();
            
            // Setup dependencies
            var renderer = new ConsoleRenderer();
            var environment = new TetrisEnvironment(10, 20, null, renderer);
            
            switch (choice)
            {
                case "1":
                    var inputHandler = new KeyboardInputHandler();
                    var humanGameEngine = new GameEngine(environment, inputHandler);
                    humanGameEngine.PlayHuman();
                    break;
                    
                case "2":
                    var agent = new RandomAgent();
                    var aiGameEngine = new GameEngine(environment, new KeyboardInputHandler());
                    aiGameEngine.PlayAgent(agent, true);
                    break;
                    
                default:
                    Console.WriteLine("Invalid choice. Exiting.");
                    break;
            }
        }
    }
}