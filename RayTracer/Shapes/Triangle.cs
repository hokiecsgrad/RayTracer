using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Triangle : Shape
    {
        private const double EPSILON = 0.00001;
        public Point p1;
        public Point p2;
        public Point p3;
        public Vector e1;
        public Vector e2;
        public Vector Normal;

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
            var dirCrossE2 = r.Direction.Cross(e2);
            var det = dirCrossE2.Dot(e1);
            if (Math.Abs(det) < EPSILON)
                return new List<Intersection>();

            var f = 1.0 / det;
            var p1ToOrigin = r.Origin - p1;
            var u = f * p1ToOrigin.Dot(dirCrossE2);
            if (u < 0 || u > 1)
                return new List<Intersection>();

            var originCrossE1 = p1ToOrigin.Cross(e1);
            var v = f * r.Direction.Dot(originCrossE1);
            if (v < 0 || (u + v) > 1)
                return new List<Intersection>();

            var t = f * e2.Dot(originCrossE1);
            return new List<Intersection> { new Intersection(t, this) };
        }

        public override Vector LocalNormalAt(Point local_point)
        {
            return Normal;
        }

        public override BoundingBox GetBounds()
        {
            return new BoundingBox();
        }
    }
}