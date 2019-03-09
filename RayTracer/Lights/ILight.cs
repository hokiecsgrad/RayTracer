using System;

namespace RayTracer
{
    public interface ILight
    {
        Point Position { get; }
        Color Color { get; }

        double IntensityAt(Point point, World world);
    } 
}
