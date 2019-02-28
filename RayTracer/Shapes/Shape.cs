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
        public Shape Parent = null;

        public Shape()
        {
            Transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            CacheTransformInverse = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
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
            Point local_point = this.ConverWorldPointToObjectPoint(world_point);
            Vector local_normal = this.LocalNormalAt(local_point);
            return this.NormalToWorld(local_normal);
        }

        public Point ConverWorldPointToObjectPoint(Point point)
        {
            if (this.Parent != null)
                point = this.Parent.ConverWorldPointToObjectPoint(point);

            return this.CacheTransformInverse * point;
        }

        public Vector NormalToWorld(Vector normal)
        {
            normal = this.CacheTransformInverse.Transpose() * normal;
            normal.w = 0;
            normal = normal.Normalize();
            if (this.Parent != null)
                normal = this.Parent.NormalToWorld(normal);

            return normal;
        }

        public abstract BoundingBox GetBounds();
       
        public BoundingBox GetParentSpaceBounds()
        {
            return BoundingBox.Transform(this.GetBounds(), this.Transform);
        }

        public abstract void Divide(int threshold);
    }
}