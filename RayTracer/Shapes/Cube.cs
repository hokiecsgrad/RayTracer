using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Cube : Shape
    {
        private const double EPSILON = 0.00001;
        public Point Min { get; set; }
        public Point Max { get; set; }


        private static double GetMax(double a, double b, double c) =>
            Math.Max(a, Math.Max(b, c));

        private static double GetMin(double a, double b, double c) =>
            Math.Min(a, Math.Min(b, c));

        public Cube()
        {
            this.Min = new Point(-1, -1, -1);
            this.Max = new Point(1, 1, 1);
        }
        
        public Cube(Point min, Point max)
        {
            this.Min = min;
            this.Max = max;
        }

        private (double min, double max) CheckAxis(double origin, double direction, double min = -1, double max = 1)
        {
            var tmin_numerator = min - origin;
            var tmax_numerator = max - origin;

            var tmin = Double.PositiveInfinity;
            var tmax = 0.0;

            if (Math.Abs(direction) >= EPSILON)
            {
                tmin = tmin_numerator / direction;
                tmax = tmax_numerator / direction;
            } 
            else 
            {
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
            var xtVals = CheckAxis(ray.Origin.x, ray.Direction.x, this.Min.x, this.Max.x);
            var ytVals = CheckAxis(ray.Origin.y, ray.Direction.y, this.Min.y, this.Max.y);
            var ztVals = CheckAxis(ray.Origin.z, ray.Direction.z, this.Min.z, this.Max.z);

            var tmin = GetMax(xtVals.min, ytVals.min, ztVals.min);
            var tmax = GetMin(xtVals.max, ytVals.max, ztVals.max);

            if (tmin > tmax) return new List<Intersection>();

            return new List<Intersection> { 
                new Intersection(tmin, this), 
                new Intersection(tmax, this) };
        }

        public override Vector LocalNormalAt(Point local_point, Intersection hit = null)
        {
            var maxc = GetMax(
                        Math.Abs(local_point.x),
                        Math.Abs(local_point.y), 
                        Math.Abs(local_point.z)
                        );

            if (maxc == Math.Abs(local_point.x))
            {
                return new Vector(local_point.x, 0, 0);
            }
            else if (maxc == Math.Abs(local_point.y))
            {
                return new Vector(0, local_point.y, 0);
            }
            else
            {
                return new Vector(0, 0, local_point.z);
            }
        }

        public override BoundingBox GetBounds() =>
            new BoundingBox(
                new Point(-1, -1, -1), 
                new Point(1, 1, 1));

        public override void Divide(int threshold) { }
    }
}