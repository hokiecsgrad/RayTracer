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

        protected abstract List<Intersection> LocalIntersect(Ray r);

        public List<Intersection> Intersect(Ray r)
        {
            Ray transformedRay = r.Transform(this.Transform.Inverse());
            return LocalIntersect(transformedRay);
        }

        protected abstract Vector LocalNormalAt(Point local_point);

        public Vector Normal_at(Point world_point)
        {
            Point local_point = this.Transform.Inverse() * world_point;
            Vector local_normal = LocalNormalAt(local_point);
            Vector world_normal = this.Transform.Inverse().Transpose() * local_normal;
            world_normal.w = 0;
            return world_normal.Normalize();
        }
    }
}