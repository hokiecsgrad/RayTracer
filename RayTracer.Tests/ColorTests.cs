using System;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class ColorTests
    {
        const double epsilon = 0.000001;

        static readonly IEqualityComparer<Color> Comparer =
            Color.GetEqualityComparer(epsilon);

        [Fact]
        public void ImplementingColorClassWithTuple_ShouldWork()
        {
            var myColor = new Color(-0.5, 0.4, 1.7);
            Assert.True(myColor.Red == -0.5);
            Assert.True(myColor.Green == 0.4);
            Assert.True(myColor.Blue == 1.7);
        }

        [Fact]
        public void AddingTwoColors_ShouldReturnNewColor()
        {
            var myColor1 = new Color(0.9, 0.6, 0.75);
            var myColor2 = new Color(0.7, 0.1, 0.25);
            var result = myColor1 + myColor2;
            Assert.Equal(new Color(1.6, 0.7, 1.0), result, Comparer);
        }

        [Fact]
        public void SubtractingTwoColors_ShouldReturnNewColor()
        {
            var myColor1 = new Color(0.9, 0.6, 0.75);
            var myColor2 = new Color(0.7, 0.1, 0.25);
            var result = myColor1 - myColor2;
            Assert.Equal(new Color(0.2, 0.5, 0.5), result, Comparer);
        }

        [Fact]
        public void MultiplingColorByScalar_ShouldReturnNewColor()
        {
            var myColor = new Color(0.2, 0.3, 0.4);
            var result = myColor * 2;
            Assert.Equal(new Color(0.4, 0.6, 0.8), result, Comparer);
        }

        [Fact]
        public void MultiplingTwoColors_ShouldReturnNewColor()
        {
            var myColor1 = new Color(1, 0.2, 0.4);
            var myColor2 = new Color(0.9, 1, 0.1);
            var result = myColor1 * myColor2;
            Assert.Equal(new Color(0.9, 0.2, 0.04), result, Comparer);
        }
    }
}