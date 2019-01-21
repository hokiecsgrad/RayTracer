using System;

namespace RayTracer
{
    public class Sphere
    {
        public Point Origin { get; }
        public double Radius { get; }
        public Matrix Transform { get; set; }

        public Sphere()
        {
            Origin = new Point(0, 0, 0);
            Radius = 1.0;
            Transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
        }
    }
}