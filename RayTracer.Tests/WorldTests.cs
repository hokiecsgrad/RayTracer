using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class WorldTests
    {
        const double epsilon = 0.0001;

        static readonly IEqualityComparer<Color> Comparer =
            Color.GetEqualityComparer(epsilon);

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
            Assert.True(w.Lights != null);
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
            var comps = i.PrepareComputations(r, new List<Intersection>());
            var c = w.ShadeHit(comps);
            Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c, Comparer);
        }

        [Fact]
        public void ShadingAnIntersectionFromTheInside_ShouldWork()
        {
            var w = new World();
            w.CreateDefaultWorld();
            w.Lights = new List<ILight> {new PointLight(new Point(0, 0.25, 0), new Color(1, 1, 1))};
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = w.Shapes[1];
            var i = new Intersection(0.5, shape);
            var comps = i.PrepareComputations(r, new List<Intersection>());
            var c = w.ShadeHit(comps);
            Assert.Equal(new Color(0.90498, 0.90498, 0.90498), c, Comparer);
        }     

        [Fact]
        public void TheColorWhenRayMisses_ShouldBeBlack()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 1, 0));
            var c = w.ColorAt(r);
            Assert.Equal(Color.Black, c, Comparer);
        }

        [Fact]
        public void TheColorWhenRayHits_ShouldBeTheRightColor()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var c = w.ColorAt(r);
            Assert.Equal(new Color(0.38066, 0.47583, 0.2855), c, Comparer);
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
            Assert.Equal(inner.Material.Color, c, Comparer);
        }

        [Fact]
        public void WhenShadeHitIsGivenAnIntersectionInShadow_ShouldJustCalcAmbient()
        {
            var w = new World();
            w.Lights = new List<ILight> {new PointLight(new Point(0, 0, -10), new Color(1, 1, 1))};
            var s1 = new Sphere();
            var s2 = new Sphere();
            s2.Transform = Transformation.Translation(0, 0, 10);
            w.Shapes = new List<Shape> {s1, s2};
            var r = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            var i = new Intersection(4, s2);
            var comps = i.PrepareComputations(r, new List<Intersection>());
            var c = w.ShadeHit(comps);
            Assert.Equal(new Color(0.1, 0.1, 0.1), c, Comparer);
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
            var comps = i.PrepareComputations(r, new List<Intersection>());
            var color = w.ReflectedColor(comps);
            Assert.Equal(Color.Black, color, Comparer);
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
            var comps = i.PrepareComputations(r, new List<Intersection>());
            var color = w.ShadeHit(comps);
            Assert.Equal(new Color(0.87677, 0.92436, 0.82918), color, Comparer);
        }

        [Fact]
        public void ColorAtWithMutuallyReflectiveSurfaces_ShouldTerminateWithoutInfinteLoop()
        {
            var w = new World();
            w.Lights = new List<ILight> {new PointLight(new Point(0, 0, 0), new Color(1, 1, 1))};
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
            var comps = i.PrepareComputations(r, new List<Intersection>());
            var color = w.ReflectedColor(comps, 0);
            Assert.Equal(Color.Black, color, Comparer);
        }

        [Fact]
        public void RefractedColorWithOpaqueSurface_ShouldReturnBlack()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var shape = w.Shapes[0];
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var x0 = new Intersection(4, shape);
            var x1 = new Intersection(6, shape);
            var xs = new List<Intersection> {x0, x1};
            var comps = xs[0].PrepareComputations(r, xs);
            var c = w.RefractedColor(comps, 5);
            Assert.Equal(Color.Black, c, Comparer);
        }

        [Fact]
        public void RefractedColorAtMaximumRecursiveDepth_ShouldReturnBlack()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var shape = w.Shapes[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var x0 = new Intersection(4, shape);
            var x1 = new Intersection(6, shape);
            var xs = new List<Intersection> {x0, x1};
            var comps = xs[0].PrepareComputations(r, xs);
            var c = w.RefractedColor(comps, 0);
            Assert.Equal(Color.Black, c, Comparer);
        }

        [Fact]
        public void RefractedColorUnderTotalInternalReflection_ShouldReturnBlack()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var shape = w.Shapes[0];
            shape.Material.Transparency = 1.0;
            shape.Material.RefractiveIndex = 1.5;
            var r = new Ray(new Point(0, 0, Math.Sqrt(2)/2), new Vector(0, 1, 0));
            var x0 = new Intersection(-Math.Sqrt(2)/2, shape);
            var x1 = new Intersection(Math.Sqrt(2)/2, shape);
            var xs = new List<Intersection> {x0, x1};
            // NOTE: this time you're inside the sphere, so you need
            // to look at the second intersection, xs[1], not xs[0]
            var comps = xs[1].PrepareComputations(r, xs);
            var c = w.RefractedColor(comps, 5);
            Assert.Equal(Color.Black, c, Comparer);
        }

        [Fact]
        public void RefractedColorWithRefractedRay_ShouldReturnCorrectColor()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var A = w.Shapes[0];
            A.Material.Ambient = 1.0;
            A.Material.Pattern = new TestPattern();
            var B = w.Shapes[1];
            B.Material.Transparency = 1.0;
            B.Material.RefractiveIndex = 1.5;
            var r = new Ray(new Point(0, 0, 0.1), new Vector(0, 1, 0));
            var x0 = new Intersection(-0.9899, A);
            var x1 = new Intersection(-0.4899, B);
            var x2 = new Intersection(0.4899, B);
            var x3 = new Intersection(0.9899, A);
            var xs = new List<Intersection> { x0, x1, x2, x3 };
            var comps = xs[2].PrepareComputations(r, xs);
            var c = w.RefractedColor(comps, 5);
            Assert.Equal(new Color(0, 0.99888, 0.04725), c, Comparer);
        }

        [Fact]
        public void RefractedColorWhenCalledFromShadeHitWithTransparentMaterial_ShouldReturnColor()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var floor = new Plane();
            floor.Transform = Transformation.Translation(0, -1, 0);
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            var ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Transformation.Translation(0, -3.5, -0.5);
            w.Shapes = new List<Shape> {w.Shapes[0], w.Shapes[1], floor, ball};
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2)/2, Math.Sqrt(2)/2));
            var xs = new List<Intersection> { new Intersection(Math.Sqrt(2), floor) };
            var comps = xs[0].PrepareComputations(r, xs);
            var color = w.ShadeHit(comps, 5);
            Assert.Equal(new Color(0.93642, 0.68642, 0.68642), color, Comparer);
        }

        [Theory]
        [MemberData(nameof(GetLightOcclusionData))]
        public void IsShadowMethod_ShouldTestForOcclusionBetweenTwoPoints(Point point, bool result)
        {
            var w = new World();
            w.CreateDefaultWorld();
            var light_position = new Point(-10, -10, -10);
            Assert.Equal(result, w.IsShadowed(light_position, point));
        }

        public static IEnumerable<object[]> GetLightOcclusionData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-10, -10, 10), false },
                new object[] { new Point(10, 10, 10), true },
                new object[] { new Point(-20, -20, -20), false },
                new object[] { new Point(-5, -5, -5), false },
            };

            return allData;
        }
    }
}