using System;
using System.Collections.Generic;
using System.Threading;

namespace RayTracer
{
    public class DefaultSampler : ISampler
    {
        public DefaultSampler(Camera camera) :
            base(camera)
        { }

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

        public override Color Sample(int x, int y, World world)
        {
            Color color = Color.Black;
            int numSamples = 1;
            int n = (int)Math.Sqrt((double)numSamples);

            for (int p = 0; p < n; p++)
                for (int q = 0; q < n; q++)
                {
                    double currX = this.Camera.PixelSize * (x - 0.5 * this.Camera.HSize + (q + 0.5) / n);
                    double currY = this.Camera.PixelSize * (y - 0.5 * this.Camera.VSize + (p + 0.5) / n);
                    Point pixel = this.Camera.TransformInverse * new Point(-currX, -currY, -1);
                    Point origin = this.Camera.TransformInverse * new Point(0, 0, 0);
                    Vector direction = (pixel - origin).Normalize();
                    var ray = new Ray(origin, direction, RayType.Primary);
                    Interlocked.Increment(ref Stats.PrimaryRays);
                    color += world.ColorAt(ray);
                }

            color /= numSamples;

            return color;
        }
    }
}