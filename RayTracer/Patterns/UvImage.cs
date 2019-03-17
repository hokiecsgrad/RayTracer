using System;

namespace RayTracer
{
    public class UvImage
    {
        public Canvas Image { get; set; }

        public UvImage(Canvas image)
        {
            this.Image = image;
        }

        public Color UvPatternAt(double u, double v)
        {
            // flip v over so it matches the image layout, with y at the top
            var newV = 1 - v;

            var x = u * (this.Image.Width - 1);
            var y = newV * (this.Image.Height - 1);

            // be sure and round x and y to the nearest whole number
            return this.Image.GetPixel((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}