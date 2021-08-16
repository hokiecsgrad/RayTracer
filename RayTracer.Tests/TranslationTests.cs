using System;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class TranslationTests
    {
        const double epsilon = 0.00001;

        static readonly IEqualityComparer<Point> PointComparer =
            Point.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Vector> VectorComparer =
            Vector.GetEqualityComparer(epsilon);

        static readonly IEqualityComparer<Matrix> MatrixComparer =
            Matrix.GetEqualityComparer(epsilon);

        [Fact]
        public void MultiplyingByTranslationMatrixFromZero_ShouldWork()
        {
            var transform = Transformation.Translation(5, -3, 2);
            var point = new Point(0, 0, 0);
            Assert.Equal(new Point(5, -3, 2), (transform * point), PointComparer);
        }

        [Fact]
        public void MultiplyingByTranslationMatrix_ShouldWork()
        {
            var transform = Transformation.Translation(5, -3, 2);
            var point = new Point(-3, 4, 5);
            Assert.Equal(new Point(2, 1, 7), (transform * point), PointComparer);
        }

        [Fact]
        public void MultiplyingByInverseOfTranslationMatrix_ShouldWork()
        {
            var transform = Transformation.Translation(5, -3, 2);
            var inverse = transform.Inverse();
            var point = new Point(-3, 4, 5);
            Assert.Equal(new Point(-8, 7, 3), (inverse * point), PointComparer);
        }

        [Fact]
        public void MultiplyingVectorByTranslationMatrix_ShouldNotChangeVector()
        {
            var transform = Transformation.Translation(5, -3, 2);
            var vector = new Vector(-3, 4, 5);
            Assert.Equal(new Vector(-3, 4, 5), (transform * vector), VectorComparer);
        }

        [Fact]
        public void ApplyingScalingMatrixToPoint_ShouldWork()
        {
            var transform = Transformation.Scaling(2, 3, 4);
            var point = new Point(-4, 6, 8);
            Assert.Equal(new Point(-8, 18, 32), (transform * point), PointComparer);
        }

        [Fact]
        public void ApplyingScalingMatrixToVector_ShouldWork()
        {
            var transform = Transformation.Scaling(2, 3, 4);
            var vector = new Vector(-4, 6, 8);
            Assert.Equal(new Vector(-8, 18, 32), (transform * vector), VectorComparer);
        }

        [Fact]
        public void MultiplyingByInverseOfScalingMatrix_ShouldWork()
        {
            var transform = Transformation.Scaling(2, 3, 4);
            var inverse = transform.Inverse();
            var vector = new Vector(-4, 6, 8);
            Assert.Equal(new Vector(-2, 2, 2), (inverse * vector), VectorComparer);
        }

        [Fact]
        public void MultiplyingByNegativeScalingMatrix_ShouldCauseReflection()
        {
            var transform = Transformation.Scaling(-1, 1, 1);
            var point = new Point(2, 3, 4);
            Assert.Equal(new Point(-2, 3, 4), (transform * point), PointComparer);
        }

        [Fact]
        public void RotatingPointAroundXAxis_ShouldWork()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Transformation.Rotation_x(Math.PI / 4);
            var fullQuarter = Transformation.Rotation_x(Math.PI / 2);
            Assert.Equal(new Point(0, Math.Sqrt(2) / 2.0, Math.Sqrt(2) / 2.0), (halfQuarter * point), PointComparer);
            Assert.Equal(new Point(0, 0, 1), (fullQuarter * point), PointComparer);
        }

        [Fact]
        public void RotatingPointAroundInverseXAxis_ShouldWork()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Transformation.Rotation_x(Math.PI / 4);
            var inverse = halfQuarter.Inverse();
            Assert.Equal(new Point(0, Math.Sqrt(2) / 2.0, -Math.Sqrt(2) / 2.0), (inverse * point), PointComparer);
        }

        [Fact]
        public void RotatingPointAroundYAxis_ShouldWork()
        {
            var point = new Point(0, 0, 1);
            var halfQuarter = Transformation.Rotation_y(Math.PI / 4);
            var fullQuarter = Transformation.Rotation_y(Math.PI / 2);
            Assert.Equal(new Point(Math.Sqrt(2) / 2.0, 0, Math.Sqrt(2) / 2.0), (halfQuarter * point), PointComparer);
            Assert.Equal(new Point(1, 0, 0), (fullQuarter * point), PointComparer);
        }

        [Fact]
        public void RotatingPointAroundZAxis_ShouldWork()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Transformation.Rotation_z(Math.PI / 4);
            var fullQuarter = Transformation.Rotation_z(Math.PI / 2);
            Assert.Equal(new Point(-Math.Sqrt(2) / 2.0, Math.Sqrt(2) / 2.0, 0), (halfQuarter * point), PointComparer);
            Assert.Equal(new Point(-1, 0, 0), (fullQuarter * point), PointComparer);
        }

        [Fact]
        public void ShearingTransformationOfX_ShouldMoveYProportionately()
        {
            var transform = Transformation.Shearing(1, 0, 0, 0, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.Equal(new Point(5, 3, 4), (transform * point), PointComparer);
        }

        [Fact]
        public void ShearingTransformationOfX_ShouldMoveZProportionately()
        {
            var transform = Transformation.Shearing(0, 1, 0, 0, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.Equal(new Point(6, 3, 4), (transform * point), PointComparer);
        }

        [Fact]
        public void ShearingTransformationOfY_ShouldMoveXProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 1, 0, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.Equal(new Point(2, 5, 4), (transform * point), PointComparer);
        }

        [Fact]
        public void ShearingTransformationOfY_ShouldMoveZProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 0, 1, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.Equal(new Point(2, 7, 4), (transform * point), PointComparer);
        }

        [Fact]
        public void ShearingTransformationOfZ_ShouldMoveXProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 0, 0, 1, 0);
            var point = new Point(2, 3, 4);
            Assert.Equal(new Point(2, 3, 6), (transform * point), PointComparer);
        }

        [Fact]
        public void ShearingTransformationOfZ_ShouldMoveYProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 0, 0, 0, 1);
            var point = new Point(2, 3, 4);
            Assert.Equal(new Point(2, 3, 7), (transform * point), PointComparer);
        }

        [Fact]
        public void IndividualTransformations_ShouldBeAppliedInSequence()
        {
            var point = new Point(1, 0, 1);
            var A = Transformation.Rotation_x(Math.PI / 2);
            var B = Transformation.Scaling(5, 5, 5);
            var C = Transformation.Translation(10, 5, 7);
            // apply rotation first
            var rotatedPoint = A * point;
            Assert.Equal(new Point(1, -1, 0), rotatedPoint, PointComparer);
            // then apply scaling
            var scaledPoint = B * rotatedPoint;
            Assert.Equal(new Point(5, -5, 0), scaledPoint, PointComparer);
            // then apply translation
            var translatedPoint = C * scaledPoint;
            Assert.Equal(new Point(15, 0, 7), translatedPoint, PointComparer);
        }

        [Fact]
        public void ChainedTranformations_ShouldBeAppliedInReverseOrder()
        {
            var point = new Point(1, 0, 1);
            var A = Transformation.Rotation_x(Math.PI / 2);
            var B = Transformation.Scaling(5, 5, 5);
            var C = Transformation.Translation(10, 5, 7);
            var allTransformations = C * B * A;
            Assert.Equal(new Point(15, 0, 7), (allTransformations * point), PointComparer);
        }

        [Fact]
        public void ViewTransformationMatrixForTheDefaultOrientation_ShouldBeIdentity()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, -1);
            var up = new Vector(0, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            Assert.Equal(Matrix.Identity, t, MatrixComparer);
        }

        [Fact]
        public void ViewTransformationMatrixLookingInPositiveZDirection_ShouldBeSameAsScalingByNegativeValue()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, 1);
            var up = new Vector(0, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            Assert.Equal(Transformation.Scaling(-1, 1, -1), t, MatrixComparer);
        }

        [Fact]
        public void ViewTransfomration_ShouldMoveWorldAndNotEye()
        {
            var from = new Point(0, 0, 8);
            var to = new Point(0, 0, 0);
            var up = new Vector(0, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            Assert.Equal(Transformation.Translation(0, 0, -8), t, MatrixComparer);
        }

        [Fact]
        public void ViewTransformationInArbitraryDirection_ShouldProduceMatrixWhichIsComboOfAllTranslations()
        {
            var from = new Point(1, 3, 2);
            var to = new Point(4, -2, 8);
            var up = new Vector(1, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            var expected = new Matrix(new double[,] {
                {-0.50709, 0.50709, 0.67612, -2.36643},
                {0.76772, 0.60609, 0.12122, -2.82843},
                {-0.35857, 0.59761, -0.71714, 0.00000},
                {0.00000, 0.00000, 0.00000, 1.00000}
                });
            Assert.Equal(expected, t, MatrixComparer);
        }
    }
}