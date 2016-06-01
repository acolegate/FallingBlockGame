using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ShapeTests
    {
        private const int Id = 999;

        [TestMethod]
        public void Shape_Constructor_IsConstructedProperly()
        {
            // Act

            Shape shape = new Shape(Shape.ShapeTypes.I);

            // Assert
            Assert.AreEqual(1, shape.Width);
            Assert.AreEqual(4, shape.Height);
        }

        [TestMethod]
        public void Shape_Rotate2X3Clockwise_RotatesProperly()
        {
            // Arrange

            int[,] expected = new int[2, 3] { { 6, 0, 0 }, { 6, 6, 6 } };
            Shape shape = new Shape(Shape.ShapeTypes.J);

            // Act
            shape.RotateClockwise();

            // Assert
            CollectionAssert.AreEqual(expected, shape.ShapeData);
        }

        [TestMethod]
        public void Shape_Rotate2X3AntiClockwise_RotatesProperly()
        {
            // Arrange
            int[,] expected = new int[2, 3] { { 0, 0, 7 }, { 7, 7, 7 } };

            Shape shape = new Shape(Shape.ShapeTypes.L);

            // Act
            shape.RotateAntiClockwise();

            // Assert
            CollectionAssert.AreEqual(expected, shape.ShapeData);
        }

        [TestMethod]
        public void Shape_Rotate2X2Clockwise_RotatesProperly()
        {
            // Arrange
            int[,] expected = new int[2, 2] { { 2, 2 }, { 2, 2 } };

            Shape shape = new Shape(Shape.ShapeTypes.O);

            // Act
            shape.RotateClockwise();

            // Assert
            CollectionAssert.AreEqual(expected, shape.ShapeData);
        }

        [TestMethod]
        public void Shape_Rotate2X2AntiClockwise_RotatesProperly()
        {
            // Arrange
            int[,] expected = new int[2, 2] { { 2, 2 }, { 2, 2 } };

            Shape shape = new Shape(Shape.ShapeTypes.O);

            // Act
            shape.RotateAntiClockwise();

            // Assert
            CollectionAssert.AreEqual(expected, shape.ShapeData);
        }

        [TestMethod]
        public void Shape_Rotate4X1Clockwise_RotatesProperly()
        {
            // Arrange
            int[,] expected = new int[1, 4] { { 1, 1, 1, 1 } };

            Shape shape = new Shape(Shape.ShapeTypes.I);

            // Act
            shape.RotateClockwise();

            // Assert
            CollectionAssert.AreEqual(expected, shape.ShapeData);
        }

        [TestMethod]
        public void Shape_Rotate4X1AntiClockwise_RotatesProperly()
        {
            // Arrange

            int[,] expected = new int[1, 4] { { 1, 1, 1, 1 } };

            Shape shape = new Shape(Shape.ShapeTypes.I);

            // Act
            shape.RotateAntiClockwise();

            // Assert
            CollectionAssert.AreEqual(expected, shape.ShapeData);
        }
    }
}