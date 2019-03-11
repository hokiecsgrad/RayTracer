using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace RayTracer
{
    public abstract class Shape
    {
        protected Matrix _transform = Matrix.Identity;

        protected Matrix _inverse = Matrix.Identity;

        public Matrix Inverse => this._inverse;

        protected Matrix _transpose = Matrix.Identity;

        public Matrix Transpose => this._transpose;

        public Matrix Transform
        {
            get => this._transform;
            set
            {
                this._inverse = value.Inverse();
                this._transpose = this._inverse.Transpose();
                this._transform = value;
            }
        }

        public Material Material { get; set; } = new Material();

        public Shape Parent { get; set; } = null;

        public bool HasParent => this.Parent != null;

        public RayType HitBy { get; set; } = 
            RayType.Primary | 
            RayType.Shadow |
            RayType.Reflection | 
            RayType.Refraction;
            

        public abstract List<Intersection> LocalIntersect(Ray r);

        public List<Intersection> Intersect(Ray r)
        {
            Interlocked.Increment(ref Stats.Tests);
            Ray transformedRay = r.Transform(this.Inverse);
            return LocalIntersect(transformedRay);
        }


        public abstract Vector LocalNormalAt(Point local_point, Intersection hit = null);

        public Vector NormalAt(Point world_point, Intersection i = null)
        {
            Point local_point = this.ConverWorldPointToObjectPoint(world_point);
            Vector local_normal = this.LocalNormalAt(local_point, i);
            return this.NormalToWorld(local_normal);
        }
        

        public Point ConverWorldPointToObjectPoint(Point point)
        {
            if (this.HasParent)
                point = this.Parent.ConverWorldPointToObjectPoint(point);

            return this.Inverse * point;
        }

        public Vector NormalToWorld(Vector normal)
        {
            normal = this.Transpose * normal;
            normal.w = 0;
            normal = normal.Normalize();

            if (this.HasParent)
                normal = this.Parent.NormalToWorld(normal);

            return normal;
        }


        public abstract BoundingBox GetBounds();
       
        public BoundingBox GetParentSpaceBounds()
        {
            return BoundingBox.Transform(this.GetBounds(), this._transform);
        }

        public abstract void Divide(int threshold);
    }
}