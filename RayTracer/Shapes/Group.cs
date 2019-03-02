using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class Group : Shape
    {
        private const double EPSILON = 0.00001;

        public string Name { get; set; } = string.Empty;


        private List<Shape> _shapes = new List<Shape>();

        public List<Shape> Shapes { 
            get => this._shapes; 
        }

        public Shape this[int index]
        {
            get => this._shapes[index];
            set => this._shapes[index] = value;
        }

        public int Count => this._shapes.Count;

        public void AddShape(Shape shape)
        {
            shape.Parent = this;
            this._shapes.Add(shape);
            this._bounds = this.CalculateBounds();
        }

        public void AddShapes(List<Shape> shapes)
        {
            foreach (var shape in shapes)
            {
                shape.Parent = this;
                this._shapes.Add(shape);
            }
            this._bounds = this.CalculateBounds();
        }


        public override List<Intersection> LocalIntersect(Ray r)
        {
            var xs = new List<Intersection>();

            if (!this.GetBounds().Intersects(r))
                return xs;

            xs = this._shapes.SelectMany(x => x.Intersect(r)).ToList();
            return xs;
        }

        public override Vector LocalNormalAt(Point local_point, Intersection hit = null)
        {
            throw new NotImplementedException();
        }


        private BoundingBox _bounds = new BoundingBox();

        public override BoundingBox GetBounds() =>
            this._bounds;

        private BoundingBox CalculateBounds()
        {
            var box = new BoundingBox();

            foreach (Shape s in this._shapes)
            {
                var cbox = s.GetParentSpaceBounds();
                box.Add(cbox);
            }

            return box;
        }


        public override void Divide(int threshold) 
        { 
            if (threshold <= this._shapes.Count)
            {
                var (left, right) = PartitionChildren();
                if (left.Any()) MakeSubgroup(left);
                if (right.Any()) MakeSubgroup(right);
            }

            foreach (var shape in this._shapes)
                shape.Divide(threshold);
        }

        public (List<Shape>, List<Shape>) PartitionChildren()
        {
            var left = new List<Shape>();
            var right = new List<Shape>();

            var (leftBox, rightBox) = GetBounds().SplitBounds();
            foreach (var shape in this._shapes)
            {
                if (leftBox.Contains(shape.GetParentSpaceBounds()))
                    left.Add(shape);
                else if (rightBox.Contains(shape.GetParentSpaceBounds()))
                    right.Add(shape);
            }

            foreach (var shape in left)
                this._shapes.Remove(shape);

            foreach (var shape in right)
                this._shapes.Remove(shape);

            return (left, right);
        }

        public void MakeSubgroup(List<Shape> shapes)
        {
            var subgroup = new Group();

            foreach (var shape in shapes)
                subgroup.AddShape(shape);

            this.AddShape(subgroup);
        }
    }
}