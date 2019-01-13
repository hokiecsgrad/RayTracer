using System;
using Xunit;

namespace RayTracer.Tests
{
    public class TupleTests
    {
        [Fact]
        public void CreateBasicTuple_ShouldReturnTuple()
        {
            var myTuple = new Tuple<int, int, int>(3, 4, 5);
            Assert.True(myTuple is Tuple<int, int, int>);
        }

        [Fact]
        public void CreateTupleWithWSetToOne_ShouldReturnPoint()
        {
            var myPoint = new Point(4.3, -4.2, 3.1);
            Assert.True(myPoint is Tuple<double, double, double, double>);
            Assert.True(myPoint.Item4 == 1.0);
        }

        [Fact]
        public void CreateTupleWithWSetToZero_ShouldReturnVector()
        {
            var myVector = new Vector(4.3, -4.2, 3.1);
            Assert.True(myVector is Tuple<double, double, double, double>);
            Assert.True(myVector.Item4 == 0.0);
        }

        [Fact]
        public void CreateTwoPointsWithDiffValues_ShouldNotBeEqual()
        {
            var firstPoint = new Point(1.0, 2.0, 3.0);
            var secondPoint = new Point(4.0, 5.0, 6.0);
            Assert.False(firstPoint.Equals(secondPoint));
        }

        [Fact]
        public void CreateTwoPointsWithSameValues_ShouldBeEqual()
        {
            var firstPoint = new Point(1.0, 2.0, 3.0);
            var secondPoint = new Point(1.0, 2.0, 3.0);
            Assert.True(firstPoint.Equals(secondPoint));
        }

        [Fact]
        public void CreateTwoPointsWithHighPrecisionDiff_ShouldBeEqual()
        {
            var firstPoint = new Point(1.0000001, 2.0000001, 3.0000001);
            var secondPoint = new Point(1.0000005, 2.0000005, 3.0000005);
            Assert.True(firstPoint.Equals(secondPoint));
        }

        [Fact]
        public void CreateTwoVectorsWithDiffValues_ShouldNotBeEqual()
        {
            var firstVector = new Vector(1.0, 2.0, 3.0);
            var secondVector = new Vector(4.0, 5.0, 6.0);
            Assert.False(firstVector.Equals(secondVector));
        }

        [Fact]
        public void CreateTwoVectorsWithSameValues_ShouldBeEqual()
        {
            var firstVector = new Vector(1.0, 2.0, 3.0);
            var secondVector = new Vector(1.0, 2.0, 3.0);
            Assert.True(firstVector.Equals(secondVector));
        }

        [Fact]
        public void CreateTwoVectorsWithHighPrecisionDiff_ShouldBeEqual()
        {
            var firstVector = new Vector(1.0000001, 2.0000001, 3.0000001);
            var secondVector = new Vector(1.0000005, 2.0000005, 3.0000005);
            Assert.True(firstVector.Equals(secondVector));
        }

    }
}
