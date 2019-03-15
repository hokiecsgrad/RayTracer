using System;

namespace RayTracer
{
    public class UvCheckers
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public Color a { get; set; } = Color.White;
        public Color b { get; set; } = Color.Black;

        public UvCheckers(int width, int height, Color a, Color b)
        {
            this.Width = width;
            this.Height = height;
            this.a = a;
            this.b = b;
        }

        public Color UvPatternAt(double u, double v)
        {
            var u2 = Math.Floor(u * this.Width);
            var v2 = Math.Floor(v * this.Height);

            if ((u2 + v2) % 2 == 0)
                return this.a;
            else
                return this.b;
        }
    }
}