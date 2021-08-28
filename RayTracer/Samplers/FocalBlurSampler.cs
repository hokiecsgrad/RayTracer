/*
    Code originally authored by @basp on Github
    https://github.com/basp/pixie.net/blob/master/src/Pixie.Core/FocalBlurSampler.cs
*/

using System;
using System.Threading;

namespace RayTracer
{
    public class FocalBlurSampler : ISampler
    {
        private readonly double focalLength;
        private readonly double aperture;
        private readonly int numSamples;

        public FocalBlurSampler(
            Camera camera,
            double focalLength = 1.0,
            double aperture = 0.1,
            int n = 8) :
                base(camera)
        {
            this.focalLength = focalLength;
            this.aperture = aperture;
            this.numSamples = n;
        }

        public override Color Sample(int x, int y, World world)
        {
            Color color = Color.Black;

            Ray ray = this.RayForPixel(x, y);
            Point focalPoint = ray.Position(this.focalLength);

            for (int i = 0; i < this.numSamples; i++)
            {
                Vector apertureOffset = RandomInUnitDisk() * this.aperture;

                Point newOrigin = ray.Origin + apertureOffset;
                Vector direction = (focalPoint - newOrigin).Normalize();
                Ray secondaryRay = new Ray(newOrigin, direction, RayType.Primary);

                Interlocked.Increment(ref Stats.PrimaryRays);
                color += world.ColorAt(secondaryRay);
            }

            return color / (double)numSamples;
        }

        public Ray RayForPixel(double px, double py)
        {
            double xoffset = (px + 0.5) * this.Camera.PixelSize;
            double yoffset = (py + 0.5) * this.Camera.PixelSize;

            double world_x = this.Camera.HalfWidth - xoffset;
            double world_y = this.Camera.HalfHeight - yoffset;

            Point pixel = this.Camera.TransformInverse * new Point(world_x, world_y, -1);
            Point origin = this.Camera.TransformInverse * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();

            return new Ray(origin, direction, RayType.Primary);
        }


        private Vector RandomInUnitDisk()
        {
            double r = Math.Sqrt(RandomGeneratorThreadSafe.NextDouble());
            double theta = RandomGeneratorThreadSafe.NextDouble() * 2 * Math.PI;
            double x = r * Math.Cos(theta);
            double y = r * Math.Sin(theta);
            return new Vector(x, y, 0);
        }
    }
}