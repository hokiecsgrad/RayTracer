using System;

namespace RayTracer
{
    public abstract class Pattern
    {
        protected Matrix _transform = Matrix.Identity;

        public Matrix Transform 
        { 
            get => this._transform;
            set 
            { 
                this._transform = value;
                this._inverse = value.Inverse(); 
            }
        }

        protected Matrix _inverse = Matrix.Identity;

        public Matrix Inverse => this._inverse;


        public abstract Color PatternAt(Point point);

        public Color PatternAtShape(Shape shape, Point world_point)
        {
            var object_point = shape.ConverWorldPointToObjectPoint(world_point);
            var pattern_point = this.Inverse * object_point;
            return this.PatternAt(pattern_point);
        }
    }
}