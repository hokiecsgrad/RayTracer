using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class WorldTests
    {
        [Fact]
        public void CreatingWorld_ShouldWork()
        {
            var w = new World();
            Assert.True(w != null);
        }

        [Fact]
        public void CreatingDefaultWorld_ShouldHaveLightAndTwoSpheres()
        {
            var w = new World();
            w.CreateDefaultWorld();
            Assert.True(w.Light != null);
            // w contains s1
            // w contains s2
        }

        [Fact]
        public void IntersectDefaultWorldWithRay_ShouldCreate4Intersections()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var xs = w.Intersect(r);
            Assert.Equal(4, xs.Count);
            Assert.Equal(4, xs[0].Time);
            Assert.Equal(4.5, xs[1].Time);
            Assert.Equal(5.5, xs[2].Time);
            Assert.Equal(6, xs[3].Time);
        }

        [Fact]
        public void ShadingAnIntersection_ShouldWork()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = w.Shapes[0];
            var i = new Intersection(4, shape);
            var comps = i.PrepareComputations(r);
            var c = w.ShadeHit(comps);
            Assert.True(c.Equals(new Color(0.38066, 0.47583, 0.2855)));
        }

        [Fact]
        public void ShadingAnIntersectionFromTheInside_ShouldWork()
        {
            var w = new World();
            w.CreateDefaultWorld();
            w.Light = new PointLight(new Point(0, 0.25, 0), new Color(1, 1, 1));
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = w.Shapes[1];
            var i = new Intersection(0.5, shape);
            var comps = i.PrepareComputations(r);
            var c = w.ShadeHit(comps);
            Assert.True(c.Equals(new Color(0.90498, 0.90498, 0.90498)));
        }     

        [Fact]
        public void TheColorWhenRayMisses_ShouldBeBlack()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));
            var c = w.ColorAt(r);
            Assert.True(c.Equals(new Color(0, 0, 0)));
        }

        [Fact]
        public void TheColorWhenRayHits_ShouldBeTheRightColor()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var c = w.ColorAt(r);
            Assert.True(c.Equals(new Color(0.38066, 0.47583, 0.2855)));
        }

        [Fact]
        public void TheColorWithAnIntersectionBehindTheRay_ShouldBeTheInnerColor()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var outer = w.Shapes[0];
            outer.Material.Ambient = 1;
            var inner = w.Shapes[1];
            inner.Material.Ambient = 1;
            var r = new Ray(new Point(0, 0, 0.75), new Vector(0, 0, -1));
            var c = w.ColorAt(r);
            Assert.Equal(inner.Material.Color, c);
        }

        [Fact]
        public void WhenNothingIsCollinearWithPointAndLight_ShouldBeNoShadow()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var p = new Point(0, 10, 0);
            Assert.False(w.IsShadowed(p));
        }

        [Fact]
        public void WhenAnObjectIsBetweenThePointAndTheLight_ShouldBeInShadow()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var p = new Point(10, -10, 10);
            Assert.True(w.IsShadowed(p));
        }

        [Fact]
        public void WhenAnObjectIsBehindTheLight_ShouldBeNoShadow()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var p = new Point(-20, 20, -20);
            Assert.False(w.IsShadowed(p));
        }

        [Fact]
        public void WhenAnObjectIsBehindThePoint_ShouldBeNoShadow()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var p = new Point(-2, 2, -2);
            Assert.False(w.IsShadowed(p));
        }

        [Fact]
        public void WhenShadeHitIsGivenAnIntersectionInShadow_ShouldJustCalcAmbient()
        {
            var w = new World();
            w.Light = new PointLight(new Point(0, 0, -10), new Color(1, 1, 1));
            var s1 = new Sphere();
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(0, 0, 10);
            w.Shapes = new List<Shape> {s1, s2};
            var r = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            var i = new Intersection(4, s2);
            var comps = i.PrepareComputations(r);
            var c = w.ShadeHit(comps);
            Assert.True(c.Equals(new Color(0.1, 0.1, 0.1)));
        }

        [Fact]
        public void TheReflectedColorForNonReflectiveMaterial_ShouldBeBlack()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = w.Shapes[1];
            shape.Material.Ambient = 1;
            var i = new Intersection(1, shape);
            var comps = i.PrepareComputations(r);
            var color = w.ReflectedColor(comps);
            Assert.True(color.Equals(new Color(0, 0, 0)));
        }

        [Fact]
        public void TheReflectedColorForReflectiveMaterial_ShouldBeRightColor()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Transformation.Translation(0, -1, 0);
            w.Shapes = new List<Shape> {w.Shapes[0], w.Shapes[1], shape};
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2)/2, Math.Sqrt(2)/2));
            var i = new Intersection(Math.Sqrt(2), shape);
            var comps = i.PrepareComputations(r);
            var color = w.ShadeHit(comps);
            Assert.True(color.Equals(new Color(0.87677, 0.92436, 0.82918)));
        }

        [Fact]
        public void ColorAtWithMutuallyReflectiveSurfaces_ShouldTerminateWithoutInfinteLoop()
        {
            var w = new World();
            w.Light = new PointLight(new Point(0, 0, 0), new Color(1, 1, 1));
            var lower = new Plane();
            lower.Material.Reflective = 1;
            lower.Transform = Transformation.Translation(0, -1, 0);
            var upper = new Plane();
            upper.Material.Reflective = 1;
            upper.Transform = Transformation.Translation(0, 1, 0);
            w.Shapes = new List<Shape> {lower, upper};
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));
            w.ColorAt(r);
            Assert.True(true);
        }      

        [Fact]
        public void TheReflectedColorAtMaxRecursionDepth_ShouldTerminateAsBlack()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var shape = new Plane();
            shape.Material.Reflective = 0.5;
            shape.Transform = Transformation.Translation(0, -1, 0);
            w.Shapes = new List<Shape> {w.Shapes[0], w.Shapes[1], shape};
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2)/2, Math.Sqrt(2)/2));
            var i = new Intersection(Math.Sqrt(2), shape);
            var comps = i.PrepareComputations(r);
            var color = w.ReflectedColor(comps, 0);
            Assert.True(color.Equals(new Color(0, 0, 0)));
        }
    }
}