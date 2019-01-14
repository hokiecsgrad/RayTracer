using System;

namespace RayTracer
{
    public class RayTuple : Tuple<double, double, double, double>
    {
        private const double EPSILON = 0.00001;

        public RayTuple(double x, double y, double z, double w) : base(x, y, z, w) {}

        public static RayTuple operator+(RayTuple a, RayTuple b)
        {
            if ( a.Item4 + b.Item4 > 1.0 )
                throw new InvalidOperationException("Cannot add two points together.");

            return new RayTuple(a.Item1 + b.Item1, 
                                a.Item2 + b.Item2, 
                                a.Item3 + b.Item3, 
                                a.Item4 + b.Item4);
        }

        public static RayTuple operator-(RayTuple a)
        {
            return new RayTuple(-a.Item1, -a.Item2, -a.Item3, -a.Item4);
        }

        public static RayTuple operator-(RayTuple a, RayTuple b)
        {
            if ( a.Item4 - b.Item4 < 0.0 )
                throw new InvalidOperationException("Cannot subtract a Point from a Vector.");

            return new RayTuple(a.Item1 - b.Item1, 
                                a.Item2 - b.Item2, 
                                a.Item3 - b.Item3, 
                                a.Item4 - b.Item4);
        }

        public static RayTuple operator*(RayTuple a, double multiplier)
        {
            return new RayTuple(a.Item1 * multiplier, 
                                a.Item2 * multiplier, 
                                a.Item3 * multiplier, 
                                a.Item4 * multiplier);
        }

        public static RayTuple operator/(RayTuple a, double divisor)
        {
            return new RayTuple(a.Item1 / divisor, 
                                a.Item2 / divisor, 
                                a.Item3 / divisor, 
                                a.Item4 / divisor);
        }

        public override bool Equals(Object other)
        {
            RayTuple objTuple = other as RayTuple;

            if (objTuple == null) {
                return false;
            }
 
            return (Math.Abs(objTuple.Item1 - this.Item1) < EPSILON) && 
                    (Math.Abs(objTuple.Item2 - this.Item2) < EPSILON) && 
                    (Math.Abs(objTuple.Item3 - this.Item3) < EPSILON) && 
                    (Math.Abs(objTuple.Item4 - this.Item4) < EPSILON);
        }
    }

    public class Point : RayTuple
    {
        public Point(double x, double y, double z) : base(x, y, z, 1.0)
        {
        }
    }

    public class Vector : RayTuple
    {
        public Vector(double x, double y, double z) : base(x, y, z, 0.0)
        {
        }

        public double Magnitude()
        {
            return Math.Sqrt(Item1*Item1 + Item2*Item2 + Item3*Item3);
        }

        public Vector Normalize()
        {
            return new Vector(Item1 / Magnitude(), Item2 / Magnitude(), Item3 / Magnitude());
        }

        public double Dot(Vector other)
        {
            return Item1 * other.Item1 + Item2 * other.Item2 + Item3 * other.Item3;
        }

        public Vector Cross(Vector other)
        {
            return new Vector(Item2 * other.Item3 - Item3 * other.Item2,
                              Item3 * other.Item1 - Item1 * other.Item3,
                              Item1 * other.Item2 - Item2 * other.Item1);
        }
    }
}
