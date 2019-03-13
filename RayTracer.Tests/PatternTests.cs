using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class TestPattern : Pattern
    {
        public TestPattern() {}

        public override Color PatternAt(Point point)
        {
            return new Color(point.x, point.y, point.z);
        }
    }

    public class PatternTests
    {
        const double epsilon = 0.0001;

        static readonly IEqualityComparer<Color> ColorComparer =
            Color.GetEqualityComparer(epsilon);

        Color black;
        Color white;

        public PatternTests()
        {
            Color black = new Color(0,0,0);
            Color white = new Color(1,1,1);
        }

        [Fact]
        public void CreatingStripePattern_ShouldWork()
        {
            var pattern = new Stripe(white, black);
            Assert.Equal(white, pattern.a);
            Assert.Equal(black, pattern.b);
        }

        [Fact]
        public void StripePattern_ShouldBeConstantInY()
        {
            var pattern = new Stripe(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 1, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 2, 0)));
        }

        [Fact]
        public void StripePattern_ShouldBeConstantInZ()
        {
            var pattern = new Stripe(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 1)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 2)));
        }

        [Fact]
        public void StripePattern_ShouldAlternateInX()
        {
            var pattern = new Stripe(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0.9, 0, 0)));
            Assert.Equal(black, pattern.PatternAt(new Point(1, 0, 0)));
            Assert.Equal(black, pattern.PatternAt(new Point(-0.1, 0, 0)));
            Assert.Equal(black, pattern.PatternAt(new Point(-1, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(-1.1, 0, 0)));
        }

        [Fact]
        public void StripesWithObjectTransform_ShouldWork()
        {
            var shape = new Sphere();
            shape.Transform = Transformation.Scaling(2, 2, 2);
            var pattern = new TestPattern();
            var c = pattern.PatternAtShape(shape, new Point(2, 3, 4));
            Assert.Equal(new Color(1, 1.5, 2), c);
        }

        [Fact]
        public void StripesWithPatternTransform_ShouldWork()
        {
            var shape = new Sphere();
            var pattern = new TestPattern();
            pattern.Transform = Transformation.Scaling(2, 2, 2);
            var c = pattern.PatternAtShape(shape, new Point(2, 3, 4));
            Assert.Equal(new Color(1, 1.5, 2), c);
        }

        [Fact]
        public void StripesWithBothObjectAndPatternTransforms_ShouldWork()
        {
            var shape = new Sphere();
            shape.Transform = Transformation.Scaling(2, 2, 2);
            var pattern = new TestPattern();
            pattern.Transform = Transformation.Translation(0.5, 1, 1.5);
            var c = pattern.PatternAtShape(shape, new Point(2.5, 3, 3.5));
            Assert.Equal(new Color(0.75, 0.5, 0.25), c);
        }

        [Fact]
        public void GradientPattern_ShouldLinearlyInterpolateBetweenColors()
        {
            var pattern = new Gradient(new Color(1,1,1), new Color(0,0,0));
            Assert.Equal(new Color(1,1,1), pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(new Color(0.75, 0.75, 0.75), pattern.PatternAt(new Point(0.25, 0, 0)));
            Assert.Equal(new Color(0.5, 0.5, 0.5), pattern.PatternAt(new Point(0.5, 0, 0)));
            Assert.Equal(new Color(0.25, 0.25, 0.25), pattern.PatternAt(new Point(0.75, 0, 0)));
        }

        [Fact]
        public void RingPattern_ShouldExtendInBothXandZ()
        {
            var pattern = new Ring(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(black, pattern.PatternAt(new Point(1, 0, 0)));
            Assert.Equal(black, pattern.PatternAt(new Point(0, 0, 1)));
            // 0.708 = just slightly more than √2/2
            Assert.Equal(black, pattern.PatternAt(new Point(0.708, 0, 0.708)));
        }

        [Fact]
        public void CheckerPattern_ShouldRepeatInX()
        {
            var pattern = new Checkers(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0.99, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(1.01, 0, 0)));
        }

        [Fact]
        public void CheckerPattern_ShouldRepeatInY()
        {
            var pattern = new Checkers(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0.99, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 1.01, 0)));
        }

        [Fact]
        public void CheckerPattern_ShouldRepeatInZ()
        {
            var pattern = new Checkers(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0.99)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 1.01)));
        }

        [Theory]
        [MemberData(nameof(GetUvData))]
        public void CheckerPatternIn2d_ShouldBeDefined(double u, double v, Color expected)
        {
            var checkers = new UvCheckers(2, 2, Color.Black, Color.White);
            var color = checkers.UvPatternAt(u, v);
            Assert.Equal(expected, color, ColorComparer);
        }

        public static IEnumerable<object[]> GetUvData()
        {
            var allData = new List<object[]>
            {
                new object[] { 0.0, 0.0, Color.Black },
                new object[] { 0.5, 0.0, Color.White },
                new object[] { 0.0, 0.5, Color.White },
                new object[] { 0.5, 0.5, Color.Black },
                new object[] { 1.0, 1.0, Color.Black },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetUvMappingData))]
        public void UsingSphericalMappingOn3dPoint_ShouldTranslateFrom3dTo2d(Point point, double expectedU, double expectedV)
        {
            var checkers = new UvCheckers(2, 2, Color.Black, Color.White);
            var (u, v) = checkers.SphericalMap(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetUvMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 0, -1), 0.0, 0.5 },
                new object[] { new Point(1, 0, 0), 0.25, 0.5 },
                new object[] { new Point(0, 0, 1), 0.5, 0.5 },
                new object[] { new Point(-1, 0, 0), 0.75, 0.5 },
                new object[] { new Point(0, 1, 0), 0.5, 1.0 },
                new object[] { new Point(0, -1, 0), 0.5, 0.0 },
                new object[] { new Point(Math.Sqrt(2)/2, Math.Sqrt(2)/2, 0), 0.25, 0.75 },
            };

            return allData;
        }
    }
}