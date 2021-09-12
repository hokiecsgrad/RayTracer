using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class PointLight : ILight
    {
        public Point Position { get; init; }
        public Color Color { get; init; }
        public int Samples { get; init; }

        public PointLight()
        {
            this.Position = new Point(0, 0, 0);
            this.Color = Color.White;
            this.Samples = 1;
        }

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