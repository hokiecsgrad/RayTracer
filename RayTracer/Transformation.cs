using System;

namespace RayTracer
{
    public class Transformation
    {

        public static Matrix Translation(double x, double y, double z)
        {
            var transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            transform[0, 3] = x;
            transform[1, 3] = y;
            transform[2, 3] = z;
            return transform;
        }

        public static Matrix Scaling(double x, double y, double z)
        {
            var transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            transform[0, 0] = x;
            transform[1, 1] = y;
            transform[2, 2] = z;
            return transform;
        }

        public static Matrix Rotation_x(double radians)
        {
            var transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            transform[1, 1] = Math.Cos(radians);
            transform[1, 2] = -Math.Sin(radians);
            transform[2, 1] = Math.Sin(radians);
            transform[2, 2] = Math.Cos(radians);
            return transform;
        }

        public static Matrix Rotation_y(double radians)
        {
            var transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            transform[0, 0] = Math.Cos(radians);
            transform[0, 2] = Math.Sin(radians);
            transform[2, 0] = -Math.Sin(radians);
            transform[2, 2] = Math.Cos(radians);
            return transform;
        }

        public static Matrix Rotation_z(double radians)
        {
            var transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            transform[0, 0] = Math.Cos(radians);
            transform[0, 1] = -Math.Sin(radians);
            transform[1, 0] = Math.Sin(radians);
            transform[1, 1] = Math.Cos(radians);
            return transform;
        }

        public static Matrix Shearing(double xy, double xz, double yx, double yz, double zx, double zy)
        {
            var transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            transform[0, 1] = xy;
            transform[0, 2] = xz;
            transform[1, 0] = yx;
            transform[1, 2] = yz;
            transform[2, 0] = zx;
            transform[2, 1] = zy;
            return transform;
        }
    }
}

