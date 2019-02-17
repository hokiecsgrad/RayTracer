using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public abstract class Shape
    {
        private Matrix CacheTransformInverse = null;
        private Matrix _transform;
        public Matrix Transform { 
            get { return _transform; } 
            set { _transform = value; CacheTransformInverse = value.Inverse(); }
        }
        public Material Material { get; set; }
        public bool CastsShadow = true;

        public Shape()
        {
            Transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            Material = new Material();
        }

        public abstract List<Intersection> LocalIntersect(Ray r);

        public List<Intersection> Intersect(Ray r)
        {
            Ray transformedRay = r.Transform(this.CacheTransformInverse);
            return LocalIntersect(transformedRay);
        }

        public abstract Vector LocalNormalAt(Point local_point);

        public Vector NormalAt(Point world_point)
        {
            Point local_point = this.CacheTransformInverse * world_point;
            Vector local_normal = LocalNormalAt(local_point);
            Vector world_normal = this.CacheTransformInverse.Transpose() * local_normal;
            world_normal.w = 0;
            return world_normal.Normalize();
        }
    }
}