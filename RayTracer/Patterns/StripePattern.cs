using System;

namespace RayTracer
{
    public class StripePattern : Pattern
    {
        public Color a;
        public Color b;

        public StripePattern(Color a, Color b)
        {
            this.a = a;
            this.b = b;
        }

        public override Color PatternAt(Point point)
        {
            if (Math.Floor(point.x) % 2 == 0)
                return a;
            else
                return b;
        }
    }
}