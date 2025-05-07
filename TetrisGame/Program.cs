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
            GameEngine gameEngine;
            
            switch (choice)
            {
                case "1":
                    var inputHandler = new KeyboardInputHandler();
                    gameEngine = new HumanGameEngine(environment, inputHandler);
                    break;
                    
                case "2":
                    var agent = new RandomAgent();
                    gameEngine = new AgentGameEngine(environment, agent, true);
                    break;
                    
                default:
                    Console.WriteLine("Invalid choice. Exiting.");
                    return;
            }
            
            gameEngine.Play();
        }
    }
}