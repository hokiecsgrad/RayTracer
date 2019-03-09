using System;
using System.Collections.Generic;

namespace RayTracer
{
    public class AreaLight : ILight
    {
        public Point Position { get; }
        public Color Color { get; }
        public Point Corner;
        public Vector FullUVec;
        public Vector UVec;
        public int USteps;
        public Vector FullVVec;
        public Vector VVec;
        public int VSteps;
        public int Samples { get; }
        public ISequence JitterBy { get; set; }

        public AreaLight(
            Point corner, 
            Vector uvec, 
            int usteps, 
            Vector vvec, 
            int vsteps, 
            Color color)
        {
            this.Corner = corner;
            this.FullUVec = uvec;
            this.USteps = usteps;
            this.UVec = FullUVec / usteps;
            this.FullVVec = vvec;
            this.VSteps = vsteps;
            this.VVec = FullVVec / vsteps;
            this.Color = color;
            this.Samples = usteps * vsteps;
            this.Position = (this.Corner + (this.FullUVec + this.FullVVec)) / 2;
            this.JitterBy = new RandomSequence();
        }

        public Point PointOnLight(double u, double v) =>
            this.Corner +
            this.UVec * (u + this.JitterBy.Next()) +
            this.VVec * (v + this.JitterBy.Next());

        public double IntensityAt(Point point, World world)
        {
            var total = 0.0;

            for (int v = 0; v < this.VSteps; v++)
            {
                for (int u = 0; u < this.USteps; u++)
                {
                    var light_position = this.PointOnLight(u, v);
                    if (! world.IsShadowed(light_position, point))
                        total = total + 1.0;
                }
            }

            return total / this.Samples;
        }

        public IEnumerable<Point> Sample()
        {
            for (var v = 0; v < this.VSteps; v++)
                for (var u = 0; u < this.USteps; u++)
                    yield return this.PointOnLight(u, v);
        }
    }
}