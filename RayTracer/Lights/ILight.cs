using System;
using System.Collections.Generic;

namespace RayTracer
{
    public interface ILight
    {
        Color Color { get; }
        int Samples { get; }

        double IntensityAt(Point point, World world);
        IEnumerable<Point> Sample();
    } 
}
