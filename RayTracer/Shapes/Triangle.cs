using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Triangle : Shape
    {
        protected const double EPSILON = 0.00001;

        public Point p1 { get; }

        public Point p2 { get; }

        public Point p3 { get; }

        public Vector e1 { get; }

        public Vector e2 { get; }

        public Vector Normal { get; }

        public Triangle(Point point1, Point point2, Point point3)
        {
            p1 = point1;
            p2 = point2;
            p3 = point3;
            e1 = p2 - p1;
            e2 = p3 - p1;
            Normal = e2.Cross(e1).Normalize();
        }

        public override List<Intersection> LocalIntersect(Ray r)
        {
            var dirCrossE2 = r.Direction.Cross(this.e2);
            var det = dirCrossE2.Dot(this.e1);

            if (Math.Abs(det) < EPSILON)
                return new List<Intersection>();

            var f = 1.0 / det;
            var p1ToOrigin = r.Origin - this.p1;
            var u = f * p1ToOrigin.Dot(dirCrossE2);

            if (u < 0 || u > 1)
                return new List<Intersection>();

            var originCrossE1 = p1ToOrigin.Cross(this.e1);
            var v = f * r.Direction.Dot(originCrossE1);

            if (v < 0 || (u + v) > 1)
                return new List<Intersection>();

            var t = f * this.e2.Dot(originCrossE1);

            return new List<Intersection> { 
                new Intersection(t, this) };
        }

        public override Vector LocalNormalAt(Point local_point, Intersection hit = null) =>
            this.Normal;


        public override BoundingBox GetBounds()
        {
            var box = new BoundingBox();

            box.Add(this.p1);
            box.Add(this.p2);
            box.Add(this.p3);

            return box;
        }

        public override void Divide(int threshold) { }
    }
}