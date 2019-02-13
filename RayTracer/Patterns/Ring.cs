using System;

namespace RayTracer
{
    public class Ring : Pattern
    {
        public Color a;
        public Color b;

        public Ring(Color a, Color b)
        {
            this.a = a;
            this.b = b;
        }

        public override Color PatternAt(Point point)
        {
            if (Math.Floor(Math.Sqrt(point.x*point.x+point.z*point.z)) % 2 == 0)
                return this.a;
            else
                return this.b;
        }
    }
}