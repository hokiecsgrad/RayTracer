using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RayTracer.Tests
{
    public class RayTests
    {
        const double epsilon = 0.00001;

        static readonly IEqualityComparer<Point> PointComparer =
            Point.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Vector> VectorComparer =
            Vector.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Matrix> MatrixComparer =
            Matrix.GetEqualityComparer(epsilon);

        [Fact]
        public void CreatingAndQueryingRay_ShouldWork()
        {
            var origin = new Point(1, 2, 3);
            var direction = new Vector(4, 5, 6);
            var ray = new Ray(origin, direction);
            Assert.Equal(ray.Origin, origin, PointComparer);
            Assert.Equal(ray.Direction, direction, VectorComparer);
        }

        [Fact]
        public void ComputingPointFromADistance_ShouldWork()
        {
            var ray = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));
            Assert.Equal(new Point(2, 3, 4), ray.Position(0), PointComparer);
            Assert.Equal(new Point(3, 3, 4), ray.Position(1), PointComparer);
            Assert.Equal(new Point(1, 3, 4), ray.Position(-1), PointComparer);
            Assert.Equal(new Point(4.5, 3, 4), ray.Position(2.5), PointComparer);
        }

        [Fact]
        public void RayPassesThroughSphere_ShouldIntersectAtTwoPoints()
        {
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            List<Intersection> xs = sphere.Intersect(ray);
            Assert.Equal(2, xs.Count);
            Assert.Equal(4.0, xs[0].Time);
            Assert.Equal(6.0, xs[1].Time);
        }

        [Fact]
        public void RayPassesSphereAtTangent_ShouldIntersectAtOnePoint()
        {
            Ray ray = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            List<Intersection> xs = sphere.Intersect(ray);
            Assert.Equal(2, xs.Count);
            Assert.Equal(5.0, xs[0].Time);
            Assert.Equal(5.0, xs[1].Time);
        }

        [Fact]
        public void RayMissesSphereEntirely_ShouldNotIntersect()
        {
            Ray ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            List<Intersection> xs = sphere.Intersect(ray);
            Assert.Empty(xs);
        }

        [Fact]
        public void RayOriginatesInsideSphere_ShouldIntersectAtTwoPoints()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            List<Intersection> xs = sphere.Intersect(ray);
            Assert.Equal(2, xs.Count);
            Assert.Equal(-1.0, xs[0].Time);
            Assert.Equal(1.0, xs[1].Time);
        }

        [Fact]
        public void RayIsCompletelyInFrontOfSphere_ShouldIntersectAtTwoPoints()
        {
            Ray ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            Sphere sphere = new Sphere();
            List<Intersection> xs = sphere.Intersect(ray);
            Assert.Equal(2, xs.Count);
            Assert.Equal(-6.0, xs[0].Time);
            Assert.Equal(-4.0, xs[1].Time);
        }

        [Fact]
        public void HitWhenAllIntersectionsHavePositiveT_ShouldBeSmallestValue()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            List<Intersection> xs = new List<Intersection> { new Intersection(1, sphere), new Intersection(2, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Equal(new Intersection(1, sphere), hit);
        }

        [Fact]
        public void HitWhenSomeIntersectionsHaveNegativeT_ShouldReturnSmallestPositiveValue()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            List<Intersection> xs = new List<Intersection> { new Intersection(-1, sphere), new Intersection(1, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Equal(new Intersection(1, sphere), hit);
        }

        [Fact]
        public void HitWhenAllIntersectionsHaveNegativeT_ShouldReturnNothing()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            List<Intersection> xs = new List<Intersection> { new Intersection(-2, sphere), new Intersection(-1, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Null(hit);
        }

        [Fact]
        public void HitWithListOfIntersections_ShouldAlwaysBeLowestPositiveNumber()
        {
            Ray ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 0));
            Sphere sphere = new Sphere();
            List<Intersection> xs = new List<Intersection> { new Intersection(5, sphere), new Intersection(7, sphere), new Intersection(-3, sphere), new Intersection(2, sphere) };
            Intersection hit = ray.Hit(xs);
            Assert.Equal(new Intersection(2, sphere), hit);
        }

        [Fact]
        public void TranslatingARay_ShouldWork()
        {
            Ray ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            var transform = Transformation.Translation(3, 4, 5);
            var ray2 = ray.Transform(transform);
            Assert.Equal(new Point(4, 6, 8), ray2.Origin, PointComparer);
            Assert.Equal(new Vector(0, 1, 0), ray2.Direction, VectorComparer);
        }

        [Fact]
        public void ScalingARay_ShouldWork()
        {
            Ray ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            var transform = Transformation.Scaling(2, 3, 4);
            var ray2 = ray.Transform(transform);
            Assert.Equal(new Point(2, 6, 12), ray2.Origin, PointComparer);
            Assert.Equal(new Vector(0, 3, 0), ray2.Direction, VectorComparer);
        }

        [Fact]
        public void SphereDefaultTransformation_ShouldBeIdentityMatrix()
        {
            Sphere s = new Sphere();
            Assert.Equal(Matrix.Identity, s.Transform, MatrixComparer);
        }

        [Fact]
        public void ChangingSphereDefaultTransformation_ShouldWork()
        {
            Sphere s = new Sphere();
            var transform = Transformation.Translation(2, 3, 4);
            s.Transform = transform;
            Assert.True(s.Transform.Equals(transform));
        }

        [Fact]
        public void IntersectingScaledSphereWithRay_ShouldWork()
        {
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere s = new Sphere();
            s.Transform = Transformation.Scaling(2, 2, 2);
            List<Intersection> xs = s.Intersect(ray);
            Assert.Equal(2, xs.Count);
            Assert.Equal(3, xs[0].Time);
            Assert.Equal(7, xs[1].Time);
        }

        [Fact]
        public void IntersectingTranslatedSphereWithRay_ShouldWork()
        {
            Ray ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            Sphere s = new Sphere();
            s.Transform = Transformation.Translation(5, 0, 0);
            List<Intersection> xs = s.Intersect(ray);
            Assert.Empty(xs);
        }
    }
}