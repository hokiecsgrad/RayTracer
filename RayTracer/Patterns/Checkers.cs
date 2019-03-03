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
            var x = Math.Floor(point.x);
            var y = Math.Floor(point.y);
            var z = Math.Floor(point.z);

            if ( (x + y + z) % 2 == 0 )
                return this.a;
            else
                return this.b;
        }
    }
}