using System;

namespace RayTracer
{
    public class Matrix
    {
        private const double EPSILON = 0.00001;
        private readonly double[,] Data;
        public int Rows => Data.GetUpperBound(0) + 1;
        public int Cols => Data.GetUpperBound(1) + 1;

        public Matrix(double[,] data)
        {
            Data = data;
        }

        public Matrix(int rows, int cols)
        {
            Data = new double[rows, cols];
        }

        public ref double this[int row, int column] => ref Data[row, column];

        public static Matrix operator*(Matrix a, Matrix b)
        {
            if ( a.Rows != b.Cols )
                throw new ArgumentException("Matrix Rows/Cols mismatch.");

            Matrix result = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Cols; j++)
                {
                    double value = 0.0;
                    for (int m = 0; m < a.Cols; m++)
                        value += a[i, m] * b[m, j];
                    result[i, j] = value;
                }

            return result;
        }

        public static RayTuple operator*(Matrix a, RayTuple b)
        {
            if ( a.Rows != 4 )
                throw new ArgumentException("Matrix must have same number of Rows as a Tuple.");

            var result = new double[4];
            for (int i = 0; i < a.Rows; i++)
                result[i] = a[i, 0] * b.Item1 + a[i, 1] * b.Item2 + a[i, 2] * b.Item3 + a[i, 3] * b.Item4;

            return new RayTuple(result[0], result[1], result[2], result[3]);
        }

        public Matrix Transpose()
        {
            var result = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result[i, j] = Data[j, i];
            return result;
        }

        public double Determinant()
        {
            double determinant = 0.0;

            if ( Rows == 2 && Cols == 2 )
                determinant = Data[0,0] * Data[1,1] - Data[0,1] * Data[1, 0];
            else
                for (int col = 0; col < Cols; col++)
                    determinant = determinant + Data[0, col] * Cofactor(0, col);
                
            return determinant;
        }

        public Matrix Submatrix(int row, int col)
        {
            var result = new Matrix(Rows - 1, Cols - 1);
            int currRow = 0, currCol = 0;
            for (int i = 0; i < Rows; i++)
            {
                if ( i == row ) continue;
                currCol = 0;
                for (int j = 0; j < Cols; j++)
                {
                    if ( j == col ) continue;
                    result[currRow, currCol] = Data[i, j];
                    currCol++;
                }
                currRow++;
            }
            return result;
        }

        public double Minor(int row, int col)
        {
            return this.Submatrix(row, col).Determinant();
        }

        public double Cofactor(int row, int col)
        {
            int modifier = 1;
            if ((row + col) % 2 != 0)
                modifier = -1;
            return Minor(row, col) * modifier;
        }

        public bool CanBeInversed()
        {
            if ( Determinant() == 0 )
                return false;
            else 
                return true;
        }

        public Matrix Inverse()
        {
            if ( !CanBeInversed() ) throw new ArgumentException("Matrix cannot be inverted.");

            Matrix inverted = new Matrix(Rows, Cols);
            for (int row = 0; row < Rows; row++)
                for (int col = 0; col < Cols; col++)
                {
                    double cofactor = Cofactor(row, col);
                    inverted[col, row] = cofactor / Determinant();
                }

            return inverted;
        }

        public bool Equals(Matrix other)
        {
            if (Rows != other.Rows || Cols != other.Cols)
                return false;

            for (int row = 0; row < Rows; row++)
                for (int col = 0; col < Cols; col++)
                    if (Math.Abs(other[row, col] - Data[row, col]) > EPSILON) return false;

            return true;
        }
    }
}