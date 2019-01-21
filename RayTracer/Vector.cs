using System;

namespace RayTracer
{
    public class Vector
    {
        private const double EPSILON = 0.00001;
        public double x,y,z,w;

        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0.0;
        }

        public static Vector operator+(Vector a, Vector b)
        {
            if ( a.w + b.w > 1.0 )
                throw new InvalidOperationException("Cannot add two points together.");

            return new Vector(a.x + b.x, 
                                a.y + b.y, 
                                a.z + b.z);
        }

        public static Point operator+(Vector a, Point b)
        {
            if ( a.w + b.w > 1.0 )
                throw new InvalidOperationException("Cannot add two points together.");

            return new Point(a.x + b.x, 
                                a.y + b.y, 
                                a.z + b.z);
        }

        public static Vector operator-(Vector a)
        {
            return new Vector(-a.x, -a.y, -a.z);
        }

        public static Vector operator-(Vector a, Vector b)
        {
            if ( a.w - b.w < 0.0 )
                throw new InvalidOperationException("Cannot subtract a Point from a Vector.");

            return new Vector(a.x - b.x, 
                                a.y - b.y, 
                                a.z - b.z);
        }

        public static Vector operator-(Vector a, Point b)
        {
            throw new InvalidOperationException("Cannot subtract a Point from a Vector.");
        }

        public static Vector operator*(Vector a, double multiplier)
        {
            return new Vector(a.x * multiplier, 
                                a.y * multiplier, 
                                a.z * multiplier);
        }

        public static Vector operator/(Vector a, double divisor)
        {
            return new Vector(a.x / divisor, 
                                a.y / divisor, 
                                a.z / divisor);
        }

        public override bool Equals(Object other)
        {
            Vector objTuple = other as Vector;

            if (objTuple == null) {
                return false;
            }
 
            return (Math.Abs(objTuple.x - this.x) < EPSILON) && 
                    (Math.Abs(objTuple.y - this.y) < EPSILON) && 
                    (Math.Abs(objTuple.z - this.z) < EPSILON) && 
                    (Math.Abs(objTuple.w - this.w) < EPSILON);
        }

        public double Magnitude()
        {
            return Math.Sqrt(x*x + y*y + z*z);
        }

        public Vector Normalize()
        {
            return new Vector(x / Magnitude(), y / Magnitude(), z / Magnitude());
        }

        public double Dot(Vector other)
        {
            return x * other.x + y * other.y + z * other.z;
        }

        public Vector Cross(Vector other)
        {
            return new Vector(y * other.z - z * other.y,
                              z * other.x - x * other.z,
                              x * other.y - y * other.x);
        }

        public Vector Reflect(Vector normal)
        {
            return this - normal * 2 * this.Dot(normal);
        }
    }
}