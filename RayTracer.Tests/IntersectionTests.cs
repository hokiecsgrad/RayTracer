using System;
using Xunit;

namespace RayTracer.Tests
{
    public class IntersectionTests
    {
        [Fact]
        public void CreatingIntersection_ShouldWork()
        {
            var sphere = new Sphere();
            var intersection = new Intersection(3.5, sphere);
            Assert.True(intersection.Time == 3.5);
            Assert.True(intersection.Object == sphere);
        }

        [Fact]
        public void PrecomputingStateOfAnIntersection_ShouldWork()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(4, shape);
            var comps = i.PrepareComputations(r);
            Assert.Equal(i.Time, comps.Time);
            Assert.Equal(i.Object, comps.Object);
            Assert.True(comps.Point.Equals(new Point(0, 0, -1)));
            Assert.True(comps.Eye.Equals(new Vector(0, 0, -1)));
            Assert.True(comps.Normal.Equals(new Vector(0, 0, -1)));
        }

        [Fact]
        public void IntersectionDetectsHitOnTheOutside_ShouldWork()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(4, shape);
            var comps = i.PrepareComputations(r);
            Assert.False(comps.Inside);
        }

        [Fact]
        public void IntersectionDetectsHitOnTheInside_ShouldWork()
        {
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(1, shape);
            var comps = i.PrepareComputations(r);
            Assert.True(comps.Point.Equals(new Point(0, 0, 1)));
            Assert.True(comps.Eye.Equals(new Vector(0, 0, -1)));
            Assert.True(comps.Inside);
            Assert.True(comps.Normal.Equals(new Vector(0, 0, -1)));
        }
    }
}
