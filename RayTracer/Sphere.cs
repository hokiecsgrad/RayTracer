using System;

namespace RayTracer
{
    public class Sphere
    {
        public Point Origin { get; }
        public double Radius { get; }

        public Sphere()
        {
            Origin = new Point(0, 0, 0);
            Radius = 1.0;
        }
    }
}