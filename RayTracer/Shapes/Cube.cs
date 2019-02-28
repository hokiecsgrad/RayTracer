using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Cube : Shape
    {
        private const double EPSILON = 0.00001;

        private (double, double) CheckAxis(double origin, double direction)
        {
            var tmin_numerator = (-1 - origin);
            var tmax_numerator = (1 - origin);
            var tmin = Double.PositiveInfinity;
            var tmax = 0.0;

            if (Math.Abs(direction) >= EPSILON)
            {
                tmin = tmin_numerator / direction;
                tmax = tmax_numerator / direction;
            } else {
                tmin = tmin_numerator * Double.PositiveInfinity;
                tmax = tmax_numerator * Double.PositiveInfinity;
            }
            if (tmin > tmax) { 
                var temp = tmin;
                tmin = tmax;
                tmax = temp;
            }
            return (tmin, tmax);
        }

        public override List<Intersection> LocalIntersect(Ray ray)
        {
            var xtVals = CheckAxis(ray.Origin.x, ray.Direction.x);
            var ytVals = CheckAxis(ray.Origin.y, ray.Direction.y);
            var ztVals = CheckAxis(ray.Origin.z, ray.Direction.z);
            var tmin = Math.Max(xtVals.Item1, Math.Max(ytVals.Item1, ztVals.Item1));
            var tmax = Math.Min(xtVals.Item2, Math.Min(ytVals.Item2, ztVals.Item2));
            if (tmin > tmax) return new List<Intersection>();
            return new List<Intersection> { new Intersection(tmin, this), new Intersection(tmax, this) };
        }

        public override Vector LocalNormalAt(Point local_point)
        {
            var maxc = Math.Max(Math.Abs(local_point.x), Math.Max(Math.Abs(local_point.y), Math.Abs(local_point.z)));
            if (maxc == Math.Abs(local_point.x))
                return new Vector(local_point.x, 0, 0);
            else if (maxc == Math.Abs(local_point.y))
                return new Vector(0, local_point.y, 0);
            return new Vector(0, 0, local_point.z);
        }

        public override BoundingBox GetBounds()
        {
            return new BoundingBox(new Point(-1, -1, -1), new Point(1, 1, 1));
        }

        public override void Divide(int threshold) { }
    }
}