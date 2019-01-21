using System;
using Xunit;

namespace RayTracer.Tests
{
    public class TupleTests
    {
        [Fact]
        public void CreateTupleWithWSetToOne_ShouldReturnPoint()
        {
            var myPoint = new Point(4.3, -4.2, 3.1);
            Assert.True(myPoint.w == 1.0);
        }

        [Fact]
        public void CreateTupleWithWSetToZero_ShouldReturnVector()
        {
            var myVector = new Vector(4.3, -4.2, 3.1);
            Assert.True(myVector.w == 0.0);
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
            Point result = myPoint + myVector;
            Assert.True(result.Equals(new Point(1.0, 1.0, 6.0)));
        }

        [Fact]
        public void AddingTwoVectors_ShouldCreateNewVector()
        {
            var myVector1 = new Vector(3.0, -2.0, 5.0);
            var myVector2 = new Vector(-2.0, 3.0, 1.0);
            Vector result = myVector1 + myVector2;
            Assert.True(result.Equals(new Vector(1.0, 1.0, 6.0)));
        }

        [Fact]
        public void SubtractingTwoPoints_ShouldCreateNewVector()
        {
            var myPoint1 = new Point(3, 2, 1);
            var myPoint2 = new Point(5, 6, 7);
            Vector result = myPoint1 - myPoint2;
            Assert.True(result.Equals(new Vector(-2, -4, -6)));
        }

        [Fact]
        public void SubtractingVectorFromPoint_ShouldCreateNewPoint()
        {
            var myPoint = new Point(1, 2, 3);
            var myVector = new Vector(4, 5, 6);
            Point result = myPoint - myVector;
            Assert.True(result.Equals(new Point(-3, -3, -3)));
        }

        [Fact]
        public void SubtractingTwoVectors_ShouldCreateNewVector()
        {
            var myVector1 = new Vector(4, 5, 6);
            var myVector2 = new Vector(1, 2, 3);
            Vector result = myVector1 - myVector2;
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
            Vector result = zeroVector - myVector;
            Assert.True(result.Equals(new Vector(-1, 2, -3)));
        }

        [Fact]
        public void NegatingTuple_ShouldWork()
        {
            var myVector = new Vector(1, -2, 3);
            Vector result = -myVector;
            Assert.True(result.Equals(new Vector(-1, 2, -3)));
        }

        [Fact]
        public void MultiplingTupleByScalar_ShouldMultiplyEachElementByScalar()
        {
            var myTuple = new Vector(1, -2, 3);
            Vector result = myTuple * 3.5;
            Assert.True(result.Equals(new Vector(3.5, -7.0, 10.5)));
        }

        [Fact]
        public void MultiplingTupleByFraction_ShouldMultiplyEachElementByFraction()
        {
            var myTuple = new Vector(1, -2, 3);
            Vector result = myTuple * 0.5;
            Assert.True(result.Equals(new Vector(0.5, -1.0, 1.5)));
        }

        [Fact]
        public void DividingTupleByScaler_ShouldDivideEachElementByScalar()
        {
            var myTuple = new Vector(1, -2, 3);
            Vector result = myTuple / 2;
            Assert.True(result.Equals(new Vector(0.5, -1.0, 1.5)));
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
            Assert.True(result.Equals(new Color(1.6, 0.7, 1.0)));
        }

        [Fact]
        public void SubtractingTwoColors_ShouldReturnNewColor()
        {
            var myColor1 = new Color(0.9, 0.6, 0.75);
            var myColor2 = new Color(0.7, 0.1, 0.25);
            var result = myColor1 - myColor2;
            Assert.True(result.Equals(new Color(0.2, 0.5, 0.5)));
        }

        [Fact]
        public void MultiplingColorByScalar_ShouldReturnNewColor()
        {
            var myColor = new Color(0.2, 0.3, 0.4);
            var result = myColor * 2;
            Assert.True(result.Equals(new Color(0.4, 0.6, 0.8)));
        }

        [Fact]
        public void MultiplingTwoColors_ShouldReturnNewColor()
        {
            var myColor1 = new Color(1, 0.2, 0.4);
            var myColor2 = new Color(0.9, 1, 0.1);
            var result = myColor1 * myColor2;
            Assert.True(result.Equals(new Color(0.9, 0.2, 0.04)));
        }

        [Fact]
        public void ReflectingVectorAt45Degrees_ShouldReturn45DegreeVector()
        {
            var vector = new Vector(1, -1, 0);
            var normal = new Vector(0, 1, 0);
            var reflected = vector.Reflect(normal);
            Assert.True(reflected.Equals(new Vector(1, 1, 0)));
        }

        [Fact]
        public void ReflectingOffSlantedSurface_ShouldWork()
        {
            var vector = new Vector(0, -1, 0);
            var normal = new Vector(Math.Sqrt(2)/2, Math.Sqrt(2)/2, 0);
            var reflected = vector.Reflect(normal);
            Assert.True(reflected.Equals(new Vector(1, 0, 0)));
        }
    }
}
