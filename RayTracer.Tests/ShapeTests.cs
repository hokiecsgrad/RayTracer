using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RayTracer.Tests
{
    public class ShapeTests
    {
        const double epsilon = 0.00001;

        static readonly IEqualityComparer<Vector> VectorComparer =
            Vector.GetEqualityComparer(epsilon);

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnXAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(1, 0, 0));
            Assert.Equal(new Vector(1, 0, 0), normal, VectorComparer);
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnYAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(0, 1, 0));
            Assert.Equal(new Vector(0, 1, 0), normal, VectorComparer);
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnZAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(0, 0, 1));
            Assert.Equal(new Vector(0, 0, 1), normal, VectorComparer);
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnNonAxialPoint_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3));
            Assert.Equal(new Vector(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3), normal, VectorComparer);
        }

        [Fact]
        public void Normals_ShouldBeNormalizedVectors()
        {
            var sphere = new Sphere();
            var normal = sphere.NormalAt(new Point(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3));
            Assert.Equal(normal, normal.Normalize(), VectorComparer);
        }

        [Fact]
        public void CalculatingNormalsOnTranslatedSphere_ShouldWork()
        {
            var s = new Sphere();
            s.Transform = Transformation.Translation(0, 1, 0);
            var n = s.NormalAt(new Point(0, 1.70711, -0.70711));
            Assert.Equal(new Vector(0, 0.70711, -0.70711), n, VectorComparer);
        }

        [Fact]
        public void CalculatingNormalsOnTransformedSphere_ShouldWork()
        {
            var s = new Sphere();
            var m = Transformation.Scaling(1, 0.5, 1) * Transformation.Rotation_z(Math.PI/5);
            s.Transform = m;
            var n = s.NormalAt(new Point(0, Math.Sqrt(2)/2, -Math.Sqrt(2)/2));
            Assert.Equal(new Vector(0, 0.97014, -0.24254), n, VectorComparer);
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

        [Theory]
        [MemberData(nameof(GetMissingCylinderData))]
        public void RayMissesCylinder_ShouldReturnNoIntersections(Point origin, Vector direction)
        {
            var cyl = new Cylinder();
            var dir = direction.Normalize();
            var r = new Ray(origin, direction);
            var xs = cyl.LocalIntersect(r);
            Assert.Equal(0, xs.Count);
        }

        public static IEnumerable<object[]> GetMissingCylinderData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(1, 0, 0), new Vector(0, 1, 0) },
                new object[] { new Point(0, 0, 0), new Vector(0, 1, 0) },
                new object[] { new Point(0, 0, -5), new Vector(1, 1, 1) },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCylinderData))]
        public void RayStrikesCylinder_ShouldWork(Point origin, Vector direction, double t0, double t1)
        {
            var cyl = new Cylinder();
            var dir = direction.Normalize();
            var r = new Ray(origin, dir);
            var xs = cyl.LocalIntersect(r);
            Assert.Equal(2, xs.Count);
            Assert.Equal(t0, xs[0].Time, 4);
            Assert.Equal(t1, xs[1].Time, 4);
        }

        public static IEnumerable<object[]> GetCylinderData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(1, 0, -5), new Vector(0, 0, 1), 5, 5 },
                new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 4, 6 },
                new object[] { new Point(0.5, 0, -5), new Vector(0.1, 1, 1), 6.80798, 7.08872 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCylinderNormalData))]
        public void NormalOnSurfaceOfCylinder_ShouldWork(Point point, Vector normal)
        {
            var cyl = new Cylinder();
            var n = cyl.LocalNormalAt(point);
            Assert.True(n.Equals(normal));
        }

        public static IEnumerable<object[]> GetCylinderNormalData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(1, 0, 0), new Vector(1, 0, 0) },
                new object[] { new Point(0, 5, -1), new Vector(0, 0, -1) },
                new object[] { new Point(0, -2, 1), new Vector(0, 0, 1) },
                new object[] { new Point(-1, 1, 0), new Vector(-1, 0, 0) },
            };

            return allData;
        }

        [Fact]
        public void DefaultMinimumAndMaximumForCylinders_ShouldBeInfinity()
        {
            var cyl = new Cylinder();
            Assert.True(double.IsInfinity(cyl.Minimum));
            Assert.True(double.IsInfinity(cyl.Maximum));
        }

        [Theory]
        [MemberData(nameof(GetCylinderIntersectData))]
        public void IntersectingConstrainedCylinder_ShouldWork(Point point, Vector direction, int count)
        {
            var cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            var dir = direction.Normalize();
            var r = new Ray(point, dir);
            var xs = cyl.LocalIntersect(r);
            Assert.Equal(count, xs.Count);
        }

        public static IEnumerable<object[]> GetCylinderIntersectData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 1.5, 0), new Vector(0.1, 1, 0), 0 },
                new object[] { new Point(0, 3, -5), new Vector(0, 0, 1), 0 },
                new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 0 },
                new object[] { new Point(0, 2, -5), new Vector(0, 0, 1), 0 },
                new object[] { new Point(0, 1, -5), new Vector(0, 0, 1), 0 },
                new object[] { new Point(0, 1.5, -2), new Vector(0, 0, 1), 2 },
            };

            return allData;
        }

        [Fact]
        public void DefaultClosedValueForCylinder_ShouldBeFalse()
        {
            var cyl = new Cylinder();
            Assert.False(cyl.Closed);
        }

        [Theory]
        [MemberData(nameof(GetCylinderCappedData))]
        public void IntersectingTheCapsOfClosedCylinder_ShouldWork(Point point, Vector direction, int count)
        {
            var cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            cyl.Closed = true;
            var dir = direction.Normalize();
            var r = new Ray(point, dir);
            var xs = cyl.LocalIntersect(r);
            Assert.Equal(count, xs.Count);
        }

        public static IEnumerable<object[]> GetCylinderCappedData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 3, 0), new Vector(0, -1, 0), 2 },
                new object[] { new Point(0, 3, -2), new Vector(0, -1, 2), 2 },
                new object[] { new Point(0, 4, -2), new Vector(0, -1, 1), 2 }, // corner case
                new object[] { new Point(0, 0, -2), new Vector(0, 1, 2), 2 },
                new object[] { new Point(0, -1, -2), new Vector(0, 1, 1), 2 }, // corner case
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCylinderCappedNormalData))]
        public void NormalVectorOnCylindersEndCaps_ShouldWorkLikePlanes(Point point, Vector normal)
        {
            var cyl = new Cylinder();
            cyl.Minimum = 1;
            cyl.Maximum = 2;
            cyl.Closed = true;
            var n = cyl.LocalNormalAt(point);
            Assert.True(n.Equals(normal));
        }

        public static IEnumerable<object[]> GetCylinderCappedNormalData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 1, 0), new Vector(0, -1, 0) },
                new object[] { new Point(0.5, 1, 0), new Vector(0, -1, 0) },
                new object[] { new Point(0, 1, 0.5), new Vector(0, -1, 0) },
                new object[] { new Point(0, 2, 0), new Vector(0, 1, 0) },
                new object[] { new Point(0.5, 2, 0), new Vector(0, 1, 0) },
                new object[] { new Point(0, 2, 0.5), new Vector(0, 1, 0) },       
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetConeIntersectData))]
        public void IntersectingConeWithRay_ShouldWork(Point origin, Vector direction, double t0, double t1)
        {
            var shape = new Cone();
            var dir = direction.Normalize();
            var r = new Ray(origin, dir);
            var xs = shape.LocalIntersect(r);
            Assert.Equal(2, xs.Count);
            Assert.Equal(t0, xs[0].Time, 5);
            Assert.Equal(t1, xs[1].Time, 5);
        }

        public static IEnumerable<object[]> GetConeIntersectData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 0, -5), new Vector(0, 0, 1), 5, 5 },
                new object[] { new Point(0, 0, -5), new Vector(1, 1, 1), 8.66025, 8.66025 },
                new object[] { new Point(1, 1, -5), new Vector(-0.5, -1, 1), 4.55006, 49.44994 },
            };

            return allData;
        }

        [Fact]
        public void IntersectingConeWithRayParallelToOneOfItsHalves_ShouldWork()
        {
            var shape = new Cone();
            var direction = new Vector(0, 1, 1).Normalize();
            var r = new Ray(new Point(0, 0, -1), direction);
            var xs = shape.LocalIntersect(r);
            Assert.Equal(1, xs.Count);
            Assert.Equal(0.35355, xs[0].Time, 5);
        }

        [Theory]
        [MemberData(nameof(GetConeCapIntersectData))]
        public void IntersectingConeEndCaps_ShouldWork(Point origin, Vector direction, double count)
        {
            var shape = new Cone();
            shape.Minimum = -0.5;
            shape.Maximum = 0.5;
            shape.Closed = true;
            var dir = direction.Normalize();
            var r = new Ray(origin, dir);
            var xs = shape.LocalIntersect(r);
            Assert.Equal(count, xs.Count);
        }

        public static IEnumerable<object[]> GetConeCapIntersectData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 0, -5), new Vector(0, 1, 0), 0 },
                new object[] { new Point(0, 0, -0.25), new Vector(0, 1, 1), 2 },
                new object[] { new Point(0, 0, -0.25), new Vector(0, 1, 0), 4 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetConeNormalData))]
        public void ComputingNormalVectorOfCone_ShouldWork(Point point, Vector normal)
        {
            var shape = new Cone();
            var n = shape.LocalNormalAt(point);
            Assert.True(n.Equals(normal));
        }

        public static IEnumerable<object[]> GetConeNormalData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 0, 0), new Vector(0, 0, 0) },
                new object[] { new Point(1, 1, 1), new Vector(1, -Math.Sqrt(2), 1) },
                new object[] { new Point(-1, -1, 0), new Vector(-1, 1, 0) },
            };

            return allData;
        }

        [Fact]
        public void ConstructingSmoothTriangle()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);
            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);
            Assert.Equal(p1, tri.p1);
            Assert.Equal(p2, tri.p2);
            Assert.Equal(p3, tri.p3);
            Assert.Equal(n1, tri.n1);
            Assert.Equal(n2, tri.n2);
            Assert.Equal(n3, tri.n3);
        }

        [Fact]
        public void IntersectionWithSmoothTriangle_ShouldStoresUv()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);
            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);
            var r = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));
            var xs = tri.LocalIntersect(r);
            Assert.Equal(0.45, xs[0].u, 2);
            Assert.Equal(0.25, xs[0].v, 2);
        }

        [Fact]
        public void SmoothTriangle_ShouldUseUvToInterpolateTheNormal()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);
            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);
            var i = new Intersection(1, tri, 0.45, 0.25);
            var n = tri.NormalAt(new Point(0, 0, 0), i);
            Assert.Equal(new Vector(-0.5547, 0.83205, 0), n, VectorComparer);
        }

        [Fact]
        public void PreparingTheNormalOnSmoothTriangle_ShouldWork()
        {
            var p1 = new Point(0, 1, 0);
            var p2 = new Point(-1, 0, 0);
            var p3 = new Point(1, 0, 0);
            var n1 = new Vector(0, 1, 0);
            var n2 = new Vector(-1, 0, 0);
            var n3 = new Vector(1, 0, 0);
            var tri = new SmoothTriangle(p1, p2, p3, n1, n2, n3);
            var i = new Intersection(1, tri, 0.45, 0.25);
            var r = new Ray(new Point(-0.2, 0.3, -2), new Vector(0, 0, 1));
            var xs = new List<Intersection> { i };
            var comps = i.PrepareComputations(r, xs);
            Assert.Equal(new Vector(-0.5547, 0.83205, 0), comps.Normal, VectorComparer);
        }
    }
}