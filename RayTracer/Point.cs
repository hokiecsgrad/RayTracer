using System;
using System.Collections.Generic;

namespace RayTracer
{
    public class Point
    {
        public static Point Origin =>
            new Point(0,0,0);

        public double x;
        public double y;
        public double z;
        public readonly double w;

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1.0;
        }

        public static Point operator +(Point a, Vector b)
        {
            if ( a.w + b.w > 1.0 )
                throw new InvalidOperationException("Cannot add two points together.");

            return new Point(
                        a.x + b.x, 
                        a.y + b.y, 
                        a.z + b.z);
        }

        public static Point operator -(Point a) =>
            new Point(
                -a.x, 
                -a.y, 
                -a.z);

        public static Vector operator -(Point a, Point b)
        {
            if ( a.w - b.w < 0.0 )
                throw new InvalidOperationException("Cannot subtract a Point from a Vector.");

            return new Vector(
                        a.x - b.x, 
                        a.y - b.y, 
                        a.z - b.z);
        }

        public static Point operator -(Point a, Vector b)
        {
            if ( a.w - b.w < 0.0 )
                throw new InvalidOperationException("Cannot subtract a Point from a Vector.");

            return new Point(
                        a.x - b.x, 
                        a.y - b.y, 
                        a.z - b.z);
        }

        public static Point operator *(Point a, double multiplier) =>
            new Point(
                    a.x * multiplier, 
                    a.y * multiplier, 
                    a.z * multiplier);

        public static Point operator /(Point a, double divisor) =>
            new Point(
                    a.x / divisor, 
                    a.y / divisor, 
                    a.z / divisor);

        public double Magnitude() =>
            Math.Sqrt(x*x + y*y + z*z);
            

        public static IEqualityComparer<Point> GetEqualityComparer(double epsilon = 0.0) =>
            new ApproxPointEqualityComparer(epsilon);

        public override string ToString() =>
            $"({this.x}, {this.y}, {this.z})";
    }

    internal class ApproxPointEqualityComparer : ApproxEqualityComparer<Point>
    {
        public ApproxPointEqualityComparer(double epsilon = 0.0)
            : base(epsilon)
        {
        }

        public override bool Equals(Point a, Point b) =>
            this.ApproxEqual(a.x, b.x) &&
            this.ApproxEqual(a.y, b.y) &&
            this.ApproxEqual(a.z, b.z);

        public override int GetHashCode(Point obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + obj.x.GetHashCode();
                hash = hash * 23 + obj.y.GetHashCode();
                hash = hash * 23 + obj.z.GetHashCode();
                return hash;
            }
        }        
    }    
}