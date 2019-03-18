using System;

namespace RayTracer
{
    public interface IUvPattern
    {
        Color UvPatternAt(double u, double v);
    }
}