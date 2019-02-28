using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class Group : Shape
    {
        private const double EPSILON = 0.00001;
        private List<Shape> Shapes = new List<Shape>();
        private BoundingBox Bounds = new BoundingBox();
        public string Name = string.Empty;

        public void AddShape(Shape shape)
        {
            shape.Parent = this;
            this.Shapes.Add(shape);
            this.Bounds = this.CalculateBounds();
        }

        public void AddTriangles(List<Triangle> shapes)
        {
            foreach (var shape in shapes)
            {
                shape.Parent = this;
                this.Shapes.Add(shape);
            }
            this.Bounds = this.CalculateBounds();
        }

        public void AddGroups(List<Group> groups)
        {
            foreach (var group in groups)
            {
                group.Parent = this;
                this.Shapes.Add(group);
            }
            this.Bounds = this.CalculateBounds();
        }

        public (List<Shape>, List<Shape>) PartitionChildren()
        {
            var left = new List<Shape>();
            var right = new List<Shape>();

            var (leftBox, rightBox) = GetBounds().SplitBounds();
            foreach (var shape in Shapes)
            {
                if (leftBox.Contains(shape.GetParentSpaceBounds()))
                    left.Add(shape);
                else if (rightBox.Contains(shape.GetParentSpaceBounds()))
                    right.Add(shape);
            }

            foreach (var shape in left)
                Shapes.Remove(shape);
            foreach (var shape in right)
                Shapes.Remove(shape);

            return (left, right);
        }

        public void MakeSubgroup(List<Shape> shapes)
        {
            var subgroup = new Group();
            foreach (var shape in shapes)
                subgroup.AddShape(shape);
            AddGroups(new List<Group> {subgroup});
        }

        public List<Shape> GetShapes() { return this.Shapes; }

        public override List<Intersection> LocalIntersect(Ray r)
        {
            var xs = new List<Intersection>();
            if (GetBounds().Intersects(r))
                foreach (var shape in this.Shapes)
                    xs.AddRange(shape.Intersect(r));
            return xs;
        }

        public override Vector LocalNormalAt(Point local_point)
        {
            // TODO: Throw an exception here since this should never be called.
            return new Vector(0, 0, 0);
        }

        public override BoundingBox GetBounds()
        {
            return this.Bounds;
        }

        private BoundingBox CalculateBounds()
        {
            var box = new BoundingBox();

            foreach (Shape s in this.Shapes)
            {
                var cbox = s.GetParentSpaceBounds();
                box.Add(cbox);
            }

            return box;
        }

        public override void Divide(int threshold) 
        { 
            if (threshold <= Shapes.Count)
            {
                var (left, right) = PartitionChildren();
                if (left.Any()) MakeSubgroup(left);
                if (right.Any()) MakeSubgroup(right);
            }

            foreach (var shape in Shapes)
                shape.Divide(threshold);
        }
    }
}