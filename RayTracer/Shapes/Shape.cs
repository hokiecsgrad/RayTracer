using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public abstract class Shape
    {
        public Matrix Transform { get; set; }
        public Material Material { get; set; }

        public Shape()
        {
            Transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            Material = new Material();
        }

        public abstract List<Intersection> Intersect(Ray r);

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