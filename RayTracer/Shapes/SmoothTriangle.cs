using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class SmoothTriangle : Triangle
    {
        public Vector n1;

        public Vector n2;

        public Vector n3;


        public SmoothTriangle(Point point1, Point point2, Point point3, Vector norm1, Vector norm2, Vector norm3)
            : base(point1, point2, point3)
        {
            n1 = norm1;
            n2 = norm2;
            n3 = norm3;
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
            return new List<Intersection> { 
                new Intersection(t, this, u, v) 
                };
        }

        public override Vector LocalNormalAt(Point local_point, Intersection hit = null)
        {
            return this.n2 * hit.u +
                    this.n3 * hit.v +
                    this.n1 * (1 - hit.u - hit.v);
        }

        public override BoundingBox GetBounds() =>
            new BoundingBox();

        public override void Divide(int threshold) { }
    }
}