using System;

namespace RayTracer
{
    public class Point
    {
        private const double EPSILON = 0.00001;
        public double x,y,z,w;

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1.0;
        }

        public static Point operator+(Point a, Vector b)
        {
            if ( a.w + b.w > 1.0 )
                throw new InvalidOperationException("Cannot add two points together.");

            return new Point(a.x + b.x, 
                             a.y + b.y, 
                             a.z + b.z);
        }

        public static Point operator-(Point a)
        {
            return new Point(-a.x, -a.y, -a.z);
        }

        public static Vector operator-(Point a, Point b)
        {
            if ( a.w - b.w < 0.0 )
                throw new InvalidOperationException("Cannot subtract a Point from a Vector.");

            return new Vector(a.x - b.x, 
                              a.y - b.y, 
                              a.z - b.z);
        }

        public static Point operator-(Point a, Vector b)
        {
            if ( a.w - b.w < 0.0 )
                throw new InvalidOperationException("Cannot subtract a Point from a Vector.");

            return new Point(a.x - b.x, 
                             a.y - b.y, 
                             a.z - b.z);
        }

        public static Point operator*(Point a, double multiplier)
        {
            return new Point(a.x * multiplier, 
                             a.y * multiplier, 
                             a.z * multiplier);
        }

        public static Point operator/(Point a, double divisor)
        {
            return new Point(a.x / divisor, 
                             a.y / divisor, 
                             a.z / divisor);
        }

        public override bool Equals(Object other)
        {
            Point objTuple = other as Point;

            if (objTuple == null) {
                return false;
            }
 
            return (Math.Abs(objTuple.x - this.x) < EPSILON) && 
                    (Math.Abs(objTuple.y - this.y) < EPSILON) && 
                    (Math.Abs(objTuple.z - this.z) < EPSILON) && 
                    (Math.Abs(objTuple.w - this.w) < EPSILON);
        }
    }
}