using System;
using System.Threading;

using Engine;

namespace ConsoleApp
{
    internal static class Program
    {
        private const int WellHeight = 20;
        private const int WellWidth = 10;
        private const int WaitLoopInterval = 20; // 20=50 times per second
        private const char WallVerticalChar = '║';
        private const char WallHorizontalChar = '═';
        private const char WallBottomLeftChar = '╚';
        private const char WallBottomRightChar = '╝';

        private const ConsoleColor WallColour = ConsoleColor.White;

        private const ConsoleColor Icolour = ConsoleColor.DarkRed;
        private const ConsoleColor Jcolour = ConsoleColor.Gray;
        private const ConsoleColor Lcolour = ConsoleColor.DarkMagenta;
        private const ConsoleColor Ocolour = ConsoleColor.Blue;
        private const ConsoleColor Scolour = ConsoleColor.DarkGreen;
        private const ConsoleColor Tcolour = ConsoleColor.DarkYellow;
        private const ConsoleColor Zcolour = ConsoleColor.DarkCyan;
        private const char ShapeChar = '█';
        private const char WellBackgroundChar = ' ';
        private const ConsoleColor WellBackground = ConsoleColor.Black;

        private static Engine.Engine _engine;
        private static Random _random;
        private static bool _gameOver;

        private static readonly ConsoleColor[] ShapeColour = { ConsoleColor.Black, Icolour, Ocolour, Tcolour, Scolour, Zcolour, Jcolour, Lcolour };
        private static bool updatingWell;

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            _random = new Random();

            _engine = new Engine.Engine(_random, WellWidth, WellHeight);

            _engine.GameOverEvent += GameOverEvent;
            _engine.UpdateShapeEvent += UpdateShapeEvent;
            _engine.UpdateWellEvent += UpdateWellEvent;
            
            _gameOver = false;

            SetupWindow();
            DrawWalls();

            _engine.StartGame();
            do
            {
                CheckForKeyPress();
                Thread.Sleep(WaitLoopInterval);
            }
            while (_gameOver == false);
        }

        private static void UpdateWellEvent(object sender, Engine.Engine.UpdateWellEventArgs args)
        {
            DrawWellContents();
        }

        private static void CheckForKeyPress()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Spacebar:
                        {
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if ((keyInfo.Modifiers & ConsoleModifiers.Shift) != 0)
                            {
                                _engine.PlayerInput(Engine.Engine.PlayerInputType.RotateShapeAntiClockwise);
                            }
                            else
                            {
                                _engine.PlayerInput(Engine.Engine.PlayerInputType.RotateShapeClockwise);
                            }
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            _engine.PlayerInput(Engine.Engine.PlayerInputType.MoveShapeLeft);
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            _engine.PlayerInput(Engine.Engine.PlayerInputType.MoveShapeRight);
                            break;
                        }

                    case ConsoleKey.DownArrow:
                        {
                            _engine.PlayerInput(Engine.Engine.PlayerInputType.DropShape);
                            break;
                        }
                    case ConsoleKey.Escape:
                        {
                            _engine.PlayerInput(Engine.Engine.PlayerInputType.EndGame);
                            break;
                        }
                    default:
                        {
                            // key ignored
                            break;
                        }
                }
            }
        }

        private static void SetupWindow()
        {
            Console.CursorVisible = false;

            Console.Clear();

            Console.SetWindowSize(WellWidth + 14, WellHeight + 4);
        }

        private static void DrawWalls()
        {
            Console.ForegroundColor = WallColour;

            for (int i = 0; i < WellHeight - 1; i++)
            {
                Console.SetCursorPosition(1, i + 2);
                Console.Write(WallVerticalChar);
                Console.SetCursorPosition(WellWidth + 2, i + 2);
                Console.Write(WallVerticalChar);
            }

            Console.SetCursorPosition(1, WellHeight + 1);
            Console.Write(WallBottomLeftChar + new string(WallHorizontalChar, WellWidth) + WallBottomRightChar);
        }

        private static void UpdateShapeEvent(object sender, Engine.Engine.UpdateShapeEventArgs args)
        {
            if (updatingWell == false)
            {
                updatingWell = true;

                //DrawWellContents();

                DrawCurrentShape();

                updatingWell = false;
            }
        }

        private static void DrawCurrentShape()
        {
            Shape currentShape = _engine.CurrentShape;
            Shape previousShapeOrientation = _engine.CurrentShapePreviousOrientation;

            int currentShapeX = _engine.CurrentShapeX;
            int currentShapeY = _engine.CurrentShapeY;

            int previousShapeX = _engine.CurrentShapePreviousX;
            int previousShapeY = _engine.CurrentShapePreviousY;

            const int WellOriginX = 2;
            const int WellOriginY = 1;

            DrawShape(WellOriginX, WellOriginY, currentShape, currentShapeX, currentShapeY, previousShapeOrientation, previousShapeX, previousShapeY);
        }

        private static void DrawShape(int offsetX, int offsetY, Shape shape, int shapeX, int shapeY, Shape previousShapeOrientation, int previousShapeX, int previousShapeY)
        {
            // erase previous shape
            for (int y = 0; y <= previousShapeOrientation.Height - 1; y++)
            {
                for (int x = 0; x <= previousShapeOrientation.Width - 1; x++)
                {
                    int cell = previousShapeOrientation.ShapeData[y, x];

                    if (cell != 0)
                    {
                        int cellx = previousShapeX + x;
                        int cellY = previousShapeY - (previousShapeOrientation.Height - 1 - y);

                        if (cellY >= 0)
                        {
                            Console.SetCursorPosition(cellx + offsetX, cellY + offsetY);
                            Console.ForegroundColor = WellBackground;

                            Console.Write(WellBackgroundChar);
                        }
                    }
                }
            }

            // draw shape
            for (int y = 0; y <= shape.Height - 1; y++)
            {
                for (int x = 0; x <= shape.Width - 1; x++)
                {
                    int cell = shape.ShapeData[y, x];

                    if (cell != 0)
                    {
                        int cellx = shapeX + x;
                        int cellY = shapeY - (shape.Height - 1 - y);

                        if (cellY >= 0)
                        {
                            Console.SetCursorPosition(cellx + offsetX, cellY + offsetY);
                            Console.ForegroundColor = ShapeColour[cell];

                            Console.Write(ShapeChar);
                        }
                    }
                }
            }
        }

        private static void DrawWellContents()
        {
            int[,] well = _engine.Well;

            for (int y = 0; y < WellHeight; y++)
            {
                for (int x = 0; x < WellWidth; x++)
                {
                    Console.SetCursorPosition(x + 2, y + 1);

                    int cell = well[y, x];

                    Console.ForegroundColor = ShapeColour[cell];

                    Console.Write(cell == 0 ? WellBackgroundChar : ShapeChar);
                }
            }
        }

        private static void GameOverEvent(object sender)
        {
            _gameOver = true;
        }
    }
}