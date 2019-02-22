using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Group : Shape
    {
        private const double EPSILON = 0.00001;
        private List<Shape> Shapes = new List<Shape>();

        public void AddShape(Shape shape)
        {
            shape.Parent = this;
            this.Shapes.Add(shape);
        }

        public List<Shape> GetShapes() { return this.Shapes; }

        public override List<Intersection> LocalIntersect(Ray r)
        {
            var xs = new List<Intersection>();
            foreach (var shape in this.Shapes)
                xs.AddRange(shape.Intersect(r));
            return xs;
        }

        public override Vector LocalNormalAt(Point local_point)
        {
            // TODO: Throw an exception here since this should never be called.
            return new Vector(0, 0, 0);
        }
    }
}