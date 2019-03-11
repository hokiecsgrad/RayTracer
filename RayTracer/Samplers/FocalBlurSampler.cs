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
        private readonly Random rng = new Random();
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

        public Ray RayForPixel(double px, double py)
        {
            // the offset from the edge of the canvas to the pixel's center
            double xoffset = (px + 0.5) * this.Camera.PixelSize;
            double yoffset = (py + 0.5) * this.Camera.PixelSize;

            // the untransformed coordinates of the pixel in world-space.
            // (remember that the camera looks toward -z, so +x is to the *left*.)
            double world_x = this.Camera.HalfWidth - xoffset;
            double world_y = this.Camera.HalfHeight - yoffset;

            // using the camera matrix, transform the canvas point and the origin,
            // and then compute the ray's direction vector.
            // (remember that the canvas is at z=-1)
            Point pixel = this.Camera.TransformInverse * new Point(world_x, world_y, -1);
            Point origin = this.Camera.TransformInverse * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();

            return new Ray(origin, direction, RayType.Primary);
        }

        // It was definitely not a good idea to have this static
        // and multiple threads trying to mess around with the
        // random number generator. So now every thread gets its 
        // own sampler and every sampler gets its own rng.
        private Vector RandomInUnitDisk()
        {
            var r = Math.Sqrt(rng.NextDouble());
            var theta = rng.NextDouble() * 2 * Math.PI;
            var x = r * Math.Cos(theta);
            var y = r * Math.Sin(theta);
            return new Vector(x, y, 0);
        }

        public override Color Sample(int x, int y, World world)
        {
            Color color = Color.Black;
            
            var ray = this.RayForPixel(x, y);
            var focalPoint = ray.Position(this.focalLength);
            
            for (var i = 0; i < this.numSamples; i++)
            {
                // Get a vector with a random displacement 
                // on a disk with radius 1.0 and use it to 
                // offset the origin.
                var offset = RandomInUnitDisk() * this.aperture;

                // Create a new ray offset from the original ray
                // and pointing at the focal point.
                var origin = ray.Origin + offset;
                var direction = (focalPoint - origin).Normalize();
                var secondaryRay = new Ray(origin, direction, RayType.Primary);

                // We probably should count these "secondary" rays
                // as primary rays for stats purposes; this is consistent
                // with RandomSuperSampler behavior.
                Interlocked.Increment(ref Stats.PrimaryRays);
                color += world.ColorAt(secondaryRay);
            }

            return (1.0 / this.numSamples) * color;
        }
    }
}