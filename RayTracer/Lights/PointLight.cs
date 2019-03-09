using System;

namespace RayTracer
{
    public class PointLight : ILight
    {
        public Point Position { get; }
        public Color Color { get; }

        public PointLight(Point position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        public double IntensityAt(Point point, World world)
        {
            if (world.IsShadowed(point, this.Position))
                return 0.0;
            else
                return 1.0;
        }
    }
}