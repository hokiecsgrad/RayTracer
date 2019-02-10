using System;

namespace RayTracer
{
    public abstract class Pattern
    {
        public Matrix Transform { get; set; }

        public Pattern()
        {
            Transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
        }

        public abstract Color PatternAt(Point point);

        public Color PatternAtShape(Shape shape, Point world_point)
        {
            var object_point = shape.Transform.Inverse() * world_point;
            var pattern_point = this.Transform.Inverse() * object_point;
            return this.PatternAt(pattern_point);
        }
    }
}