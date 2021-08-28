using System;
using System.Collections.Generic;
using System.Threading;

namespace RayTracer
{
    public class SuperSampler : ISampler
    {
        private readonly int numSamples;
        private readonly Color colorEpsilon = new Color(0.005, 0.005, 0.005);

        public SuperSampler(
            Camera camera, 
            int numSamples = 4) :
                base(camera)
        { 
            this.numSamples = numSamples;
        }

        public override Color Sample(int x, int y, World world)
        {
            Color color = Color.Black;
            int n = (int)Math.Sqrt((double)this.numSamples);
            int numActualSamples = 0;

            for (int jitterX = 0; jitterX < n; jitterX++)
                for (int jitterY = 0; jitterY < n; jitterY++)
                {
                    double xoffset = this.Camera.PixelSize * ((x + 0.5) + ((jitterX + 0.5) / numSamples));
                    double yoffset = this.Camera.PixelSize * ((y + 0.5) + ((jitterY + 0.5) / numSamples));

                    double world_x = this.Camera.HalfWidth - xoffset;
                    double world_y = this.Camera.HalfHeight - yoffset;

                    Point pixel = this.Camera.TransformInverse * new Point(world_x, world_y, -1);
                    Point origin = this.Camera.TransformInverse * new Point(0, 0, 0);
                    Vector direction = (pixel - origin).Normalize();
                    var ray = new Ray(origin, direction, RayType.Primary);

                    Interlocked.Increment(ref Stats.PrimaryRays);
                    Color currColor = world.ColorAt(ray);

                    if ( currColor - color < colorEpsilon)
                        return color /= numActualSamples;

                    color += currColor;
                    numActualSamples++;
                }

            return color /= numActualSamples;
        }
    }
}