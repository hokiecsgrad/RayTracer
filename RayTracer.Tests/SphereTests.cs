using System;
using Xunit;

namespace RayTracer.Tests
{
    public class SphereTests
    {
        [Fact]
        public void CalculatingNormalOnPointOfSphereOnXAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.Normal_at(new Point(1, 0, 0));
            Assert.True(normal.Equals(new Vector(1, 0, 0)));
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnYAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.Normal_at(new Point(0, 1, 0));
            Assert.True(normal.Equals(new Vector(0, 1, 0)));
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnZAxis_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.Normal_at(new Point(0, 0, 1));
            Assert.True(normal.Equals(new Vector(0, 0, 1)));
        }

        [Fact]
        public void CalculatingNormalOnPointOfSphereOnNonAxialPoint_ShouldWork()
        {
            var sphere = new Sphere();
            var normal = sphere.Normal_at(new Point(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3));
            Assert.True(normal.Equals(new Vector(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3)));
        }

        [Fact]
        public void Normals_ShouldBeNormalizedVectors()
        {
            var sphere = new Sphere();
            var normal = sphere.Normal_at(new Point(Math.Sqrt(3)/3, Math.Sqrt(3)/3, Math.Sqrt(3)/3));
            Assert.True(normal.Equals(normal.Normalize()));
        }

        [Fact]
        public void CalculatingNormalsOnTranslatedSphere_ShouldWork()
        {
            var s = new Sphere();
            s.Transform = Transformation.Translation(0, 1, 0);
            var n = s.Normal_at(new Point(0, 1.70711, -0.70711));
            Assert.True(n.Equals(new Vector(0, 0.70711, -0.70711)));
        }

        [Fact]
        public void CalculatingNormalsOnTransformedSphere_ShouldWork()
        {
            var s = new Sphere();
            var m = Transformation.Scaling(1, 0.5, 1) * Transformation.Rotation_z(Math.PI/5);
            s.Transform = m;
            var n = s.Normal_at(new Point(0, Math.Sqrt(2)/2, -Math.Sqrt(2)/2));
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
    }
}