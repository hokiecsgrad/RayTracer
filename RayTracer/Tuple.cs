using System;

namespace RayTracer
{
    public class RayTuple : Tuple<double, double, double, double>
    {
        private const double EPSILON = 0.00001;

        public RayTuple(double x, double y, double z, double w) : base(x, y, z, w) {}

        public override bool Equals(Object other)
        {
            RayTuple objTuple = other as RayTuple;

            if (objTuple == null) {
                return false;
            }
 
            return (Math.Abs(objTuple.Item1 - this.Item1) < EPSILON) && (Math.Abs(objTuple.Item2 - this.Item2) < EPSILON) && (Math.Abs(objTuple.Item3 - this.Item3) < EPSILON) && (Math.Abs(objTuple.Item4 - this.Item4) < EPSILON);
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
    }
}
