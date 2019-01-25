using System;
using Xunit;

namespace RayTracer.Tests
{
    public class TranslationTests
    {
        [Fact]
        public void MultiplyingByTranslationMatrix_ShouldWork()
        {
            var transform = Transformation.Translation(5, -3, 2);
            var point = new Point(-3, 4, 5);
            Assert.True((transform * point).Equals(new Point(2, 1, 7)));
        }

        [Fact]
        public void MultiplyingByInverseOfTranslationMatrix_ShouldWork()
        {
            var transform = Transformation.Translation(5, -3, 2);
            var inverse = transform.Inverse();
            var point = new Point(-3, 4, 5);
            Assert.True((inverse * point).Equals(new Point(-8, 7, 3)));
        }

        [Fact]
        public void MultiplyingVectorByTranslationMatrix_ShouldNotChangeVector()
        {
            var transform = Transformation.Translation(5, -3, 2);
            var vector = new Vector(-3, 4, 5);
            Assert.True((transform * vector).Equals(new Vector(-3, 4, 5)));
        }

        [Fact]
        public void ApplyingScalingMatrixToPoint_ShouldWork()
        {
            var transform = Transformation.Scaling(2, 3, 4);
            var point = new Point(-4, 6, 8);
            Assert.True((transform * point).Equals(new Point(-8, 18, 32)));
        }

        [Fact]
        public void ApplyingScalingMatrixToVector_ShouldWork()
        {
            var transform = Transformation.Scaling(2, 3, 4);
            var vector = new Vector(-4, 6, 8);
            Assert.True((transform * vector).Equals(new Vector(-8, 18, 32)));
        }

        [Fact]
        public void MultiplyingByInverseOfScalingMatrix_ShouldWork()
        {
            var transform = Transformation.Scaling(2, 3, 4);
            var inverse = transform.Inverse();
            var vector = new Vector(-4, 6, 8);
            Assert.True((inverse * vector).Equals(new Vector(-2, 2, 2)));
        }

        [Fact]
        public void MultiplyingByNegativeScalingMatrix_ShouldCauseReflection()
        {
            var transform = Transformation.Scaling(-1, 1, 1);
            var point = new Point(2, 3, 4);
            Assert.True((transform * point).Equals(new Point(-2, 3, 4)));
        }

        [Fact]
        public void RotatingPointAroundXAxis_ShouldWork()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Transformation.Rotation_x(Math.PI / 4);
            var fullQuarter = Transformation.Rotation_x(Math.PI / 2);
            Assert.True((halfQuarter * point).Equals(new Point(0, Math.Sqrt(2)/2.0, Math.Sqrt(2)/2.0)));
            Assert.True((fullQuarter * point).Equals(new Point(0, 0, 1)));
        }

        [Fact]
        public void RotatingPointAroundInverseXAxis_ShouldWork()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Transformation.Rotation_x(Math.PI / 4);
            var inverse = halfQuarter.Inverse();
            Assert.True((inverse * point).Equals(new Point(0, Math.Sqrt(2)/2.0, -Math.Sqrt(2)/2.0)));
        }

        [Fact]
        public void RotatingPointAroundYAxis_ShouldWork()
        {
            var point = new Point(0, 0, 1);
            var halfQuarter = Transformation.Rotation_y(Math.PI / 4);
            var fullQuarter = Transformation.Rotation_y(Math.PI / 2);
            Assert.True((halfQuarter * point).Equals(new Point(Math.Sqrt(2)/2.0, 0, Math.Sqrt(2)/2.0)));
            Assert.True((fullQuarter * point).Equals(new Point(1, 0, 0)));
        }

        [Fact]
        public void RotatingPointAroundZAxis_ShouldWork()
        {
            var point = new Point(0, 1, 0);
            var halfQuarter = Transformation.Rotation_z(Math.PI / 4);
            var fullQuarter = Transformation.Rotation_z(Math.PI / 2);
            Assert.True((halfQuarter * point).Equals(new Point(-Math.Sqrt(2)/2.0, Math.Sqrt(2)/2.0, 0)));
            Assert.True((fullQuarter * point).Equals(new Point(-1, 0, 0)));
        }

        [Fact]
        public void ShearingTransformationOfX_ShouldMoveYProportionately()
        {
            var transform = Transformation.Shearing(1, 0, 0, 0, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.True((transform * point).Equals(new Point(5, 3, 4)));
        }

        [Fact]
        public void ShearingTransformationOfX_ShouldMoveZProportionately()
        {
            var transform = Transformation.Shearing(0, 1, 0, 0, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.True((transform * point).Equals(new Point(6, 3, 4)));
        }

        [Fact]
        public void ShearingTransformationOfY_ShouldMoveXProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 1, 0, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.True((transform * point).Equals(new Point(2, 5, 4)));
        }        

        [Fact]
        public void ShearingTransformationOfY_ShouldMoveZProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 0, 1, 0, 0);
            var point = new Point(2, 3, 4);
            Assert.True((transform * point).Equals(new Point(2, 7, 4)));
        }        

        [Fact]
        public void ShearingTransformationOfZ_ShouldMoveXProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 0, 0, 1, 0);
            var point = new Point(2, 3, 4);
            Assert.True((transform * point).Equals(new Point(2, 3, 6)));
        }        

        [Fact]
        public void ShearingTransformationOfZ_ShouldMoveYProportionately()
        {
            var transform = Transformation.Shearing(0, 0, 0, 0, 0, 1);
            var point = new Point(2, 3, 4);
            Assert.True((transform * point).Equals(new Point(2, 3, 7)));
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
            Assert.True(rotatedPoint.Equals(new Point(1, -1, 0)));
            // then apply scaling
            var scaledPoint = B * rotatedPoint;
            Assert.True(scaledPoint.Equals(new Point(5, -5, 0)));
            // then apply translation
            var translatedPoint = C * scaledPoint;
            Assert.True(translatedPoint.Equals(new Point(15, 0, 7)));
        }

        [Fact]
        public void ChainedTranformations_ShouldBeAppliedInReverseOrder()
        {
            var point = new Point(1, 0, 1);
            var A = Transformation.Rotation_x(Math.PI / 2);
            var B = Transformation.Scaling(5, 5, 5);
            var C = Transformation.Translation(10, 5, 7);
            var allTransformations = C * B * A;
            Assert.True((allTransformations * point).Equals(new Point(15, 0, 7)));
        }

        [Fact]
        public void ViewTransformationMatrixForTheDefaultOrientation_ShouldBeIdentity()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, -1);
            var up = new Vector(0, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            Assert.True(t.Equals(new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} })));
        }

        [Fact]
        public void ViewTransformationMatrixLookingInPositiveZDirection_ShouldBeSameAsScalingByNegativeValue()
        {
            var from = new Point(0, 0, 0);
            var to = new Point(0, 0, 1);
            var up = new Vector(0, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            Assert.True(t.Equals(Transformation.Scaling(-1, 1, -1)));
        }

        [Fact]
        public void ViewTransfomration_ShouldMoveWorldAndNotEye()
        {
            var from = new Point(0, 0, 8);
            var to = new  Point(0, 0, 0);
            var up = new Vector(0, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            Assert.True(t.Equals(Transformation.Translation(0, 0, -8)));
        }

        [Fact]
        public void ViewTransformationInArbitraryDirection_ShouldProduceMatrixWhichIsComboOfAllTranslations()
        {
            var from = new Point(1, 3, 2);
            var to = new Point(4, -2, 8);
            var up = new Vector(1, 1, 0);
            var t = Transformation.ViewTransform(from, to, up);
            Assert.True(t.Equals(new Matrix(new double[,] { 
                {-0.50709, 0.50709, 0.67612, -2.36643},
                {0.76772, 0.60609, 0.12122, -2.82843},
                {-0.35857, 0.59761, -0.71714, 0.00000},
                {0.00000, 0.00000, 0.00000, 1.00000}
                })));
        }
    }
}