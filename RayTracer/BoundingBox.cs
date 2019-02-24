using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class BoundingBox
    {
        private const double EPSILON = 0.00001;
        public Point Min;
        public Point Max;

        public BoundingBox()
        {
            Min = new Point(double.PositiveInfinity,double.PositiveInfinity,double.PositiveInfinity);
            Max = new Point(double.NegativeInfinity,double.NegativeInfinity,double.NegativeInfinity);
        }

        public BoundingBox(Point min, Point max)
        {
            Min = min;
            Max = max;
        }

        public void Add(Point point)
        {
            if (point.x < Min.x) Min.x = point.x;
            if (point.y < Min.y) Min.y = point.y;
            if (point.z < Min.z) Min.z = point.z;

            if (point.x > Max.x) Max.x = point.x;
            if (point.y > Max.y) Max.y = point.y;
            if (point.z > Max.z) Max.z = point.z;
        }

        public void Add(BoundingBox box)
        {
            this.Add(box.Min);
            this.Add(box.Max);
        }

        public bool Contains(Point point)
        {
            return ((Min.x <= point.x && point.x <= Max.x)
                    && (Min.y <= point.y && point.y <= Max.y)
                    && (Min.z <= point.z && point.z <= Max.z));
        }

        public bool Contains(BoundingBox box)
        {
            return ((Min.x <= box.Min.x && Min.y <= box.Min.y && Min.z <= box.Min.z)
                && (Max.x >= box.Max.x && Max.y >= box.Max.y && Max.z >= box.Max.z));
        }

        public static BoundingBox Transform(BoundingBox box, Matrix matrix)
        {
            var p1 = box.Min;
            var p2 = new Point(box.Min.x, box.Min.y, box.Max.z);
            var p3 = new Point(box.Min.x, box.Max.y, box.Min.z);
            var p4 = new Point(box.Min.x, box.Max.y, box.Max.z);
            var p5 = new Point(box.Max.x, box.Min.y, box.Min.z);
            var p6 = new Point(box.Max.x, box.Min.y, box.Max.z);
            var p7 = new Point(box.Max.x, box.Max.y, box.Min.z);
            var p8 = box.Max;

            var new_bbox = new BoundingBox();

            foreach (Point p in new List<Point> {p1, p2, p3, p4, p5, p6, p7, p8})
                new_bbox.Add(matrix * p);

            return new_bbox;
        }

        private (double, double) CheckAxis(double origin, double direction, double min, double max)
        {
            var tmin_numerator = (min - origin);
            var tmax_numerator = (max - origin);
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

        public bool Intersects(Ray ray)
        {
            var xtVals = CheckAxis(ray.Origin.x, ray.Direction.x, Min.x, Max.x);
            var ytVals = CheckAxis(ray.Origin.y, ray.Direction.y, Min.y, Max.y);
            var ztVals = CheckAxis(ray.Origin.z, ray.Direction.z, Min.z, Max.z);
            var tmin = Math.Max(xtVals.Item1, Math.Max(ytVals.Item1, ztVals.Item1));
            var tmax = Math.Min(xtVals.Item2, Math.Min(ytVals.Item2, ztVals.Item2));
            if (tmin > tmax) return false;
            return true;
        }
    }
}