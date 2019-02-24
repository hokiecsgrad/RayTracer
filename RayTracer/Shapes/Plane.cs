using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Plane : Shape
    {
        private const double EPSILON = 0.00001;

        public override List<Intersection> LocalIntersect(Ray r)
        {
            if (Math.Abs(r.Direction.y) < EPSILON)
                return new List<Intersection>();
            
            var t1 = -r.Origin.y / r.Direction.y;
            return new List<Intersection> { new Intersection(t1, this) };
        }

        public override Vector LocalNormalAt(Point local_point)
        {
            return new Vector(0, 1, 0);
        }

        public override BoundingBox GetBounds()
        {
            return new BoundingBox(new Point(double.NegativeInfinity, 0, double.NegativeInfinity), 
                                   new Point(double.PositiveInfinity, 0, double.PositiveInfinity));
        }
    }
}