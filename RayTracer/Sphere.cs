using System;

namespace RayTracer
{
    public class Sphere
    {
        public Point Origin { get; }
        public double Radius { get; }
        public Matrix Transform { get; set; }
        public Material Material { get; set; }

        public Sphere()
        {
            Origin = new Point(0, 0, 0);
            Radius = 1.0;
            Transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            Material = new Material();
        }

        public Vector Normal_at(Point world_point)
        {
            var object_point = Transform.Inverse() * world_point;
            var object_normal = object_point - new Point(0, 0, 0);
            var world_normal =  Transform.Inverse().Transpose() * object_normal;
            world_normal.w = 0;
            return world_normal.Normalize();
        }
    }
}