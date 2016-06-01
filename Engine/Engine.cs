using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Timers;

using Timer = System.Timers.Timer;

namespace Engine
{
    public class Engine
    {
        public delegate void GameOverEventHandler(object sender);

        public delegate void UpdateShapeEventHandler(object sender, UpdateShapeEventArgs args);

        public delegate void UpdateWellEventHandler(object sender, UpdateWellEventArgs args);

        public enum PlayerInputType
        {
            MoveShapeLeft,
            MoveShapeRight,
            DropShape,
            EndGame,
            RotateShapeAntiClockwise,
            RotateShapeClockwise
        }

        internal const int StartingMovementInterval = 500;
        private const int DropSpeedInterval = 10;

        internal readonly int CurrentMovementInterval;

        internal readonly Random RandomInstance;

        internal readonly Timer TimerInstance;

        internal readonly int WellHeight;

        internal readonly int WellWidth;

        public Shape CurrentShape;
        public Shape CurrentShapePreviousOrientation;
        public int CurrentShapePreviousX;
        public int CurrentShapePreviousY;

        public int CurrentShapeX;
        public int CurrentShapeY;

        public int Level;

        public Shape NextShape;

        internal int Score;

        public int[,] Well;

        public Engine(Random randomInstance, int wellWidth, int wellHeight)
        {
            RandomInstance = randomInstance;
            WellWidth = wellWidth;
            WellHeight = wellHeight;

            TimerInstance = new Timer();

            Well = MakeEmptyWell(WellWidth, WellHeight);

            CurrentShape = null;
            CurrentShapePreviousOrientation = new Shape(Shape.ShapeTypes.Unspecified);

            CurrentMovementInterval = StartingMovementInterval;
            Score = 0;
        }

        public event GameOverEventHandler GameOverEvent;

        public event UpdateShapeEventHandler UpdateShapeEvent;
        public event UpdateWellEventHandler UpdateWellEvent;

        private void RaiseUpdateDisplayEvent()
        {
            UpdateShapeEvent?.Invoke(this, new UpdateShapeEventArgs
                                               {
                                                   Level = Level,
                                                   Score = Score
                                               });
        }

        private void RaiseUpdateWellEvent()
        {
            UpdateWellEvent?.Invoke(this, new UpdateWellEventArgs
                                              {
                                                  Level = Level,
                                                  Score = Score
                                              });
        }

        private void RaiseGameOverEvent()
        {
            GameOverEvent?.Invoke(this);
        }

        internal static int[,] MakeEmptyWell(int wellWidth, int wellHeight)
        {
            int[,] newWell = new int[wellHeight, wellWidth];

            for (int y = 0; y < wellHeight; y++)
            {
                for (int x = 0; x < wellWidth; x++)
                {
                    newWell[y, x] = 0;
                }
            }

            return newWell;
        }

        internal Shape RandomShape()
        {
            Shape shape = new Shape((Shape.ShapeTypes)RandomInstance.Next(1, 7));

            int rotations = RandomInstance.Next(0, 4);
            for (int i = 0; i < rotations; i++)
            {
                shape.RotateClockwise();
            }

            return shape;
        }

        internal bool ShapeWillFit(Shape shape, int x, int y)
        {
            int shapeHeight = shape.Height;
            int shapeWidth = shape.Width;

            for (int shapeY = shapeHeight - 1; shapeY >= 0; shapeY--)
            {
                for (int shapeX = 0; shapeX <= shapeWidth - 1; shapeX++)
                {
                    if (shape.ShapeData[shapeY, shapeX] != 0)
                    {
                        // this square in the shape is occupied
                        int wellX = shapeX + x;
                        int wellY = y - (shapeHeight - 1 - shapeY);

                        if (wellX < 0 || wellX > WellWidth - 1 || wellY > WellHeight - 1)
                        {
                            // a square is outside the bounds of the well
                            return false;
                        }

                        if (wellY >= 0 && Well[wellY, wellX] != 0)
                        {
                            // the shape's square would occupy an already occupied square in the well
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        internal void ApplyShapeToWell(Shape shape, int x, int y)
        {
            // it is assumed the shape will fit in the proposed position

            int shapeValue;

            // iterate through each square in the shape
            for (int shapeY = shape.Height - 1; shapeY >= 0; shapeY--)
            {
                for (int shapeX = 0; shapeX <= shape.Width - 1; shapeX++)
                {
                    shapeValue = shape.ShapeData[shapeY, shapeX];

                    if (shapeValue != 0)
                    {
                        // this square in the shape is occupied
                        // apply the value to the well
                        int wellY = y - (shape.Height - 1 - shapeY);
                        int wellX = shapeX + x;

                        // not sure why this has to be expanded. Code coverage doesn't cover all paths otherwise.
                        if (wellX >= 0)
                        {
                            if (wellX <= WellWidth - 1)
                            {
                                if (wellY >= 0)
                                {
                                    if (wellY <= WellHeight - 1)
                                    {
                                        Well[wellY, wellX] = shapeValue;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void StartGame()
        {
            Level = 1;
            Score = 0;

            NextShape = RandomShape();

            PositionNextShape();

            TimerInstance.Interval = StartingMovementInterval;
            TimerInstance.Elapsed += TimerInstance_Elapsed;
            TimerInstance.AutoReset = false;
            TimerInstance.Start();
        }

        private void PositionNextShape()
        {
            CurrentShape = NextShape;
            CurrentShapeX = (WellWidth - CurrentShape.Width) / 2;
            CurrentShapeY = -1;

            // copy the new shape
            CurrentShapePreviousOrientation.ShapeData = CurrentShape.ShapeData;
            CurrentShapePreviousX = CurrentShapeX;
            CurrentShapePreviousY = CurrentShapeY;

            NextShape = RandomShape();
        }

        private void TimerInstance_Elapsed(object sender, ElapsedEventArgs e)
        {
            MoveCurrentShapeDown();

            TimerInstance.Start();
        }

        private void MoveCurrentShapeDown()
        {
            if (ShapeWillFit(CurrentShape, CurrentShapeX, CurrentShapeY + 1))
            {
                CurrentShapePreviousOrientation.ShapeData = CurrentShape.ShapeData;
                CurrentShapePreviousX = CurrentShapeX;
                CurrentShapePreviousY = CurrentShapeY;

                CurrentShapeY++;
            }
            else
            {
                // shape has stopped
                ProcessStoppedShape();
            }

            RaiseUpdateDisplayEvent();
        }

        private void ProcessStoppedShape()
        {
            ApplyShapeToWell(CurrentShape, CurrentShapeX, CurrentShapeY);

            CheckForCompleteLines();

            PositionNextShape();
        }

        private void CheckForCompleteLines()
        {
            bool rowsComplete = false;

            int y = WellHeight - 1;
            while (y >= 0)
            {
                bool rowComplete = true;

                for (int x = 0; x < WellWidth; x++)
                {
                    if (Well[y, x] == 0)
                    {
                        rowComplete = false;
                        break;
                    }
                }

                if (rowComplete)
                {
                    ProcessCompleteRow(y);
                    rowsComplete = true;
                }
                else
                {
                    y--;
                }
            }

            if (rowsComplete)
            {
                RaiseUpdateWellEvent();
            }
        }

        private void ProcessCompleteRow(int completeRow)
        {
            for (int y = completeRow; y >= 0; y--)
            {
                for (int x = 0; x < WellWidth; x++)
                {
                    if (y == 0)
                    {
                        // blank the top row in the well
                        Well[0, x] = 0;
                    }
                    else
                    {
                        Well[y, x] = Well[y - 1, x];
                    }
                }
            }
        }

        public void PlayerInput(PlayerInputType playerInput)
        {
            switch (playerInput)
            {
                case PlayerInputType.MoveShapeLeft:
                    {
                        if (ShapeWillFit(CurrentShape, CurrentShapeX - 1, CurrentShapeY))
                        {
                            CurrentShapePreviousOrientation.ShapeData = CurrentShape.ShapeData;
                            CurrentShapePreviousX = CurrentShapeX;
                            CurrentShapePreviousY = CurrentShapeY;
                            CurrentShapeX--;

                            RaiseUpdateDisplayEvent();
                        }
                        break;
                    }
                case PlayerInputType.MoveShapeRight:
                    {
                        if (ShapeWillFit(CurrentShape, CurrentShapeX + 1, CurrentShapeY))
                        {
                            CurrentShapePreviousOrientation.ShapeData = CurrentShape.ShapeData;
                            CurrentShapePreviousX = CurrentShapeX;
                            CurrentShapePreviousY = CurrentShapeY;
                            CurrentShapeX++;

                            RaiseUpdateDisplayEvent();
                        }
                        break;
                    }
                case PlayerInputType.DropShape:
                    {
                        DropShape();
                        break;
                    }
                case PlayerInputType.EndGame:
                    {
                        RaiseGameOverEvent();
                        break;
                    }
                case PlayerInputType.RotateShapeClockwise:
                    {
                        Shape rotatedShape = new Shape(Shape.ShapeTypes.Unspecified)
                                                 {
                                                     ShapeData = CurrentShape.ShapeData
                                                 };

                        rotatedShape.RotateClockwise();
                        if (ShapeWillFit(rotatedShape, CurrentShapeX, CurrentShapeY))
                        {
                            CurrentShapePreviousOrientation.ShapeData = CurrentShape.ShapeData;
                            CurrentShapePreviousX = CurrentShapeX;
                            CurrentShapePreviousY = CurrentShapeY;

                            CurrentShape.ShapeData = rotatedShape.ShapeData;

                            RaiseUpdateDisplayEvent();
                        }
                        break;
                    }
                case PlayerInputType.RotateShapeAntiClockwise:
                    {
                        Shape rotatedShape = CurrentShape;
                        rotatedShape.RotateAntiClockwise();
                        if (ShapeWillFit(rotatedShape, CurrentShapeX, CurrentShapeY))
                        {
                            CurrentShapePreviousOrientation.ShapeData = CurrentShape.ShapeData;
                            CurrentShapePreviousX = CurrentShapeX;
                            CurrentShapePreviousY = CurrentShapeY;

                            CurrentShape.ShapeData = rotatedShape.ShapeData;

                            RaiseUpdateDisplayEvent();
                        }
                        break;
                    }
            }
        }

        private void DropShape()
        {
            CurrentShapePreviousOrientation.ShapeData = CurrentShape.ShapeData;
            CurrentShapePreviousX = CurrentShapeX;
            CurrentShapePreviousY = CurrentShapeY;
            RaiseUpdateDisplayEvent();

            while (ShapeWillFit(CurrentShape, CurrentShapeX, CurrentShapeY + 1))
            {                
                CurrentShapePreviousX = CurrentShapeX;
                CurrentShapePreviousY = CurrentShapeY;

                CurrentShapeY++;

                RaiseUpdateDisplayEvent();

                Thread.Sleep(DropSpeedInterval);
            }

            ProcessStoppedShape();
        }

        [ExcludeFromCodeCoverage]
        public class UpdateShapeEventArgs
        {
            public int Score { get; set; }
            public int Level { get; set; }
        }

        [ExcludeFromCodeCoverage]
        public class UpdateWellEventArgs
        {
            public int Score { get; set; }
            public int Level { get; set; }
        }
    }
}