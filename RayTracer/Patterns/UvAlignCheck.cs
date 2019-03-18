using System;

namespace RayTracer
{
    public class UvAlignCheck : IUvPattern
    {
        public Color Main { get; set; } = Color.White;
        public Color Ul { get; set; } = Color.Black;
        public Color Ur { get; set; } = Color.Black;
        public Color Bl { get; set; } = Color.Black;
        public Color Br { get; set; } = Color.Black;

        public UvAlignCheck(Color main, Color ul, Color ur, Color bl, Color br)
        {
            this.Main = main;
            this.Ul = ul;
            this.Ur = ur;
            this.Bl = bl;
            this.Br = br;
        }

        public Color UvPatternAt(double u, double v)
        {
            // remember: v=0 at the bottom, v=1 at the top
            if (v > 0.8)
            {
                if (u < 0.2) return this.Ul;
                if (u > 0.8) return this.Ur;
            }
            else if (v < 0.2)
            {
                if (u < 0.2) return this.Bl;
                if (u > 0.8) return this.Br;
            }

            return this.Main;
       }
    }
}