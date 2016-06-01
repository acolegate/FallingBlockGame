using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Engine.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EngineTests
    {
        private const int TestWellWidth = 10;
        private const int TestWellHeight = 20;
        private const int TestSeed = 0;

        private Random _testRandom;

        [TestInitialize]
        public void Initialise()
        {
            _testRandom = new Random(TestSeed);
        }

        [TestMethod]
        public void Engine_Constructor_CreatedInstanceOfEngine_ReturnsInstance()
        {
            // Arrange/Act
            Engine actual = new Engine(_testRandom, TestWellWidth, TestWellHeight);

            int[,] emptyWell = { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };

            // Assert
            Assert.IsNotNull(actual, "Engine not instantiated");
            Assert.IsNotNull(actual.RandomInstance, "random not instantiated");
            Assert.AreEqual(TestWellWidth, actual.WellWidth, "Unexpected width returned");
            Assert.AreEqual(TestWellHeight, actual.WellHeight, "Unexpected height returned");
            CollectionAssert.AreEqual(emptyWell, actual.Well, "Unexpected well returned");
            Assert.IsNotNull(actual.TimerInstance, "Timer not instantiated");

            Assert.IsNull(actual.CurrentShape, "Unexpected CurrentShape");
            Assert.AreEqual(Engine.StartingMovementInterval, actual.CurrentMovementInterval, "Unexpected CurrentMovementInterval");
            Assert.AreEqual(0, actual.Score, "Unexpected score");
        }

        [TestMethod]
        public void Engine_ChooseRandomShape_ChoosesNextShape_ReturnsShape()
        {
            // Arrange
            Engine classUnderTest = new Engine(_testRandom, TestWellWidth, TestWellHeight);

            int[,] expectedShapeData = new int[3, 2] { { 0, 5 }, { 5, 5 }, { 5, 0 } };

            // Act/Assert
            Shape actual = classUnderTest.RandomShape();

            CollectionAssert.AreEqual(expectedShapeData, actual.ShapeData, "Unexpected Shape returned");
        }

        [TestMethod]
        public void Engine_ShapeWillFit_TestWhetherShapeFitsInWell_ReturnsTrueOrFalseAccordingly()
        {
            // Drop a shape from the top to the bottom of a 6x6 well
            // test all posible positions x and y

            // Arrange
            Engine classUnderTest = new Engine(_testRandom, 6, 6);

            // Act / Assert
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, -1));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, -1));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, -1));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, -1));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, -1));

            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, 0));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, 0));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, 0));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, 0));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, 0));

            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, 1));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, 1));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, 1));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, 1));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, 1));

            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, 2));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, 2));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, 2));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, 2));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, 2));

            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, 3));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, 3));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, 3));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, 3));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, 3));

            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, 4));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, 4));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, 4));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, 4));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, 4));

            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, 5));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, 5));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, 5));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, 5));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, 5));

            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 0, 6));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 1, 6));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 2, 6));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 3, 6));
            Assert.IsFalse(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.Z), 4, 6));
        }

        [TestMethod]
        public void Engine_ShapeWillFit_TestWhetherShapeFitsAroundOtherShapes_ReturnsTrueOrFalseAccordingly()
        {
            // Drop a shape from the top to the bottom of a 6x6 well
            // test all posible positions x and y

            // Arrange
            Engine classUnderTest = new Engine(_testRandom, 6, 6)
                                        {
                                            Well = new[,] { { 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0 },
                                                            { 1, 0, 0, 0, 0, 0 },
                                                            { 1, 0, 3, 3, 0, 0 },
                                                            { 1, 2, 2, 3, 0, 0 },
                                                            { 1, 2, 2, 3, 0, 0 }
                                            }
                                        };

            // Act / Assert
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.I), 4, 5));
            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.I), 1, 3));

            Assert.IsTrue(classUnderTest.ShapeWillFit(new Shape(Shape.ShapeTypes.T), 0, 2));

            // rotate T shape
            Shape rotatedT = new Shape(Shape.ShapeTypes.T);
            rotatedT.RotateAntiClockwise();
            Assert.IsTrue(classUnderTest.ShapeWillFit(rotatedT, 1, 3));
        }

        [TestMethod]
        public void Engine_ApplyShapeToWell_TopLeft_ShapeValuesAreAppliedToWell_ShapeIsInWell()
        {
            // Arrange
            Engine classUnderTest = new Engine(_testRandom, 6, 6);
            int[,] expected = { { 5, 5, 0, 0, 0, 0 }, { 0, 5, 5, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };

            // Act
            classUnderTest.ApplyShapeToWell(new Shape(Shape.ShapeTypes.Z), 0, 1);

            // Assert
            CollectionAssert.AreEqual(expected, classUnderTest.Well, "Unexpected well");
        }

        [TestMethod]
        public void Engine_ApplyShapeToWell_TopRight_ShapeValuesAreAppliedToWell_ShapeIsInWell()
        {
            // Arrange
            Engine classUnderTest = new Engine(_testRandom, 6, 6);
            int[,] expected = { { 0, 0, 0, 5, 5, 0 }, { 0, 0, 0, 0, 5, 5 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };

            // Act
            classUnderTest.ApplyShapeToWell(new Shape(Shape.ShapeTypes.Z), 3, 1);

            // Assert
            CollectionAssert.AreEqual(expected, classUnderTest.Well, "Unexpected well");
        }

        [TestMethod]
        public void Engine_ApplyShapeToWell_BottomLeft_ShapeValuesAreAppliedToWell_ShapeIsInWell()
        {
            // Arrange
            Engine classUnderTest = new Engine(_testRandom, 6, 6);
            int[,] expected = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 5, 5, 0, 0, 0, 0 }, { 0, 5, 5, 0, 0, 0 } };

            // Act
            classUnderTest.ApplyShapeToWell(new Shape(Shape.ShapeTypes.Z), 0, 5);

            // Assert
            CollectionAssert.AreEqual(expected, classUnderTest.Well, "Unexpected well");
        }

        [TestMethod]
        public void Engine_ApplyShapeToWell_BottomRight_ShapeValuesAreAppliedToWell_ShapeIsInWell()
        {
            // Arrange
            Engine classUnderTest = new Engine(_testRandom, 6, 6);
            int[,] expected = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 5, 5, 0 }, { 0, 0, 0, 0, 5, 5 } };

            // Act
            classUnderTest.ApplyShapeToWell(new Shape(Shape.ShapeTypes.Z), 3, 5);

            // Assert
            CollectionAssert.AreEqual(expected, classUnderTest.Well, "Unexpected well");
        }

        [TestMethod]
        public void Engine_MakeEmptyWell_CreatesWellOfCorrectiDimensionsFilledWithZeros_ReturnsWell()
        {
            // arrange
            int[,] expected = { { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0 } };

            // Act
            int[,] actual = Engine.MakeEmptyWell(6, 6);

            // Assert
            CollectionAssert.AreEqual(expected, actual, "Unexpected well returned");
        }
    }
}