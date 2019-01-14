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

        [Fact]
        public void AddingPointAndVectors_ShouldCreateNewPoint()
        {
            var myPoint = new Point(3.0, -2.0, 5.0);
            var myVector = new Vector(-2.0, 3.0, 1.0);
            RayTuple result = myPoint + myVector;
            Assert.True(result.Equals(new Point(1.0, 1.0, 6.0)));
        }

        [Fact]
        public void AddingTwoVectors_ShouldCreateNewVector()
        {
            var myVector1 = new Vector(3.0, -2.0, 5.0);
            var myVector2 = new Vector(-2.0, 3.0, 1.0);
            RayTuple result = myVector1 + myVector2;
            Assert.True(result.Equals(new Vector(1.0, 1.0, 6.0)));
        }

        [Fact]
        public void SubtractingTwoPoints_ShouldCreateNewVector()
        {
            var myPoint1 = new Point(3, 2, 1);
            var myPoint2 = new Point(5, 6, 7);
            RayTuple result = myPoint1 - myPoint2;
            Assert.True(result.Equals(new Vector(-2, -4, -6)));
        }

        [Fact]
        public void SubtractingVectorFromPoint_ShouldCreateNewPoint()
        {
            var myPoint = new Point(1, 2, 3);
            var myVector = new Vector(4, 5, 6);
            RayTuple result = myPoint - myVector;
            Assert.True(result.Equals(new Point(-3, -3, -3)));
        }

        [Fact]
        public void SubtractingTwoVectors_ShouldCreateNewVector()
        {
            var myVector1 = new Vector(4, 5, 6);
            var myVector2 = new Vector(1, 2, 3);
            RayTuple result = myVector1 - myVector2;
            Assert.True(result.Equals(new Vector(3, 3, 3)));
        }

        [Fact]
        public void SubtractingPointFromVector_ShouldFail()
        {
            var myVector = new Vector(4, 5, 6);
            var myPoint = new Point(1, 2, 3);
            Exception ex = Assert.Throws<InvalidOperationException>(() => myVector - myPoint);
        }

        [Fact]
        public void SubtractingVectorFromZeroVector_ShouldNegateVector()
        {
            var zeroVector = new Vector(0, 0, 0);
            var myVector = new Vector(1, -2, 3);
            RayTuple result = zeroVector - myVector;
            Assert.True(result.Equals(new Vector(-1, 2, -3)));
        }

        [Fact]
        public void NegatingTuple_ShouldWork()
        {
            var myVector = new RayTuple(1, -2, 3, -4);
            RayTuple result = -myVector;
            Assert.True(result.Equals(new RayTuple(-1, 2, -3, 4)));
        }

        [Fact]
        public void MultiplingTupleByScalar_ShouldMultiplyEachElementByScalar()
        {
            var myTuple = new RayTuple(1, -2, 3, -4);
            RayTuple result = myTuple * 3.5;
            Assert.True(result.Equals(new RayTuple(3.5, -7.0, 10.5, -14)));
        }

        [Fact]
        public void MultiplingTupleByFraction_ShouldMultiplyEachElementByFraction()
        {
            var myTuple = new RayTuple(1, -2, 3, -4);
            RayTuple result = myTuple * 0.5;
            Assert.True(result.Equals(new RayTuple(0.5, -1.0, 1.5, -2)));
        }

        [Fact]
        public void DividingTupleByScaler_ShouldDivideEachElementByScalar()
        {
            var myTuple = new RayTuple(1, -2, 3, -4);
            RayTuple result = myTuple / 2;
            Assert.True(result.Equals(new RayTuple(0.5, -1.0, 1.5, -2)));
        }

        [Fact]
        public void ComputingMagnitudeOfVector1_ShouldBe1()
        {
            var myVector1 = new Vector(1, 0, 0);
            var myVector2 = new Vector(0, 1, 0);
            var myVector3 = new Vector(0, 0, 1);
            Assert.Equal(1, myVector1.Magnitude());
            Assert.Equal(1, myVector2.Magnitude());
            Assert.Equal(1, myVector3.Magnitude());
        } 

        [Fact]
        public void ComputingMagnitudeOfVector123_ShouldBeSqrt14()
        {
            var myVector = new Vector(1, 2, 3);
            Assert.Equal(Math.Sqrt(14), myVector.Magnitude());
        }

        [Fact]
        public void ComputingMagnitudeOfVectorNegative123_ShouldBeSqrt14()
        {
            var myVector = new Vector(-1, -2, -3);
            Assert.Equal(Math.Sqrt(14), myVector.Magnitude());
        }

        [Fact]
        public void NormalizingVector400_ShouldBe100()
        {
            var myVector = new Vector(4, 0, 0);
            Assert.True(myVector.Normalize().Equals(new Vector(1, 0, 0)));
        }

        [Fact]
        public void NormalizingComplexVector_ShouldWork()
        {
            var myVector = new Vector(1, 2, 3);
            Assert.True(myVector.Normalize().Equals(new Vector(0.26726, 0.53452, 0.80178)));
        }

        [Fact]
        public void MagnitudeOfNormalizedVector_ShouldBe1()
        {
            var myVector = new Vector(1, 2, 3);
            Assert.Equal(1, myVector.Normalize().Magnitude());
        }

        [Fact]
        public void DotProductOfTwoVectors_ShouldBeSumOfProducts()
        {
            var myVector1 = new Vector(1, 2, 3);
            var myVector2 = new Vector(2, 3, 4);
            Assert.Equal(20, myVector1.Dot(myVector2));
        }

        [Fact]
        public void CrossProductOfTwoVectors_ShouldBeAnotherVector()
        {
            var myVector1 = new Vector(1, 2, 3);
            var myVector2 = new Vector(2, 3, 4);
            Assert.True(myVector1.Cross(myVector2).Equals(new Vector(-1, 2, -1)));
            Assert.True(myVector2.Cross(myVector1).Equals(new Vector(1, -2, 1)));
        }
    }
}
