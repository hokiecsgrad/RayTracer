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

        [Fact]
        public void CalculatingDeterminantOf2x2Matrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {1, 5}, {-3, 2} });
            Assert.True(matrix.Determinant() == 17);
        }

        [Fact]
        public void CalculatingSubmatrixOf3x3_ShouldReturn2x2Matrix()
        {
            var matrix = new Matrix(new double[,] { {1, 5, 0}, {-3, 2, 7}, {0, 6, -2} });
            var result = matrix.Submatrix(0, 2);
            Assert.True(result.Equals(new Matrix(new double[,] { {-3, 2}, {0, 6} })));
        }

        [Fact]
        public void CalculatingSubmatrixOf4x4_ShouldReturn3x3Matrix()
        {
            var matrix = new Matrix(new double[,] { {-6, 1, 1, 6}, {-8, 5, 8, 6}, {-1, 0, 8, 2}, {-7, 1, -1, 1} });
            var result = matrix.Submatrix(2, 1);
            Assert.True(result.Equals(new Matrix(new double[,] { {-6, 1, 6}, {-8, 8, 6}, {-7, -1, 1} })));
        }

        [Fact]
        public void CalculatingMinorOf3x3Matrix_ShouldReturnDeterminantOfSubmatrix()
        {
            var matrix = new Matrix(new double[,] { {3, 5, 0}, {2, -1, -7}, {6, -1, 5} });
            var result = matrix.Minor(1, 0);
            Assert.Equal(25, result);
        }

        [Fact]
        public void CalculatingCofactorOfMatrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {3, 5, 0}, {2, -1, -7}, {6, -1, 5} });
            Assert.Equal(-12, matrix.Minor(0, 0));
            Assert.Equal(-12, matrix.Cofactor(0, 0));
            Assert.Equal(25, matrix.Minor(1, 0));
            Assert.Equal(-25, matrix.Cofactor(1, 0));
        }

        [Fact]
        public void CalculatingDeterminantOf3x3Matrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {1, 2, 6}, {-5, 8, -4}, {2, 6, 4} });
            Assert.Equal(56, matrix.Cofactor(0, 0));
            Assert.Equal(12, matrix.Cofactor(0, 1));
            Assert.Equal(-46, matrix.Cofactor(0, 2));
            Assert.Equal(-196, matrix.Determinant());
        }

        [Fact]
        public void CalculatingDeterminantOf4x4Matrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {-2, -8, 3, 5}, {-3, 1, 7, 3}, {1, 2, -9, 6}, {-6, 7, 7, -9} });
            Assert.Equal(690, matrix.Cofactor(0, 0));
            Assert.Equal(447, matrix.Cofactor(0, 1));
            Assert.Equal(210, matrix.Cofactor(0, 2));
            Assert.Equal(51, matrix.Cofactor(0, 3));
            Assert.Equal(-4071, matrix.Determinant());
        }

        [Fact]
        public void IfMatrixDeterminantDoesNotEqualZero_InverseIsPossible()
        {
            var matrix = new Matrix(new double[,] { {6, 4, 4, 4}, {5, 5, 7, 6}, {4, -9, 3, -7}, {9, 1, 7, -6} });
            Assert.True(matrix.CanBeInversed());
        }

        [Fact]
        public void IfMatrixDeterminantEqualsZero_InverseIsNotPossible()
        {
            var matrix = new Matrix(new double[,] { {-4, 2, -2, -3}, {9, 6, 2, 6}, {0, -5, 1, -5}, {0, 0, 0, 0} });
            Assert.False(matrix.CanBeInversed());
        }

        [Fact]
        public void CalculateInverseOfMatrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {-5, 2, 6, -8}, {1, -5, 1, 8}, {7, 7, -6, -7}, {1, -3, 7, 4} });
            var inverseMatrix = matrix.Inverse();
            Assert.Equal(532, matrix.Determinant());
            Assert.Equal(-160, matrix.Cofactor(2, 3));
            Assert.Equal(-160.0/532.0, inverseMatrix[3,2]);
            Assert.Equal(105, matrix.Cofactor(3, 2));
            Assert.Equal(105.0/532.0, inverseMatrix[2,3]);
            Assert.True(inverseMatrix.Equals(new Matrix(new double[,] 
                { {0.21805, 0.45113, 0.24060, -0.04511},
                  {-0.80827, -1.45677, -0.44361, 0.52068},
                  {-0.07895, -0.22368, -0.05263, 0.19737},
                  {-0.52256, -0.81391, -0.30075, 0.30639} })));
        }

        [Fact]
        public void CalculatingInverseOfAnotherMatrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {8, -5, 9, 2}, {7, 5, 6, 1}, {-6, 0, 9, 6}, {-3, 0, -9, -4} });
            Assert.True(matrix.Inverse().Equals(new Matrix(new double[,]
                { {-0.15385, -0.15385, -0.28205, -0.53846}, 
                  {-0.07692, 0.12308, 0.02564, 0.03077},
                  {0.35897, 0.35897, 0.43590, 0.92308},
                  {-0.69231, -0.69231, -0.76923, -1.92308} })));
        }

        [Fact]
        public void CalculatingInverseOfYetAnotherMatrix_ShouldWork()
        {
            var matrix = new Matrix(new double[,] { {9, 3, 0, 9}, {-5, -2, -6, -3}, {-4, 9, 6, 4}, {-7, 6, 6, 2} });
            Assert.True(matrix.Inverse().Equals(new Matrix(new double[,]
                { {-0.04074, -0.07778, 0.14444, -0.22222}, 
                  {-0.07778, 0.03333, 0.36667, -0.33333},
                  {-0.02901, -0.14630, -0.10926, 0.12963},
                  {0.17778, 0.06667, -0.26667, 0.33333} })));
        }

        [Fact]
        public void MultiplyingProductByInverse_ShouldResultInOriginalMatrix()
        {
            var matrix1 = new Matrix(new double[,] { {3, -9, 7, 3}, {3, -8, 2, -9}, {-4, 4, 4, 1}, {-6, 5, -1, 1} });
            var matrix2 = new Matrix(new double[,] { {8, 2, 2, 2}, {3, -1, 7, 0}, {7, 0, 5, 4}, {6, -2, 0, 5} });
            var product = matrix1 * matrix2;
            var result = product * matrix2.Inverse();
            Assert.True(result.Equals(matrix1));
        }
    }
}
