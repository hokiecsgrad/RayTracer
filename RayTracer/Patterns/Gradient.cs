using System;

namespace RayTracer
{
    public class Gradient : Pattern
    {
        public Color a;
        public Color b;

        public Gradient(Color a, Color b)
        {
            this.a = a;
            this.b = b;
        }

        public override Color PatternAt(Point point)
        {
            var distance = this.b - this.a;
            var fraction = point.x - Math.Floor(point.x);
            return this.a + distance * fraction;
        }
    }
}