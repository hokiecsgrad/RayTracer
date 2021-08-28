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

            var ray = this.RayForPixel(x, y);
            Interlocked.Increment(ref Stats.PrimaryRays);
            color += world.ColorAt(ray);

            return color;
        }
    }
}