using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RayTracer.Tests
{
    public class ShapeTests
    {
        [Fact]
        public void CalculatingNormalOnPointOfSphereOnXAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(1, 0, 0));
            Assert.True(normal.Equals(new Vector(1, 0, 0)));
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnYAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(0, 1, 0));
            Assert.True(normal.Equals(new Vector(0, 1, 0)));
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnZAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(0, 0, 1));
            Assert.True(normal.Equals(new Vector(0, 0, 1)));
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnNonAxialPoint_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3));
            Assert.True(normal.Equals(new Vector(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3)));
        }

        [Fact]
        public void Normals_ShouldBeNormalizedVectors()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3));
            Assert.True(normal.Equals(normal.Normalize()));
        }

        [Fact]
        public void CalculatingNormalsOnTranslatedSphere_ShouldWork()
        {
            var s = new Sphere();
            s.Transform = Transformation.Translation(0, 1, 0);
            var n = s.NormalAt(new Point(0, 1.70711, -0.70711));
            Assert.True(n.Equals(new Vector(0, 0.70711, -0.70711)));
        }

        [Fact]
        public void CalculatingNormalsOnTransformedSphere_ShouldWork()
        {
            var s = new Sphere();
            var m = Transformation.Scaling(1, 0.5, 1) * Transformation.Rotation_z(Math.PI/5);
            s.Transform = m;
            var n = s.NormalAt(new Point(0, Math.Sqrt(2)/2, -Math.Sqrt(2)/2));
            Assert.True(n.Equals(new Vector(0, 0.97014, -0.24254)));
        }

        [Fact]
        public void Sphere_ShouldHaveDefaultMaterial()
        {
            var s = new Sphere();
            Assert.True(s.Material.Equals(new Material()));
        }

        [Fact]
        public void AssigningMaterialToSphere_ShouldWork()
        {
            var s = new Sphere();
            var m = new Material();
            m.Ambient = 1;
            s.Material = m;
            Assert.Equal(1, s.Material.Ambient);
            Assert.True(m.Equals(s.Material));
        }

        [Fact]
        public void NormalOfPlane_ShouldBeConstantEverywhere()
        {
            var p = new Plane();
            var n1 = p.NormalAt(new Point(0, 0, 0));
            var n2 = p.NormalAt(new Point(10, 0, -10));
            var n3 = p.NormalAt(new Point(-5, 0, 150));
            Assert.True(n1.Equals(new Vector(0, 1, 0)));
            Assert.True(n2.Equals(new Vector(0, 1, 0)));
            Assert.True(n3.Equals(new Vector(0, 1, 0)));
        }

        [Fact]
        public void RayParallelToPlane_ShouldNotIntersect()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, 10, 0), new Vector(0, 0, 1));
            var xs = p.Intersect(r);
            Assert.True(!xs.Any());
        }

        [Fact]
        public void RayCoplanarToPlane_ShouldNotIntersect()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var xs = p.Intersect(r);
            Assert.True(!xs.Any());
        }

        [Fact]
        public void RayIntersectingPlaneFromAbove_ShouldIntersect()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, 1, 0), new Vector(0, -1, 0));
            var xs = p.Intersect(r);
            Assert.Equal(1, xs.Count);
            Assert.Equal(1, xs[0].Time);
            Assert.True(xs[0].Object == p);
        }

        [Fact]
        public void RayIntersectingPlaneFromBelow_ShouldIntersect()
        {
            var p = new Plane();
            var r = new Ray(new Point(0, -1, 0), new Vector(0, 1, 0));
            var xs = p.Intersect(r);
            Assert.Equal(1, xs.Count);
            Assert.Equal(1, xs[0].Time);
            Assert.True(xs[0].Object == p);
        }

        [Theory]
        [MemberData(nameof(GetIntersectingCubeData))]
        public void RayIntersectsCube_ShouldReturnTwoIntersections(Point origin, Vector direction, double t1, double t2)
        {
            var c = new Cube();
            var r = new Ray(origin, direction);
            var xs = c.LocalIntersect(r);
            Assert.Equal(t1, xs[0].Time, 3);
            Assert.Equal(t2, xs[1].Time, 3);
        }

        public static IEnumerable<object[]> GetIntersectingCubeData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(5, 0.5, 0), new Vector(-1, 0, 0), 4, 6 },
                new object[] { new Point(-5, 0.5, 0), new Vector(1, 0, 0), 4, 6 },
                new object[] { new Point(0.5, 5, 0), new Vector(0, -1, 0), 4, 6 },
                new object[] { new Point(0.5, -5, 0), new Vector(0, 1, 0), 4, 6 },
                new object[] { new Point(0.5, 0, 5), new Vector(0, 0, -1), 4, 6 },
                new object[] { new Point(0.5, 0, -5), new Vector(0, 0, 1), 4, 6},
                new object[] { new Point(0, 0.5, 0), new Vector(0, 0, 1), -1, 1 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetMissingCubeData))]
        public void RayMissesCube_ShouldReturnNoIntersections(Point origin, Vector direction)
        {
            var c = new Cube();
            var r = new Ray(origin, direction);
            var xs = c.LocalIntersect(r);
            Assert.Equal(0, xs.Count);
        }

        public static IEnumerable<object[]> GetMissingCubeData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-2, 0, 0), new Vector(0.2673, 0.5345, 0.8018) },
                new object[] { new Point(0, -2, 0), new Vector(0.8018, 0.2673, 0.5345) },
                new object[] { new Point(0, 0, -2), new Vector(0.5345, 0.8018, 0.2673) },
                new object[] { new Point(2, 0, 2), new Vector(0, 0, -1) }, 
                new object[] { new Point(0, 2, 2), new Vector(0, -1, 0) },
                new object[] { new Point(2, 2, 0), new Vector(-1, 0, 0) },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetNormalCubeData))]
        public void NormalOnTheSurfaceOfCube_ShouldWork(Point point, Vector expected)
        {
            var c = new Cube();
            var normal = c.LocalNormalAt(point);
            Assert.True(normal.Equals(expected));
        }

        public static IEnumerable<object[]> GetNormalCubeData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(1, 0.5, -0.8), new Vector(1, 0, 0) },
                new object[] { new Point(-1, -0.2, 0.9), new Vector(-1, 0, 0) },
                new object[] { new Point(-0.4, 1, -0.1), new Vector(0, 1, 0) },
                new object[] { new Point(0.3, -1, -0.7), new Vector(0, -1, 0) },
                new object[] { new Point(-0.6, 0.3, 1), new Vector(0, 0, 1) },
                new object[] { new Point(0.4, 0.4, -1), new Vector(0, 0, -1) },
                new object[] { new Point(1, 1, 1), new Vector(1, 0, 0) },
                new object[] { new Point(-1, -1, -1), new Vector(-1, 0, 0) },
            };

            return allData;
        }
    }
}