using System;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class TupleTests
    {
        const double epsilon = 0.00001;

        static readonly IEqualityComparer<Vector> VectorComparer =
            Vector.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Point> PointComparer =
            Point.GetEqualityComparer(epsilon);

        [Fact]
        public void CreateTupleWithWSetToOne_ShouldReturnPoint()
        {
            var myPoint = new Point(4.3, -4.2, 3.1);
            Assert.Equal(1.0, myPoint.w);
        }

        [Fact]
        public void CreateTupleWithWSetToZero_ShouldReturnVector()
        {
            var myVector = new Vector(4.3, -4.2, 3.1);
            Assert.Equal(0.0, myVector.w);
        }

        [Fact]
        public void CreateTwoPointsWithDiffValues_ShouldNotBeEqual()
        {
            var firstPoint = new Point(1.0, 2.0, 3.0);
            var secondPoint = new Point(4.0, 5.0, 6.0);
            Assert.NotEqual(firstPoint, secondPoint, PointComparer);
        }

        [Fact]
        public void CreateTwoPointsWithSameValues_ShouldBeEqual()
        {
            var firstPoint = new Point(1.0, 2.0, 3.0);
            var secondPoint = new Point(1.0, 2.0, 3.0);
            Assert.Equal(firstPoint, secondPoint, PointComparer);
        }

        [Fact]
        public void CreateTwoPointsWithHighPrecisionDiff_ShouldBeEqual()
        {
            var firstPoint = new Point(1.0000001, 2.0000001, 3.0000001);
            var secondPoint = new Point(1.0000005, 2.0000005, 3.0000005);
            Assert.Equal(firstPoint, secondPoint, PointComparer);
        }

        [Fact]
        public void CreateTwoVectorsWithDiffValues_ShouldNotBeEqual()
        {
            var firstVector = new Vector(1.0, 2.0, 3.0);
            var secondVector = new Vector(4.0, 5.0, 6.0);
            Assert.NotEqual(firstVector, secondVector, VectorComparer);
        }

        [Fact]
        public void CreateTwoVectorsWithSameValues_ShouldBeEqual()
        {
            var firstVector = new Vector(1.0, 2.0, 3.0);
            var secondVector = new Vector(1.0, 2.0, 3.0);
            Assert.Equal(firstVector, secondVector, VectorComparer);
        }

        [Fact]
        public void CreateTwoVectorsWithHighPrecisionDiff_ShouldBeEqual()
        {
            var firstVector = new Vector(1.0000001, 2.0000001, 3.0000001);
            var secondVector = new Vector(1.0000005, 2.0000005, 3.0000005);
            Assert.Equal(firstVector, secondVector, VectorComparer);
        }

        [Fact]
        public void AddingPointAndVectors_ShouldCreateNewPoint()
        {
            var myPoint = new Point(3.0, -2.0, 5.0);
            var myVector = new Vector(-2.0, 3.0, 1.0);
            Point result = myPoint + myVector;
            Assert.Equal(new Point(1.0, 1.0, 6.0), result, PointComparer);
        }

        [Fact]
        public void AddingTwoVectors_ShouldCreateNewVector()
        {
            var myVector1 = new Vector(3.0, -2.0, 5.0);
            var myVector2 = new Vector(-2.0, 3.0, 1.0);
            Vector result = myVector1 + myVector2;
            Assert.Equal(new Vector(1.0, 1.0, 6.0), result, VectorComparer);
        }

        [Fact]
        public void SubtractingTwoPoints_ShouldCreateNewVector()
        {
            var myPoint1 = new Point(3, 2, 1);
            var myPoint2 = new Point(5, 6, 7);
            Vector result = myPoint1 - myPoint2;
            Assert.Equal(new Vector(-2, -4, -6), result, VectorComparer);
        }

        [Fact]
        public void SubtractingVectorFromPoint_ShouldCreateNewPoint()
        {
            var myPoint = new Point(1, 2, 3);
            var myVector = new Vector(4, 5, 6);
            Point result = myPoint - myVector;
            Assert.Equal(new Point(-3, -3, -3), result, PointComparer);
        }

        [Fact]
        public void SubtractingTwoVectors_ShouldCreateNewVector()
        {
            var myVector1 = new Vector(4, 5, 6);
            var myVector2 = new Vector(1, 2, 3);
            Vector result = myVector1 - myVector2;
            Assert.Equal(new Vector(3, 3, 3), result, VectorComparer);
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
            Assert.Equal(new Vector(-1, 2, -3), result, VectorComparer);
        }

        [Fact]
        public void NegatingTuple_ShouldWork()
        {
            var myVector = new Vector(1, -2, 3);
            Vector result = -myVector;
            Assert.Equal(new Vector(-1, 2, -3), result, VectorComparer);
        }

        [Fact]
        public void MultiplingTupleByScalar_ShouldMultiplyEachElementByScalar()
        {
            var myTuple = new Vector(1, -2, 3);
            Vector result = myTuple * 3.5;
            Assert.Equal(new Vector(3.5, -7.0, 10.5), result, VectorComparer);
        }

        [Fact]
        public void MultiplingTupleByFraction_ShouldMultiplyEachElementByFraction()
        {
            var myTuple = new Vector(1, -2, 3);
            Vector result = myTuple * 0.5;
            Assert.Equal(new Vector(0.5, -1.0, 1.5), result, VectorComparer);
        }

        [Fact]
        public void DividingTupleByScaler_ShouldDivideEachElementByScalar()
        {
            var myTuple = new Vector(1, -2, 3);
            Vector result = myTuple / 2;
            Assert.Equal(new Vector(0.5, -1.0, 1.5), result, VectorComparer);
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
            Assert.Equal(new Vector(1, 0, 0), myVector.Normalize(), VectorComparer);
        }

        [Fact]
        public void NormalizingComplexVector_ShouldWork()
        {
            var myVector = new Vector(1, 2, 3);
            Assert.Equal(new Vector(0.26726, 0.53452, 0.80178), myVector.Normalize(), VectorComparer);
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
            Assert.Equal(new Vector(-1, 2, -1), myVector1.Cross(myVector2), VectorComparer);
            Assert.Equal(new Vector(1, -2, 1), myVector2.Cross(myVector1), VectorComparer);
        }

        [Fact]
        public void ReflectingVectorAt45Degrees_ShouldReturn45DegreeVector()
        {
            var vector = new Vector(1, -1, 0);
            var normal = new Vector(0, 1, 0);
            var reflected = vector.Reflect(normal);
            Assert.Equal(new Vector(1, 1, 0), reflected, VectorComparer);
        }

        [Fact]
        public void ReflectingOffSlantedSurface_ShouldWork()
        {
            var vector = new Vector(0, -1, 0);
            var normal = new Vector(Math.Sqrt(2)/2, Math.Sqrt(2)/2, 0);
            var reflected = vector.Reflect(normal);
            Assert.Equal(new Vector(1, 0, 0), reflected, VectorComparer);
        }
    }
}
