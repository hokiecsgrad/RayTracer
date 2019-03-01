using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class GlassSphere : Sphere
    {
        public GlassSphere() 
        {
            this.Material.Transparency = 1.0;
            this.Material.RefractiveIndex = 1.5;
        }
    }

    public class MaterialTests
    {
        const double epsilon = 0.0001;

        static readonly IEqualityComparer<Color> ColorComparer =
            Color.GetEqualityComparer(epsilon);

        [Fact]
        public void Material_ShouldHaveColorAmbientDiffuseSpecularShininess()
        {
            Material m = new Material();
            Assert.Equal(Color.White, m.Color, ColorComparer);
            Assert.Equal(0.1, m.Ambient);
            Assert.Equal(0.9, m.Diffuse);
            Assert.Equal(0.9, m.Specular);
            Assert.Equal(200.0, m.Shininess);
        }

        [Fact]
        public void LightingWithEyeBetweenLightAndSurface_ShouldWork()
        {
            var s = new Sphere();
            var m = new Material();
            var position = new Point(0, 0, 0);
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));
            var result = m.Lighting(s, light, position, eye, normal);
            Assert.Equal(new Color(1.9, 1.9, 1.9), result, ColorComparer);
        }

        [Fact]
        public void LightingWithTheEyeBetweenLightAndSurfaceWithEyeOffset45Degrees_ShouldWork()
        {
            var s = new Sphere();
            var m = new Material();
            var position = new Point(0, 0, 0);
            var eye = new Vector(0, Math.Sqrt(2)/2, -Math.Sqrt(2)/2);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));
            var result = m.Lighting(s, light, position, eye, normal);
            Assert.Equal(Color.White, result, ColorComparer);
        }

        [Fact]
        public void LightingWithTheEyeOppositeSurfaceLightOffset45Degrees()
        {
            var s = new Sphere();
            var m = new Material();
            var position = new Point(0, 0, 0);
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 10, -10), new Color(1, 1, 1));
            var result = m.Lighting(s, light, position, eye, normal);
            Assert.Equal(new Color(0.7364, 0.7364, 0.7364), result, ColorComparer);
        }

        [Fact]
        public void LightingWithTheEyeInThePathOfTheReflectionVector_ShouldWork()
        {
            var s = new Sphere();
            var m = new Material();
            var position = new Point(0, 0, 0);
            var eye = new Vector(0, -Math.Sqrt(2)/2, -Math.Sqrt(2)/2);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 10, -10), new Color(1, 1, 1));
            var result = m.Lighting(s, light, position, eye, normal);
            Assert.Equal(new Color(1.6364, 1.6364, 1.6364), result, ColorComparer);
        }

        [Fact]
        public void LightingWithTheLightBehindTheSurface_ShouldWork()
        {
            var s = new Sphere();
            var m = new Material();
            var position = new Point(0, 0, 0);
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, 10), new Color(1, 1, 1));
            var result = m.Lighting(s, light, position, eye, normal);
            Assert.Equal(new Color(0.1, 0.1, 0.1), result, ColorComparer);
        }

        [Fact]
        public void LightingWithTheSurfaceInShadow_ShouldWork()
        {
            var s = new Sphere();
            var m = new Material();
            var position = new Point(0, 0, 0);
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));
            var in_shadow = true;
            var result = m.Lighting(s, light, position, eye, normal, in_shadow);
            Assert.Equal(new Color(0.1, 0.1, 0.1), result, ColorComparer);
        }

        [Fact]
        public void LightingWithPatternApplied_ShouldReturnColor()
        {
            var s = new Sphere();
            var m = new Material();
            m.Pattern = new Stripe(new Color(1, 1, 1), new Color(0, 0, 0));
            m.Ambient = 1;
            m.Diffuse = 0;
            m.Specular = 0;
            var eyev = new Vector(0, 0, -1);
            var normalv = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));
            var c1 = m.Lighting(s, light, new Point(0.9, 0, 0), eyev, normalv, false);
            var c2 = m.Lighting(s, light, new Point(1.1, 0, 0), eyev, normalv, false);
            Assert.Equal(Color.White, c1, ColorComparer);
            Assert.Equal(Color.Black, c2, ColorComparer);
        }

        [Fact]
        public void ReflectivityForTheDefaultMaterial_ShouldExistAndDefaultTo0()
        {
            var m = new Material();
            Assert.Equal(0, m.Reflective);
        }

        [Fact]
        public void TransparencyAndRefractiveIndexForDefaultMaterial_ShouldExistAndDefaultTo0And1Respectively()
        {
            var m = new Material();
            Assert.Equal(0.0, m.Transparency);
            Assert.Equal(1.0, m.RefractiveIndex);
        }

        [Fact]
        public void HelperClassForProducingSphereWithGlassyMaterial_ShouldExist()
        {
            var s = new GlassSphere();
            Assert.True(s.Transform.Equals(new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} })));
            Assert.Equal(1.0, s.Material.Transparency);
            Assert.Equal(1.5, s.Material.RefractiveIndex);
        }
    }
}