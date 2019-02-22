using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RayTracer.Tests
{

    public class TestShape : Shape
    {
        public override List<Intersection> LocalIntersect(Ray r) { return new List<Intersection>(); }
        public override Vector LocalNormalAt(Point local_point) { return new Vector(0, 0, 0); }
    }

    public class GroupTests
    {
        [Fact]
        public void CreatingNewGroup_ShouldWork()
        {
            var g = new Group();
            Assert.True(g.Transform.Equals(new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} })));
            Assert.True(!g.GetShapes().Any());
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
            Assert.True(g.GetShapes().Any());
            Assert.True(g.GetShapes().Find(x => x.Equals(s)) != null);
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
            Assert.True(p.Equals(new Point(0, 0, -1)));
        }

        [Fact]
        public void ConvertingNormalFromObjectToWorldSpace_ShouldWork()
        {
            var g1 = new Group();
            g1.Transform = Transformation.Rotation_y(Math.PI/2);
            var g2 = new Group();
            g2.Transform = Transformation.Scaling(1,2,3);
            g1.AddShape(g2);
            var s = new Sphere();
            s.Transform = Transformation.Translation(5, 0, 0);
            g2.AddShape(s);
            var n = s.NormalToWorld(new Vector(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3));
            Assert.True(n.Equals(new Vector(0.28571, 0.42857, -0.85714)));
            //Assert.StrictEqual(n, new Vector(0.28571, 0.42857, -0.85714));
        }

        [Fact]
        public void FindingNormalOnChildObject_ShouldWork()
        {
            var g1 = new Group();
            g1.Transform = Transformation.Rotation_y(Math.PI/2);
            var g2 = new Group();
            g2.Transform = Transformation.Scaling(1, 2, 3);
            g1.AddShape(g2);
            var s = new Sphere();
            s.Transform = Transformation.Translation(5, 0, 0);
            g2.AddShape(s);
            var n = s.NormalAt(new Point(1.7321, 1.1547, -5.5774));
            Assert.True(n.Equals(new Vector(0.28570, 0.42854, -0.85716)));
            //Assert.StrictEqual(n, new Vector(0.28570, 0.42854, -0.85716));
        }
    }
}