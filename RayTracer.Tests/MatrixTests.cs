using System;
using Xunit;

namespace RayTracer.Tests
{
    public class MatrixTests
    {
        [Fact]
        public void Constructing4x4Matrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {1, 2, 3, 4}, {5.5, 6.5, 7.5, 8.5}, {9, 10, 11, 12}, {13.5, 14.5, 15.5, 16.5} });
            Assert.True(matrix[0, 0] == 1);
            Assert.True(matrix[0, 3] == 4);
            Assert.True(matrix[1, 0] == 5.5);
            Assert.True(matrix[1, 2] == 7.5);
            Assert.True(matrix[2, 2] == 11);
            Assert.True(matrix[3, 0] == 13.5);
            Assert.True(matrix[3, 2] == 15.5);
        }

        [Fact]
        public void Constructing2x2Matrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {-3, 5}, {1, -2} });
            Assert.True(matrix[0, 0] == -3);
            Assert.True(matrix[0, 1] == 5);
            Assert.True(matrix[1, 0] == 1);
            Assert.True(matrix[1, 1] == -2);
        }

        [Fact]
        public void Constructing3x3Matrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {-3, 5, 0}, {1, -2, -7}, {0, 1, 1} });
            Assert.True(matrix[0, 0] == -3);
            Assert.True(matrix[1, 1] == -2);
            Assert.True(matrix[2, 2] == 1);
        }

        [Fact]
        public void ComparingTwoSameMatrices_ShouldBeEqual()
        {
            var matrix1 = new Matrix(new double[,] { {1, 2, 3, 4}, {5, 6, 7, 8}, {9, 8, 7, 6}, {5, 4, 3, 2} });
            var matrix2 = new Matrix(new double[,] { {1, 2, 3, 4}, {5, 6, 7, 8}, {9, 8, 7, 6}, {5, 4, 3, 2} });
            Assert.True(matrix1.Equals(matrix2));
        }

        [Fact]
        public void ComparingTwoDifferentMatrices_ShouldNotEqual()
        {
            var matrix1 = new Matrix(new double[,] { {1, 2, 3, 4}, {5, 6, 7, 8}, {9, 8, 7, 6}, {5, 4, 3, 2} });
            var matrix2 = new Matrix(new double[,] { {2, 3, 4, 5}, {6, 7, 8, 9}, {8, 7, 6, 5}, {4, 3, 2, 1} });
            Assert.False(matrix1.Equals(matrix2));
        }

        [Fact]
        public void MultiplyingTwoMatrices_ShouldWork()
        {
            var matrix1 = new Matrix(new double[,] { {1, 2, 3, 4}, {5, 6, 7, 8}, {9, 8, 7, 6}, {5, 4, 3, 2} });
            var matrix2 = new Matrix(new double[,] { {-2, 1, 2, 3}, {3, 2, 1, -1}, {4, 3, 6, 5}, {1, 2, 7, 8} });
            Matrix result = matrix1 * matrix2;
            Assert.True(result.Equals(new Matrix(new double[,] { {20, 22, 50, 48}, {44, 54, 114, 108}, {40, 58, 110, 102}, {16, 26, 46, 42} })));
        }

        [Fact]
        public void MultiplyingMatrixByTuple_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {1, 2, 3, 4}, {2, 4, 4, 2}, {8, 6, 4, 1}, {0, 0, 0, 1} });
            var tuple = new RayTuple(1, 2, 3, 1);
            RayTuple result = matrix * tuple;
            Assert.True(result.Equals(new RayTuple(18, 24, 33, 1)));
        }

        [Fact]
        public void MultiplyMatrixByIdentityMatrix_ShouldReturnOriginalMatrix()
        {
            var matrix = new Matrix(new double[,] { {0, 1, 2, 4}, {1, 2, 4, 8}, {2, 4, 8, 16}, {4, 8, 16, 32} });
            var identity = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            Matrix result = matrix * identity;
            Assert.True(result.Equals(new Matrix(new double[,] { {0, 1, 2, 4}, {1, 2, 4, 8}, {2, 4, 8, 16}, {4, 8, 16, 32} })));
        }

        [Fact]
        public void MultiplyIdentityMatrixByTuple_ShouldReturnOriginalTuple()
        {
            var identity = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            var tuple = new RayTuple(1, 2, 3, 4);
            RayTuple result = identity * tuple;
            Assert.True(result.Equals(new RayTuple(1, 2, 3, 4)));
        }

        [Fact]
        public void TransposeRegularMatrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {0, 9, 3, 0}, {9, 8, 0, 8}, {1, 8, 5, 3}, {0, 0, 5, 8} });
            Matrix result = matrix.Transpose();
            Assert.True(result.Equals(new Matrix(new double[,] { {0, 9, 1, 0}, {9, 8, 8, 0}, {3, 0, 5, 5}, {0, 8, 3, 8} })));
        }

        [Fact]
        public void TransposeIdentityMatrix_ShouldReturnIdentityMatrix()
        {
            var identity = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            Matrix result = identity.Transpose();
            Assert.True(result.Equals(new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} })));
        }
    }
}
