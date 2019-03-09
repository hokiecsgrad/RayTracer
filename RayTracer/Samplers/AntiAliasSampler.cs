/*
    Code originally authored by @basp on Github
    https://github.com/basp/pixie.net/blob/master/src/Pixie.Core/RandomSuperSampler.cs
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace RayTracer
{
    public class AntiAliasSampler : ISampler
    {
        private static readonly Random rng = new Random();
        private int NumSamples { get; set; }

        public AntiAliasSampler(Camera camera, int numSamples) :
            base(camera)
        {
            this.NumSamples = numSamples;
        }

        public AntiAliasSampler(Camera camera) :
            base(camera) 
        {
            this.NumSamples = 4;
        }

        public List<Ray> RaysForPixel(double px, double py)
        {
            var rays = new List<Ray>();
            var inv = this.Camera.TransformInverse;
            var origin = inv * new Point(0, 0, 0);

            var pixelSize = this.Camera.PixelSize;
            var halfWidth = this.Camera.HalfWidth;
            var halfHeight = this.Camera.HalfHeight;

            for (var i = 0; i < this.NumSamples; i++)
            {
                var xOffset = (px + 0.5);
                var yOffset = (py + 0.5);

                var rx = rng.NextDouble();
                var ry = rng.NextDouble();

                xOffset += (0.5 - rx);
                yOffset += (0.5 - ry);

                xOffset *= pixelSize;
                yOffset *= pixelSize;

                var worldX = halfWidth - xOffset;
                var worldY = halfHeight - yOffset;

                var pixel = inv * new Point(worldX, worldY, -1);
                var direction = (pixel - origin).Normalize();

                rays.Add(new Ray(origin, direction));
            }
            return rays;
        }

        public override Color Sample(int x, int y, World world)
        {
            Color color = Color.Black;
            foreach (var ray in this.RaysForPixel(x, y))
            {
                Interlocked.Increment(ref Stats.PrimaryRays);
                color += world.ColorAt(ray);
            }
            color *= 0.25;
            return color;
        }
    }
}