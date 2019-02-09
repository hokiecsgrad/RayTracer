using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Plane : Shape
    {
        private const double EPSILON = 0.00001;

        protected override List<Intersection> LocalIntersect(Ray r)
        {
            if (Math.Abs(r.Direction.y) < EPSILON)
                return new List<Intersection>();
            
            var t1 = -r.Origin.y / r.Direction.y;
            return new List<Intersection> { new Intersection(t1, this) };
        }

        protected override Vector LocalNormalAt(Point local_point)
        {
            return new Vector(0, 1, 0);
        }
    }
}