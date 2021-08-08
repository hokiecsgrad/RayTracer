using System;
using Xunit;

namespace RayTracer.Tests
{
    public class TriangleTests
    {
        [Fact]
        public void ConstructingTriangle_ShouldPrecomputeEdgesAndNormal()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var t = new Triangle(p1, p2, p3);
            Assert.StrictEqual(p1, t.p1);
            Assert.StrictEqual(p2, t.p2);
            Assert.StrictEqual(p3, t.p3);
            Assert.StrictEqual(new Vector(-1, -1, 0), t.e1);
            Assert.StrictEqual(new Vector(1, -1, 0), t.e2);
            Assert.StrictEqual(new Vector(0, 0, -1), t.Normal);
        }

        [Fact]
        public void FindingTheNormalOfTriangle_ShouldBeSameForAllPointsOnTriangle()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var n1 = t.LocalNormalAt(new Point(0, 0.5, 0));
            var n2 = t.LocalNormalAt(new Point(-0.5, 0.75, 0));
            var n3 = t.LocalNormalAt(new Point(0.5, 0.25, 0));
            Assert.Equal(n1, t.Normal);
            Assert.Equal(n2, t.Normal);
            Assert.Equal(n3, t.Normal);
        }

        [Fact]
        public void IntersectingRayParallelToTheTriangle_ShouldReturnNoIntersections()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(0, -1, -2), new Vector(0, 1, 0));
            var xs = t.LocalIntersect(r);
            Assert.Empty(xs);
        }

        [Fact]
        public void RayMissesTheP1P3Edge_ShouldReturnNoIntersections()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(1, 1, -2), new Vector(0, 0, 1));
            var xs = t.LocalIntersect(r);
            Assert.Empty(xs);
        }

        [Fact]
        public void RayMissesTheP1P2Edge_ShouldReturnNoIntersections()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(-1, 1, -2), new Vector(0, 0, 1));
            var xs = t.LocalIntersect(r);
            Assert.Empty(xs);
        }

        [Fact]
        public void RayMissesTheP2P3Edge_ShouldReturnNoIntersections()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(0, -1, -2), new Vector(0, 0, 1));
            var xs = t.LocalIntersect(r);
            Assert.Empty(xs);
        }

        [Fact]
        public void RayStrikesTriangle_ShouldReturnOneIntersection()
        {
            var t = new Triangle(new Point(0, 1, 0), new Point(-1, 0, 0), new Point(1, 0, 0));
            var r = new Ray(new Point(0, 0.5, -2), new Vector(0, 0, 1));
            var xs = t.LocalIntersect(r);
            Assert.Single(xs);
            Assert.Equal(2, xs[0].Time, 4);
        }
    }
}