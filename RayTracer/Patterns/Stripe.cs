using System;

namespace RayTracer
{
    public class Stripe : Pattern
    {
        public Color a;
        public Color b;

        public Stripe(Color a, Color b)
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