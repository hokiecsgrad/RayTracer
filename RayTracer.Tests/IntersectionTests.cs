using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class IntersectionTests
    {
        private const double EPSILON = 0.00001;
        
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
            var comps = i.PrepareComputations(r, new List<Intersection>());
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
            var comps = i.PrepareComputations(r, new List<Intersection>());
            Assert.False(comps.Inside);
        }

        [Fact]
        public void IntersectionDetectsHitOnTheInside_ShouldWork()
        {
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(1, shape);
            var comps = i.PrepareComputations(r, new List<Intersection>());
            Assert.True(comps.Point.Equals(new Point(0, 0, 1)));
            Assert.True(comps.Eye.Equals(new Vector(0, 0, -1)));
            Assert.True(comps.Inside);
            Assert.True(comps.Normal.Equals(new Vector(0, 0, -1)));
        }

        [Fact]
        public void WhenShadingTheHit_ShouldOffsetThePoint()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new Sphere();
            shape.Transform = Transformation.Translation(0, 0, 1);
            var i = new Intersection(5, shape);
            var comps = i.PrepareComputations(r, new List<Intersection>());
            Assert.True(comps.OverPoint.z < -EPSILON/2);
            Assert.True(comps.Point.z > comps.OverPoint.z);
        }

        [Fact]
        public void PrepareComs_ShouldPrecomputTheReflectionVector()
        {
            var shape = new Plane();
            var r = new Ray(new Point(0, 1, -1), new Vector(0, -Math.Sqrt(2)/2, Math.Sqrt(2)/2));
            var i = new Intersection(Math.Sqrt(2), shape);
            var comps = i.PrepareComputations(r, new List<Intersection>());
            Assert.True(comps.Reflect.Equals(new Vector(0, Math.Sqrt(2)/2, Math.Sqrt(2)/2)));
        }

        [Fact]
        public void FindingN1AndN2AtVariousIntersections_ShouldWork()
        {
            var A = new GlassSphere();
            A.Transform = Transformation.Scaling(2, 2, 2);
            A.Material.RefractiveIndex = 1.5;
            var B = new GlassSphere();
            B.Transform = Transformation.Translation(0, 0, -0.25);
            B.Material.RefractiveIndex = 2.0;
            var C = new GlassSphere();
            C.Transform = Transformation.Translation(0, 0, 0.25);
            C.Material.RefractiveIndex = 2.5;

            var r = new Ray(new Point(0, 0, -4), new Vector(0, 0, 1));

            var x0 = new Intersection(2, A);
            var x1 = new Intersection(2.75, B);
            var x2 = new Intersection(3.25, C);
            var x3 = new Intersection(4.75, B);
            var x4 = new Intersection(5.25, C);
            var x5 = new Intersection(6, A);

            var xs = new List<Intersection> {x0, x1, x2, x3, x4, x5};

            var comps0 = x0.PrepareComputations(r, xs);
            var comps1 = x1.PrepareComputations(r, xs);
            var comps2 = x2.PrepareComputations(r, xs);
            var comps3 = x3.PrepareComputations(r, xs);
            var comps4 = x4.PrepareComputations(r, xs);
            var comps5 = x5.PrepareComputations(r, xs);

            Assert.Equal(1.0, comps0.n1);
            Assert.Equal(1.5, comps0.n2);
            Assert.Equal(1.5, comps1.n1);
            Assert.Equal(2.0, comps1.n2);
            Assert.Equal(2.0, comps2.n1);
            Assert.Equal(2.5, comps2.n2);
            Assert.Equal(2.5, comps3.n1);
            Assert.Equal(2.5, comps3.n2);
            Assert.Equal(2.5, comps4.n1);
            Assert.Equal(1.5, comps4.n2);
            Assert.Equal(1.5, comps5.n1);
            Assert.Equal(1.0, comps5.n2);
        }

        [Fact]
        public void WhenCalculatingUnderPoint_ShouldOffsetBelowTheSurface()
        {
            var r = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var shape = new GlassSphere();
            shape.Transform = Transformation.Translation(0, 0, 1);
            var i = new Intersection(5, shape);
            var xs = new List<Intersection> { i };
            var comps = i.PrepareComputations(r, xs);
            Assert.True(comps.UnderPoint.z > EPSILON/2);
            Assert.True(comps.Point.z < comps.UnderPoint.z);
        }

        [Fact]
        public void SchlickApproximationUnderTotalInternalReflection_ShouldReturn1()
        {
            var shape = new GlassSphere();
            var r = new Ray(new Point(0, 0, Math.Sqrt(2)/2), new Vector(0, 1, 0));
            var xs = new List<Intersection> { new Intersection(-Math.Sqrt(2)/2,shape), new Intersection(Math.Sqrt(2)/2,shape) };
            var comps = xs[1].PrepareComputations(r, xs);
            Assert.Equal(1.0, comps.Schlick());
        }

        [Fact]
        public void SchlickApproximationWithPerpendicularViewingAngle_ShouldBeSmall()
        {
            var shape = new GlassSphere();
            var r = new Ray(new Point(0, 0, 0), new Vector(0, 1, 0));
            var xs = new List<Intersection> { new Intersection(-1, shape), new Intersection(1, shape) };
            var comps = xs[1].PrepareComputations(r, xs);
            Assert.Equal(0.04, comps.Schlick(), 2);
        }

        [Fact]
        public void SchlickApproximationWithSmallAngleAndN2GreaterThanN1_ShouldBeSignificant()
        {
            var shape = new GlassSphere();
            var r = new Ray(new Point(0, 0.99, -2), new Vector(0, 0, 1));
            var xs = new List<Intersection> { new Intersection(1.8589, shape) };
            var comps = xs[0].PrepareComputations(r, xs);
            Assert.Equal(0.48873, comps.Schlick(), 4);
        }

        [Fact]
        public void ShadeHitWithReflectiveAndTransparentMaterial_ShouldUseSchlickValue()
        {
            var w = new World();
            w.CreateDefaultWorld();
            var r = new Ray(new Point(0, 0, -3), new Vector(0, -Math.Sqrt(2)/2, Math.Sqrt(2)/2));
            var floor = new Plane();
            floor.Transform = Transformation.Translation(0, -1, 0);
            floor.Material.Reflective = 0.5;
            floor.Material.Transparency = 0.5;
            floor.Material.RefractiveIndex = 1.5;
            var ball = new Sphere();
            ball.Material.Color = new Color(1, 0, 0);
            ball.Material.Ambient = 0.5;
            ball.Transform = Transformation.Translation(0, -3.5, -0.5);
            w.Shapes = new List<Shape> {w.Shapes[0], w.Shapes[1], floor, ball};
            var xs = new List<Intersection> { new Intersection(Math.Sqrt(2), floor) };
            var comps = xs[0].PrepareComputations(r, xs);
            var color = w.ShadeHit(comps, 5);
            Assert.True(color.Equals(new Color(0.93391, 0.69643, 0.69243)));
        }
    }
}
