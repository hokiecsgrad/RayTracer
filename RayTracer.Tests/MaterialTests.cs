using System;
using Xunit;

namespace RayTracer.Tests
{
    public class MaterialTests
    {
        [Fact]
        public void Material_ShouldHaveColorAmbientDiffuseSpecularShininess()
        {
            Material m = new Material();
            Assert.True(m.Color.Equals(new Color(1, 1, 1)));
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
            Assert.True(result.Equals(new Color(1.9, 1.9, 1.9)));
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
            Assert.True(result.Equals(new Color(1.0, 1.0, 1.0)));
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
            Assert.True(result.Equals(new Color(0.7364, 0.7364, 0.7364)));
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
            Assert.True(result.Equals(new Color(1.6364, 1.6364, 1.6364)));
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
            Assert.True(result.Equals(new Color(0.1, 0.1, 0.1)));
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
            Assert.True(result.Equals(new Color(0.1, 0.1, 0.1)));
        }

        [Fact]
        public void LightingWithPatternApplied_ShouldReturnColor()
        {
            var s = new Sphere();
            var m = new Material();
            m.Pattern = new StripePattern(new Color(1, 1, 1), new Color(0, 0, 0));
            m.Ambient = 1;
            m.Diffuse = 0;
            m.Specular = 0;
            var eyev = new Vector(0, 0, -1);
            var normalv = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));
            var c1 = m.Lighting(s, light, new Point(0.9, 0, 0), eyev, normalv, false);
            var c2 = m.Lighting(s, light, new Point(1.1, 0, 0), eyev, normalv, false);
            Assert.True(c1.Equals(new Color(1, 1, 1)));
            Assert.True(c2.Equals(new Color(0, 0, 0)));
        }
    }
}