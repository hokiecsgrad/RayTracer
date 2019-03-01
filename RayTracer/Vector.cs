using System;
using System.Collections.Generic;

namespace RayTracer
{
    public struct Vector
    {
        public readonly double x;
        public readonly double y;
        public readonly double z;
        public readonly double w;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0.0;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if ( a.w + b.w > 1.0 )
                throw new InvalidOperationException("Cannot add two points together.");

            return new Vector(
                        a.x + b.x, 
                        a.y + b.y, 
                        a.z + b.z);
        }

        public static Point operator +(Vector a, Point b)
        {
            if ( a.w + b.w > 1.0 )
                throw new InvalidOperationException("Cannot add two points together.");

            return new Point(
                        a.x + b.x, 
                        a.y + b.y, 
                        a.z + b.z);
        }

        public static Vector operator -(Vector a) =>
            new Vector(
                -a.x, 
                -a.y, 
                -a.z);

        public static Vector operator -(Vector a, Vector b)
        {
            if ( a.w - b.w < 0.0 )
                throw new InvalidOperationException("Cannot subtract a Point from a Vector.");

            return new Vector(
                        a.x - b.x, 
                        a.y - b.y, 
                        a.z - b.z);
        }

        public static Vector operator -(Vector a, Point b)
        {
            throw new InvalidOperationException("Cannot subtract a Point from a Vector.");
        }

        public static Vector operator *(Vector a, double multiplier) =>
            new Vector(
                    a.x * multiplier, 
                    a.y * multiplier, 
                    a.z * multiplier);

        public static Vector operator /(Vector a, double divisor) =>
            new Vector(
                a.x / divisor, 
                a.y / divisor, 
                a.z / divisor);

        public double Magnitude() =>
            Math.Sqrt(x*x + y*y + z*z);

        public Vector Normalize() =>
            new Vector(
                x / Magnitude(), 
                y / Magnitude(), 
                z / Magnitude());

        public double Dot(Vector other) =>
            x * other.x + y * other.y + z * other.z;

        public Vector Cross(Vector other) =>
            new Vector(
                y * other.z - z * other.y,
                z * other.x - x * other.z,
                x * other.y - y * other.x);

        public Vector Reflect(Vector normal) =>
            this - normal * 2 * this.Dot(normal);

        public static IEqualityComparer<Vector> GetEqualityComparer(double epsilon = 0.0) =>
            new ApproxVectorEqualityComparer(epsilon);

        public override string ToString() =>
            $"({this.x}, {this.y}, {this.z})";
    }

    internal class ApproxVectorEqualityComparer : ApproxEqualityComparer<Vector>
    {
        public ApproxVectorEqualityComparer(double epsilon = 0.0)
            : base(epsilon)
        {
        }

        public override bool Equals(Vector a, Vector b) =>
            this.ApproxEqual(a.x, b.x) &&
            this.ApproxEqual(a.y, b.y) &&
            this.ApproxEqual(a.z, b.z);

        public override int GetHashCode(Vector obj)
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