using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RayTracer.Tests
{
    public class TestShape : Shape
    {
        public Ray SavedRay = null;

        public override List<Intersection> LocalIntersect(Ray r)
        {
            SavedRay = r;
            return new List<Intersection>();
        }

        public override Vector LocalNormalAt(Point local_point, Intersection hit = null)
        {
            return new Vector(0, 0, 0);
        }

        public override BoundingBox GetBounds()
        {
            return new BoundingBox(new Point(-1, -1, -1), new Point(1, 1, 1));
        }

        public override void Divide(int threshold) { }
    }

    public class GroupTests
    {
        const double epsilon = 0.00001;

        static readonly IEqualityComparer<Color> ColorComparer =
            Color.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Vector> VectorComparer =
            Vector.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Point> PointComparer =
            Point.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Matrix> MatrixComparer =
            Matrix.GetEqualityComparer(epsilon);

        [Fact]
        public void CreatingNewGroup_ShouldWork()
        {
            var g = new Group();
            Assert.Equal(Matrix.Identity, g.Transform, MatrixComparer);
            Assert.Equal(0, g.Count);
        }

        [Fact]
        public void Shape_ShouldHaveParentMember()
        {
            var s = new TestShape();
            Assert.True(s.Parent == null);
        }

        [Fact]
        public void AddingChildToGroup_ShouldWork()
        {
            var g = new Group();
            var s = new TestShape();
            g.AddShape(s);
            Assert.Equal(1, g.Count);
            Assert.Contains(s, g.Shapes);
            Assert.Equal(s.Parent, g);
        }

        [Fact]
        public void IntersectingRayWithEmptyGroup_ShouldReturnNoIntersections()
        {
            var g = new Group();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var xs = g.LocalIntersect(r);
            Assert.False(xs.Any());
        }

        [Fact]
        public void IntersectingRayWithNonemptyGroup_ShouldReturnSomeIntersections()
        {
            var g = new Group();
            var s1 = new Sphere();
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(0, 0, -3);
            var s3 = new Sphere();
            s3.Transform = Transformation.Translation(5, 0, 0);
            g.AddShape(s1);
            g.AddShape(s2);
            g.AddShape(s3);
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var xs = g.LocalIntersect(r);
            Assert.Equal(4, xs.Count);
            Assert.StrictEqual(xs[0].Object, s1);
            Assert.StrictEqual(xs[1].Object, s1);
            Assert.StrictEqual(xs[2].Object, s2);
            Assert.StrictEqual(xs[3].Object, s2);
        }

        [Fact]
        public void IntersectingTransformedGroup_ShouldHitSphere()
        {
            var g = new Group();
            g.Transform = Transformation.Scaling(2, 2, 2);
            var s = new Sphere();
            s.Transform = Transformation.Translation(5, 0, 0);
            g.AddShape(s);
            var r = new Ray(new Point(10, 0, -10), new Vector(0, 0, 1));
            var xs = g.Intersect(r);
            Assert.Equal(2, xs.Count);
        }

        [Fact]
        public void ConvertingPointFromWorldToObjectSpace_ShouldWork()
        {
            var g1 = new Group();
            g1.Transform = Transformation.Rotation_y(Math.PI / 2);
            var g2 = new Group();
            g2.Transform = Transformation.Scaling(2, 2, 2);
            g1.AddShape(g2);
            var s = new Sphere();
            s.Transform = Transformation.Translation(5, 0, 0);
            g2.AddShape(s);
            var p = s.ConverWorldPointToObjectPoint(new Point(-2, 0, -10));
            Assert.Equal(new Point(0, 0, -1), p, PointComparer);
        }

        [Fact]
        public void ConvertingNormalFromObjectToWorldSpace_ShouldWork()
        {
            var g1 = new Group();
            g1.Transform = Transformation.Rotation_y(Math.PI / 2);
            var g2 = new Group();
            g2.Transform = Transformation.Scaling(1, 2, 3);
            g1.AddShape(g2);
            var s = new Sphere();
            s.Transform = Transformation.Translation(5, 0, 0);
            g2.AddShape(s);
            var n = s.NormalToWorld(new Vector(Math.Sqrt(3) / 3, Math.Sqrt(3) / 3, Math.Sqrt(3) / 3));
            Assert.Equal(new Vector(0.28571, 0.42857, -0.85714), n, VectorComparer);
            //Assert.StrictEqual(n, new Vector(0.28571, 0.42857, -0.85714));
        }

        [Fact]
        public void FindingNormalOnChildObject_ShouldWork()
        {
            var g1 = new Group();
            g1.Transform = Transformation.Rotation_y(Math.PI / 2);
            var g2 = new Group();
            g2.Transform = Transformation.Scaling(1, 2, 3);
            g1.AddShape(g2);
            var s = new Sphere();
            s.Transform = Transformation.Translation(5, 0, 0);
            g2.AddShape(s);
            var n = s.NormalAt(new Point(1.7321, 1.1547, -5.5774));
            Assert.Equal(new Vector(0.28570, 0.42854, -0.85716), n, VectorComparer);
        }

        [Fact]
        public void GroupHasBoundingBoxThatContainsItsChildren_ShouldWork()
        {
            var s = new Sphere();
            s.Transform = Transformation.Translation(2, 5, -3) * Transformation.Scaling(2, 2, 2);
            var c = new Cylinder();
            c.Minimum = -2;
            c.Maximum = 2;
            c.Transform = Transformation.Translation(-4, -1, 4) * Transformation.Scaling(0.5, 1, 0.5);
            var group = new Group();
            group.AddShape(s);
            group.AddShape(c);
            var box = group.GetBounds();
            Assert.Equal(new Point(-4.5, -3, -5), box.Min, PointComparer);
            Assert.Equal(new Point(4, 7, 4.5), box.Max, PointComparer);
        }

        [Fact]
        public void GroupHasBoundingBoxThatContainsItsSubgroups_ShouldWork()
        {
            var s = new Sphere();
            s.Transform = Transformation.Translation(2, 5, -3) * Transformation.Scaling(2, 2, 2);
            var sphereGroup = new Group();
            sphereGroup.AddShape(s);
            var c = new Cylinder();
            c.Minimum = -2;
            c.Maximum = 2;
            c.Transform = Transformation.Translation(-4, -1, 4) * Transformation.Scaling(0.5, 1, 0.5);
            var cylGroup = new Group();
            cylGroup.AddShape(c);
            var group = new Group();
            group.AddShape(sphereGroup);
            group.AddShape(cylGroup);
            var box = group.GetBounds();
            Assert.Equal(new Point(-4.5, -3, -5), box.Min, PointComparer);
            Assert.Equal(new Point(4, 7, 4.5), box.Max, PointComparer);
        }

        [Fact]
        public void IntersectingGroupWithRay_ShouldNotTestChildrenIfBoxIsMissed()
        {
            var child = new TestShape();
            var group = new Group();
            group.AddShape(child);
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));
            var xs = group.Intersect(r);
            Assert.Null(child.SavedRay);
        }

        [Fact]
        public void IntersectingGroupWithRay_ShouldTestChildrenIfBoxIsHit()
        {
            var child = new TestShape();
            var group = new Group();
            group.AddShape(child);
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var xs = group.Intersect(r);
            Assert.NotNull(child.SavedRay);
        }

        [Fact]
        public void PartitioningGroupsChildren_ShouldSplitShapesInGroupIntoLeftAndRight()
        {
            var s1 = new Sphere();
            s1.Transform = Transformation.Translation(-2, 0, 0);
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(2, 0, 0);
            var s3 = new Sphere();
            var g = new Group();
            g.AddShape(s1);
            g.AddShape(s2);
            g.AddShape(s3);
            var (left, right) = g.PartitionChildren();
            Assert.Equal(1, g.Count);
            Assert.Contains(s3, g.Shapes);
            Assert.Single(left);
            Assert.Contains(s1, left);
            Assert.Single(right);
            Assert.Contains(s2, right);
        }

        [Fact]
        public void AddingListOfShapesToGroup_ShouldCreateSubGroup()
        {
            var s1 = new Sphere();
            var s2 = new Sphere();
            var g = new Group();
            g.MakeSubgroup(new List<Shape> { s1, s2 });
            var subgroup = (object)g[0] as Group;
            Assert.Equal(1, g.Count);
            Assert.Contains(s1, subgroup.Shapes);
            Assert.Contains(s2, subgroup.Shapes);
        }

        [Fact]
        public void SubdividingPrimitive_ShouldDoNothing()
        {
            var shape = new Sphere();
            shape.Divide(1);
            Assert.True(shape is Sphere);
        }

        [Fact]
        public void SubdividingGroup_ShouldPartitionItsChildren()
        {
            var s1 = new Sphere();
            s1.Transform = Transformation.Translation(-2, -2, 0);
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(-2, 2, 0);
            var s3 = new Sphere();
            s3.Transform = Transformation.Scaling(4, 4, 4);
            var g = new Group();
            g.AddShape(s1);
            g.AddShape(s2);
            g.AddShape(s3);
            g.Divide(1);
            Assert.StrictEqual(s3, g[0]);
            var subgroup = (object)g[1] as Group;
            Assert.True(subgroup is Group);
            Assert.Equal(2, subgroup.Count);
            var subgroupS1 = (object)subgroup[0] as Group;
            Assert.Equal(1, subgroupS1.Count);
            Assert.Contains(s1, subgroupS1.Shapes);
            var subgroupS2 = (object)subgroup[1] as Group;
            Assert.Equal(1, subgroupS2.Count);
            Assert.Contains(s2, subgroupS2.Shapes);
        }

        [Fact]
        public void SubdividingGroupWithTooFewChildren_ShouldNotDivideGroup()
        {
            var s1 = new Sphere();
            s1.Transform = Transformation.Translation(-2, 0, 0);
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(2, 1, 0);
            var s3 = new Sphere();
            s3.Transform = Transformation.Translation(2, -1, 0);
            var subgroup = new Group();
            subgroup.AddShape(s1);
            subgroup.AddShape(s2);
            subgroup.AddShape(s3);
            var s4 = new Sphere();
            var g = new Group();
            g.AddShape(s4);
            g.AddShape(subgroup);
            g.Divide(3);
            Assert.Equal(s4, g[0]);
            Assert.Equal(subgroup, g[1]);
            Assert.Equal(2, g.Count);
            var subgroup0 = (object)subgroup[0] as Group;
            var subgroup1 = (object)subgroup[1] as Group;
            Assert.Contains(s1, subgroup0.Shapes);
            Assert.Contains(s2, subgroup1.Shapes);
            Assert.Contains(s3, subgroup1.Shapes);
        }

        [Fact]
        public void SettingMaterialOnGroup_ShouldSetMaterialOnAllChildren()
        {
            var s1 = new Sphere();
            s1.Transform = Transformation.Translation(-2, -2, 0);
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(-2, 2, 0);
            var s3 = new Sphere();
            s3.Transform = Transformation.Scaling(4, 4, 4);
            var g = new Group();
            g.AddShape(s1);
            g.AddShape(s2);
            g.AddShape(s3);
            g.SetMaterial(new Material() { Color = new Color(1, 0, 0) });
            Assert.Equal(new Color(1, 0, 0), g[0].Material.Color, ColorComparer);
            Assert.Equal(new Color(1, 0, 0), g[1].Material.Color, ColorComparer);
            Assert.Equal(new Color(1, 0, 0), g[2].Material.Color, ColorComparer);
        }

        [Fact]
        public void SettingMaterialOnGroup_ShouldCascadeDownThroughOtherGroups()
        {
            var s1 = new Sphere();
            s1.Transform = Transformation.Translation(-2, -2, 0);
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(-2, 2, 0);
            var s3 = new Sphere();
            s3.Transform = Transformation.Scaling(4, 4, 4);
            var g = new Group();
            var sub = new Group();
            g.AddShape(s1);
            sub.AddShape(s2);
            sub.AddShape(s3);
            g.AddShape(sub);
            g.SetMaterial(new Material() { Color = new Color(1, 0, 0) });
            var subgroup = (object)g[1] as Group;
            Assert.Equal(new Color(1, 0, 0), g[0].Material.Color, ColorComparer);
            Assert.Equal(new Color(1, 0, 0), subgroup[0].Material.Color, ColorComparer);
            Assert.Equal(new Color(1, 0, 0), subgroup[1].Material.Color, ColorComparer);
        }
    }
}