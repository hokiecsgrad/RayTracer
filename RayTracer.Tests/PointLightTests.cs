using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class PointLightTests
    {
        const double epsilon = 0.0001;

        static readonly IEqualityComparer<Point> PointComparer =
            Point.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Vector> VectorComparer =
            Vector.GetEqualityComparer(epsilon);

        [Fact]
        public void PointLight_ShouldHavePositionAndIntensity()
        {
            var color = new Color(1, 1, 1);
            var position = new Point(0, 0, 0);
            var light = new PointLight(position, color);
            Assert.True(light.Position.Equals(position));
            Assert.True(light.Color.Equals(color));
        }

        [Theory]
        [MemberData(nameof(GetLightIntensityData))]
        public void PointLights_ShouldEvaluateTheLightIntensityAtGivenPoint(Point point, double result)
        {
            var w = new World();
            w.CreateDefaultWorld();
            var light = w.Lights[0];
            var pt = point;
            var intensity = light.IntensityAt(pt, w);
            Assert.Equal(result, intensity);
        }

        public static IEnumerable<object[]> GetLightIntensityData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 1.0001, 0), 1.0, },
                new object[] { new Point(-1.0001, 0, 0), 1.0, },
                new object[] { new Point(0, 0, -1.0001), 1.0, },
                new object[] { new Point(0, 0, 1.0001), 0.0, },
                new object[] { new Point(1.0001, 0, 0), 0.0, },
                new object[] { new Point(0, -1.0001, 0), 0.0, },
                new object[] { new Point(0, 0, 0), 0.0, },
            };

            return allData;
        }

        [Fact]
        public void CreatingAnAreaLight_ShouldWork()
        {
            var corner = new Point(0, 0, 0);
            var v1 = new Vector(2, 0, 0);
            var v2 = new Vector(0, 0, 1);
            var light = new AreaLight(corner, v1, 4, v2, 2, Color.White);
            Assert.Equal(corner, light.Corner);
            Assert.Equal(new Vector(0.5, 0, 0), light.UVec, VectorComparer);
            Assert.Equal(4, light.USteps);
            Assert.Equal(new Vector(0, 0, 0.5), light.VVec, VectorComparer);
            Assert.Equal(2, light.VSteps);
            Assert.Equal(8, light.Samples);
            Assert.Equal(new Point(1, 0, 0.5), light.Position, PointComparer);
        }

        [Theory]
        [MemberData(nameof(AreaLightUVData))]
        public void FindingSinglePointOnAnAreaLight_ShouldWork(double u, double v, Point result)
        {
            var corner = new Point(0, 0, 0);
            var v1 = new Vector(2, 0, 0);
            var v2 = new Vector(0, 0, 1);
            var light = new AreaLight(corner, v1, 4, v2, 2, Color.White);
            light.JitterBy = new Sequence(new List<double> {0.5});
            var pt = light.PointOnLight(u, v);
            Assert.Equal(result, pt, PointComparer);
        }

        public static IEnumerable<object[]> AreaLightUVData()
        {
            var allData = new List<object[]>
            {
                new object[] { 0, 0, new Point(0.25, 0, 0.25) },
                new object[] { 1, 0, new Point(0.75, 0, 0.25) },
                new object[] { 0, 1, new Point(0.25, 0, 0.75) },
                new object[] { 2, 0, new Point(1.25, 0, 0.25) },
                new object[] { 3, 1, new Point(1.75, 0, 0.75) },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(AreaLightIntensityData))]
        public void AreaLightIntensityMethod_ShouldWork(Point point, double result)
        {
            var w = new World();
            w.CreateDefaultWorld();
            var corner = new Point(-0.5, -0.5, -5);
            var v1 = new Vector(1, 0, 0);
            var v2 = new Vector(0, 1, 0);
            var light = new AreaLight(corner, v1, 2, v2, 2, Color.White);
            light.JitterBy = new Sequence(new List<double> {0.5});
            var pt = point;
            var intensity = light.IntensityAt(pt, w);
            Assert.Equal(result, intensity);
        }

        public static IEnumerable<object[]> AreaLightIntensityData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 0, 2), 0.0 },
                new object[] { new Point(1, -1, 2), 0.25 },
                new object[] { new Point(1.5, 0, 2), 0.5 },
                new object[] { new Point(1.25, 1.25, 3), 0.75 },
                new object[] { new Point(0, 0, -2), 1.0 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(AreaLightJitterData))]
        public void FindingSinglePointOnLight_ShouldReturnJitter(int u, int v, Point result)
        {
            var corner = new Point(0, 0, 0);
            var v1 = new Vector(2, 0, 0);
            var v2 = new Vector(0, 0, 1);
            var light = new AreaLight(corner, v1, 4, v2, 2, Color.White);
            light.JitterBy = new Sequence(new List<double> { 0.3, 0.7 });
            var pt = light.PointOnLight(u, v);
            Assert.Equal(result, pt, PointComparer);
        }

        public static IEnumerable<object[]> AreaLightJitterData()
        {
            var allData = new List<object[]>
            {
                new object[] { 0, 0, new Point(0.15, 0, 0.35) },
                new object[] { 1, 0, new Point(0.65, 0, 0.35) },
                new object[] { 0, 1, new Point(0.15, 0, 0.85) },
                new object[] { 2, 0, new Point(1.15, 0, 0.35) },
                new object[] { 3, 1, new Point(1.65, 0, 0.85) },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(AreaLightWithMoreJitterData))]
        public void AreaLight_ShouldWorkWithJitteredSamples(Point point, double result)
        {
            var w = new World();
            w.CreateDefaultWorld();
            var corner = new Point(-0.5, -0.5, -5);
            var v1 = new Vector(1, 0, 0);
            var v2 = new Vector(0, 1, 0);
            var light = new AreaLight(corner, v1, 2, v2, 2, Color.White);
            light.JitterBy = new Sequence(new List<double> { 0.7, 0.3, 0.9, 0.1, 0.5 });
            var pt = point;
            var intensity = light.IntensityAt(pt, w);
            Assert.Equal(result, intensity, 2);
        }

        public static IEnumerable<object[]> AreaLightWithMoreJitterData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 0, 2), 0.0 },
                new object[] { new Point(1, -1, 2), 0.5 },
                new object[] { new Point(1.5, 0, 2), 0.75 },
                new object[] { new Point(1.25, 1.25, 3), 0.75 },
                new object[] { new Point(0, 0, -2), 1.0 },
            };

            return allData;
        }
    }
}