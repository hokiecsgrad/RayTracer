using System;

namespace RayTracer
{
    public abstract class Pattern
    {
        private Matrix CacheTransformInverse = null;
        private Matrix _transform;
        public Matrix Transform { 
            get { return _transform; } 
            set { _transform = value; CacheTransformInverse = value.Inverse(); }
        }

        public Pattern()
        {
            Transform = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
            CacheTransformInverse = new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} });
        }

        public abstract Color PatternAt(Point point);

        public Color PatternAtShape(Shape shape, Point world_point)
        {
            var object_point = shape.ConverWorldPointToObjectPoint(world_point);
            //var object_point = shape.Transform.Inverse() * world_point;
            var pattern_point = this.Transform.Inverse() * object_point;
            return this.PatternAt(pattern_point);
        }
    }
}