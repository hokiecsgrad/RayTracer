using System;
using Xunit;

namespace RayTracer.Tests
{
    public class RayTests
    {
        [Fact]
        public void CreatingAndQueryingRay_ShouldWork()
        {
            var origin = new Point(1, 2, 3);
            var direction = new Vector(4, 5, 6);
            var ray = new Ray(origin, direction);
            Assert.True(ray.Origin == origin);
            Assert.True(ray.Direction == direction);
        }

        [Fact]
        public void ComputingPointFromADistance_ShouldWork()
        {
            var ray = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));
            Assert.True(ray.Position(0).Equals(new Point(2, 3, 4)));
            Assert.True(ray.Position(1).Equals(new Point(3, 3, 4)));
            Assert.True(ray.Position(-1).Equals(new Point(1, 3, 4)));
            Assert.True(ray.Position(2.5).Equals(new Point(4.5, 3, 4)));
        }

        [Fact]
        public void RayPassesThroughSphere_ShouldIntersectAtTwoPoints()
        {
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            Intersection[] xs = ray.Intersect(sphere);
            Assert.Equal(2, xs.Length);
            Assert.Equal(4.0, xs[0].Time);
            Assert.Equal(6.0, xs[1].Time);
        }

        [Fact]
        public void RayPassesSphereAtTangent_ShouldIntersectAtOnePoint()
        {
            Ray ray = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            Intersection[] xs = ray.Intersect(sphere);
            Assert.Equal(2, xs.Length);
            Assert.Equal(5.0, xs[0].Time);
            Assert.Equal(5.0, xs[1].Time);
        }

        [Fact]
        public void RayMissesSphereEntirely_ShouldNotIntersect()
        {
            Ray ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            Intersection[] xs = ray.Intersect(sphere);
            Assert.Equal(0, xs.Length);
        }

        [Fact]
        public void RayOriginatesInsideSphere_ShouldIntersectAtTwoPoints()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            Intersection[] xs = ray.Intersect(sphere);
            Assert.Equal(2, xs.Length);
            Assert.Equal(-1.0, xs[0].Time);
            Assert.Equal(1.0, xs[1].Time);
        }

        [Fact]
        public void RayIsCompletelyInFrontOfSphere_ShouldIntersectAtTwoPoints()
        {
            Ray ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            Intersection[] xs = ray.Intersect(sphere);
            Assert.Equal(2, xs.Length);
            Assert.Equal(-6.0, xs[0].Time);
            Assert.Equal(-4.0, xs[1].Time);
        }

        [Fact]
        public void HitWhenAllIntersectionsHavePositiveT_ShouldBeSmallestValue()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            Intersection[] xs = new Intersection[] { new Intersection(1, sphere), new Intersection(2, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Equal(new Intersection(1, sphere), hit);
        }

        [Fact]
        public void HitWhenSomeIntersectionsHaveNegativeT_ShouldReturnSmallestPositiveValue()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            Intersection[] xs = new Intersection[] { new Intersection(-1, sphere), new Intersection(1, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Equal(new Intersection(1, sphere), hit);
        }

        [Fact]
        public void HitWhenAllIntersectionsHaveNegativeT_ShouldReturnNothing()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            Intersection[] xs = new Intersection[] { new Intersection(-2, sphere), new Intersection(-1, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Equal(null, hit);
        }

        [Fact]
        public void HitWithListOfIntersections_ShouldAlwaysBeLowestPositiveNumber()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            Intersection[] xs = new Intersection[] { new Intersection(5, sphere), new Intersection(7, sphere), new Intersection(-3, sphere), new Intersection(2, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Equal(new Intersection(2, sphere), hit);
        }
    }
}