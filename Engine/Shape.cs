namespace Engine
{
    public class Shape
    {
        public enum ShapeTypes
        {
            Unspecified = 0,
            I = 1,
            J = 6,
            L = 7,
            O = 2,
            S = 4,
            T = 3,
            Z = 5
        }

        private int[,] _shapeData;

        public Shape(ShapeTypes type)
        {
            switch (type)
            {
                case ShapeTypes.I:
                    ShapeData = new int[4, 1] { { 1 }, { 1 }, { 1 }, { 1 } };
                    break;
                case ShapeTypes.J:
                    ShapeData = new int[3, 2] { { 0, 6 }, { 0, 6 }, { 6, 6 } };
                    break;
                case ShapeTypes.L:
                    ShapeData = new int[3, 2] { { 7, 0 }, { 7, 0 }, { 7, 7 } };
                    break;
                case ShapeTypes.O:
                    ShapeData = new int[2, 2] { { 2, 2 }, { 2, 2 } };
                    break;
                case ShapeTypes.T:
                    ShapeData = new int[2, 3] { { 3, 3, 3 }, { 0, 3, 0 } };
                    break;
                case ShapeTypes.S:
                    ShapeData = new int[2, 3] { { 0, 4, 4 }, { 4, 4, 0 } };
                    break;
                case ShapeTypes.Z:
                    ShapeData = new int[2, 3] { { 5, 5, 0 }, { 0, 5, 5 } };
                    break;
                default:
                    ShapeData = new int[,] { { } };
                    break;
            }
        }

        public int[,] ShapeData
        {
            get { return _shapeData; }
            internal set
            {
                Height = value.GetLength(0);
                Width = value.GetLength(1);
                _shapeData = value;
            }
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public void RotateAntiClockwise()
        {
            Transpose();
            if (Height > 1)
            {
                ReverseRows();
            }
        }

        public void RotateClockwise()
        {
            Transpose();
            if (Width > 1)
            {
                ReverseColumns();
            }
        }

        private void Transpose()
        {
            int[,] newShape = new int[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    newShape[x, y] = ShapeData[y, x];
                }
            }

            ShapeData = newShape;
        }

        private void ReverseColumns()
        {
            int[,] newShape = new int[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    newShape[y, x] = ShapeData[y, Width - 1 - x];
                }
            }

            ShapeData = newShape;
        }

        private void ReverseRows()
        {
            int[,] newShape = new int[Height, Width];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    newShape[y, x] = ShapeData[Height - 1 - y, x];
                }
            }

            ShapeData = newShape;
        }
    }
}