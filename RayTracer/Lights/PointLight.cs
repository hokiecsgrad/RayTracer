using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class PointLight : ILight
    {
        public Point Position { get; }
        public Color Color { get; }
        public int Samples { get; }

        public PointLight(Point position, Color color)
        {
            this.Position = position;
            this.Color = color;
            this.Samples = 1;
        }

        public double IntensityAt(Point point, World world)
        {
            if (world.IsShadowed(point, this.Position))
                return 0.0;
            else
                return 1.0;
        }

        public IEnumerable<Point> Sample() => new List<Point> { this.Position };
    }
}