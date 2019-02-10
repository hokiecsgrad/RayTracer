using System;
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
        private Color black, white;

        public PatternTests()
        {
            Color black = new Color(0,0,0);
            Color white = new Color(1,1,1);
        }

        [Fact]
        public void CreatingStripePattern_ShouldWork()
        {
            var pattern = new StripePattern(white, black);
            Assert.Equal(white, pattern.a);
            Assert.Equal(black, pattern.b);
        }

        [Fact]
        public void StripePattern_ShouldBeConstantInY()
        {
            var pattern = new StripePattern(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 1, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 2, 0)));
        }

        [Fact]
        public void StripePattern_ShouldBeConstantInZ()
        {
            var pattern = new StripePattern(white, black);
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 0)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 1)));
            Assert.Equal(white, pattern.PatternAt(new Point(0, 0, 2)));
        }

        [Fact]
        public void StripePattern_ShouldAlternateInX()
        {
            var pattern = new StripePattern(white, black);
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
    }
}