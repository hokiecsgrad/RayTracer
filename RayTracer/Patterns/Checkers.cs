using System;

namespace RayTracer
{
    public class Checkers : Pattern
    {
        public Color a;
        public Color b;

        public Checkers(Color a, Color b)
        {
            this.a = a;
            this.b = b;
        }

        public override Color PatternAt(Point point)
        {
            if ( (Math.Floor(point.x) + Math.Floor(point.y) + Math.Floor(point.z)) % 2 == 0 )
                return this.a;
            else
                return this.b;
        }
    }
}