using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class BoundingBoxTests
    {
        private const double EPSILON = 0.00001;

        static readonly IEqualityComparer<Point> PointComparer =
            Point.GetEqualityComparer(EPSILON);

        [Fact]
        public void CreatingEmptyBoundingBox_ShouldSetMinToInfinityAndMaxToNegativeInfinity()
        {
            var box = new BoundingBox();
            Assert.True(double.IsPositiveInfinity(box.Min.x));
            Assert.True(double.IsPositiveInfinity(box.Min.y));
            Assert.True(double.IsPositiveInfinity(box.Min.z));
            Assert.True(double.IsNegativeInfinity(box.Max.x));
            Assert.True(double.IsNegativeInfinity(box.Max.y));
            Assert.True(double.IsNegativeInfinity(box.Max.z));
        }

        [Fact]
        public void CreatingBoundingBoxWithGivenVolume_ShouldSetBoxToThatVolume()
        {
            var box = new BoundingBox(new Point(-1, -2, -3), new Point(3, 2, 1));
            Assert.Equal(new Point(-1, -2, -3), box.Min, PointComparer);
            Assert.Equal(new Point(3, 2, 1), box.Max, PointComparer);
        }

        [Fact]
        public void AddingPointsToEmptyBoundingBox_ShouldAdjustTheBounds()
        {
            var box = new BoundingBox();
            var p1 = new Point(-5, 2, 0);
            var p2 = new Point(7, 0, -3);
            box.Add(p1);
            box.Add(p2);
            Assert.Equal(new Point(-5, 0, -3), box.Min, PointComparer);
            Assert.Equal(new Point(7, 2, 0), box.Max, PointComparer);
        }

        [Fact]
        public void BoundingBoxForSphere_ShouldExist()
        {
            var shape = new Sphere();
            var box = shape.GetBounds();
            Assert.Equal(new Point(-1, -1, -1), box.Min, PointComparer);
            Assert.Equal(new Point(1, 1, 1), box.Max, PointComparer);
        }

        [Fact]
        public void BoundingBoxForPlane_ShouldExist()
        {
            var shape = new Plane();
            var box = shape.GetBounds();
            Assert.True(double.IsPositiveInfinity(box.Max.x));
            Assert.True(box.Min.y == 0);
            Assert.True(double.IsPositiveInfinity(box.Max.z));
            Assert.True(double.IsNegativeInfinity(box.Min.x));
            Assert.True(box.Max.y == 0);
            Assert.True(double.IsNegativeInfinity(box.Min.z));
        }

        [Fact]
        public void BoundingBoxForCube_ShouldExist()
        {
            var shape = new Cube();
            var box = shape.GetBounds();
            Assert.Equal(new Point(-1, -1, -1), box.Min, PointComparer);
            Assert.Equal(new Point(1, 1, 1), box.Max, PointComparer);
        }

        [Fact]
        public void BoundingBoxForUnboundedCylinder_ShouldExist()
        {
            var shape = new Cylinder();
            var box = shape.GetBounds();
            Assert.True(box.Min.x == -1);
            Assert.True(double.IsNegativeInfinity(box.Min.y));
            Assert.True(box.Min.z == -1);
            Assert.True(box.Max.x == 1);
            Assert.True(double.IsPositiveInfinity(box.Max.y));
            Assert.True(box.Max.z == 1);
        }

        [Fact]
        public void BoundingBoxForBoundedCylinder_ShouldExist()
        {
            var shape = new Cylinder();
            shape.Minimum = -5;
            shape.Maximum = 3;
            var box = shape.GetBounds();
            Assert.Equal(new Point(-1, -5, -1), box.Min, PointComparer);
            Assert.Equal(new Point(1, 3, 1), box.Max, PointComparer);
        }

        [Fact]
        public void BoundingBoxForUnboundedCone_ShouldExist()
        {
            var shape = new Cone();
            var box = shape.GetBounds();
            Assert.True(double.IsNegativeInfinity(box.Min.x));
            Assert.True(double.IsNegativeInfinity(box.Min.y));
            Assert.True(double.IsNegativeInfinity(box.Min.z));
            Assert.True(double.IsPositiveInfinity(box.Max.x));
            Assert.True(double.IsPositiveInfinity(box.Max.y));
            Assert.True(double.IsPositiveInfinity(box.Max.z));
        }

        [Fact]
        public void BoundingBoxForBoundedCone_ShouldExist()
        {
            var shape = new Cone();
            shape.Minimum = -5;
            shape.Maximum = 3;
            var box = shape.GetBounds();
            Assert.Equal(new Point(-5, -5, -5), box.Min, PointComparer);
            Assert.Equal(new Point(5, 3, 5), box.Max, PointComparer);
        }

        [Fact]
        public void AddingOneBoundingBoxToAnother_ShouldCauseOriginalBoxToExpand()
        {
            var box1 = new BoundingBox(new Point(-5, -2, 0), new Point(7, 4, 4));
            var box2 = new BoundingBox(new Point(8, -7, -2), new Point(14, 2, 8));
            box1.Add(box2);
            Assert.Equal(new Point(-5, -7, -2), box1.Min, PointComparer);
            Assert.Equal(new Point(14, 4, 8), box1.Max, PointComparer);
        }

        [Theory]
        [MemberData(nameof(GetBoundingBoxData))]
        public void CheckingToSeeIfBoxContainsGivenPoint_ShouldWork(Point point, bool result)
        {
            var box = new BoundingBox(new Point(5, -2, 0), new Point(11, 4, 7));
            Assert.Equal(result, box.Contains(point));
        }

        public static IEnumerable<object[]> GetBoundingBoxData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(5, -2, 0), true },
                new object[] { new Point(11, 4, 7), true },
                new object[] { new Point(8, 1, 3), true },
                new object[] { new Point(3, 0, 3), false },
                new object[] { new Point(8, -4, 3), false },
                new object[] { new Point(8, 1, -1), false },
                new object[] { new Point(13, 1, 3), false },
                new object[] { new Point(8, 5, 3), false },
                new object[] { new Point(8, 1, 8), false },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetMoreBoundingBoxData))]
        public void CheckingToSeeIfBoxContainsAnotherBox_ShouldWork(Point min, Point max, bool result)
        {
            var box = new BoundingBox(new Point(5, -2, 0), new Point(11, 4, 7));
            var box2 = new BoundingBox(min, max);
            Assert.Equal(result, box.Contains(box2));
        }

        public static IEnumerable<object[]> GetMoreBoundingBoxData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(5, -2, 0), new Point(11, 4, 7), true },
                new object[] { new Point(6, -1, 1), new Point(10, 3, 6), true },
                new object[] { new Point(4, -3, -1), new Point(10, 3, 6), false },
                new object[] { new Point(6, -1, 1), new Point(12, 5, 8), false },
            };

            return allData;
        }

        [Fact]
        public void TransformingBoundingBoxes_ShouldWork()
        {
            var box = new BoundingBox(new Point(-1, -1, -1), new Point(1, 1, 1));
            var transform = Transformation.Rotation_x(Math.PI / 4) * Transformation.Rotation_y(Math.PI / 4);
            var box2 = BoundingBox.Transform(box, transform);
            Assert.Equal(new Point(-1.41421, -1.70710, -1.70711), box2.Min, PointComparer);
            Assert.Equal(new Point(1.41421, 1.70710, 1.70711), box2.Max, PointComparer);
        }

        [Fact]
        public void QueryingShapesBoundingBoxInItsParentsSpace_ShouldWork()
        {
            var shape = new Sphere();
            shape.Transform = Transformation.Translation(1, -3, 5) * Transformation.Scaling(0.5, 2, 4);
            var box = shape.GetParentSpaceBounds();
            Assert.Equal(new Point(0.5, -5, 1), box.Min, PointComparer);
            Assert.Equal(new Point(1.5, -1, 9), box.Max, PointComparer);
        }

        [Theory]
        [MemberData(nameof(RayBoundingBoxIntersectionData))]
        public void IntersectingRayWithBoundingBoxAtTheOrigin_ShouldWork(Point origin, Vector direction, bool result)
        {
            var box = new BoundingBox(new Point(-1, -1, -1), new Point(1, 1, 1));
            var dir = direction.Normalize();
            var r = new Ray(origin, dir);
            Assert.Equal(result, box.Intersects(r));
        }

        public static IEnumerable<object[]> RayBoundingBoxIntersectionData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(5, 0.5, 0), new Vector(-1, 0, 0), true },
                new object[] { new Point(-5, 0.5, 0), new Vector(1, 0, 0), true },
                new object[] { new Point(0.5, 5, 0), new Vector(0, -1, 0), true },
                new object[] { new Point(0.5, -5, 0), new Vector(0, 1, 0), true },
                new object[] { new Point(0.5, 0, 5), new Vector(0, 0, -1), true },
                new object[] { new Point(0.5, 0, -5), new Vector(0, 0, 1), true },
                new object[] { new Point(0, 0.5, 0), new Vector(0, 0, 1), true },
                new object[] { new Point(-2, 0, 0), new Vector(2, 4, 6), false },
                new object[] { new Point(0, -2, 0), new Vector(6, 2, 4), false },
                new object[] { new Point(0, 0, -2), new Vector(4, 6, 2), false },
                new object[] { new Point(2, 0, 2), new Vector(0, 0, -1), false },
                new object[] { new Point(0, 2, 2), new Vector(0, -1, 0), false },
                new object[] { new Point(2, 2, 0), new Vector(-1, 0, 0), false },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(MoreRayBoundingBoxIntersectionData))]
        public void IntersectingRayWithNonCubicBoundingBox_ShouldWork(Point origin, Vector direction, bool result)
        {
            var box = new BoundingBox(new Point(5, -2, 0), new Point(11, 4, 7));
            var dir = direction.Normalize();
            var r = new Ray(origin, dir);
            Assert.Equal(result, box.Intersects(r));
        }

        public static IEnumerable<object[]> MoreRayBoundingBoxIntersectionData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(15, 1, 2), new Vector(-1, 0, 0), true },
                new object[] { new Point(-5, -1, 4), new Vector(1, 0, 0), true },
                new object[] { new Point(7, 6, 5), new Vector(0, -1, 0), true },
                new object[] { new Point(9, -5, 6), new Vector(0, 1, 0), true },
                new object[] { new Point(8, 2, 12), new Vector(0, 0, -1), true },
                new object[] { new Point(6, 0, -5), new Vector(0, 0, 1), true },
                new object[] { new Point(8, 1, 3.5), new Vector(0, 0, 1), true },
                new object[] { new Point(9, -1, -8), new Vector(2, 4, 6), false },
                new object[] { new Point(8, 3, -4), new Vector(6, 2, 4), false },
                new object[] { new Point(9, -1, -2), new Vector(4, 6, 2), false },
                new object[] { new Point(4, 0, 9), new Vector(0, 0, -1), false },
                new object[] { new Point(8, 6, -1), new Vector(0, -1, 0), false },
                new object[] { new Point(12, 5, 4), new Vector(-1, 0, 0), false },
            };

            return allData;
        }

        [Fact]
        public void SplittingPerfectCubeIntoBhv_ShoudSplitBoundingBoxOnX()
        {
            var box = new BoundingBox(new Point(-1, -4, -5), new Point(9, 6, 5));
            var (left, right) = box.SplitBounds();
            Assert.Equal(new Point(-1, -4, -5), left.Min, PointComparer);
            Assert.Equal(new Point(4, 6, 5), left.Max, PointComparer);
            Assert.Equal(new Point(4, -4, -5), right.Min, PointComparer);
            Assert.Equal(new Point(9, 6, 5), right.Max, PointComparer);
        }

        [Fact]
        public void SplittingAnXwideBox_ShouldSplitBoxAlongX()
        {
            var box = new BoundingBox(new Point(-1, -2, -3), new Point(9, 5.5, 3));
            var (left, right) = box.SplitBounds();
            Assert.Equal(new Point(-1, -2, -3), left.Min, PointComparer);
            Assert.Equal(new Point(4, 5.5, 3), left.Max, PointComparer);
            Assert.Equal(new Point(4, -2, -3), right.Min, PointComparer);
            Assert.Equal(new Point(9, 5.5, 3), right.Max, PointComparer);
        }

        [Fact]
        public void SplittingYwideBox_ShouldSplitBoxAlongY()
        {
            var box = new BoundingBox(new Point(-1, -2, -3), new Point(5, 8, 3));
            var (left, right) = box.SplitBounds();
            Assert.Equal(new Point(-1, -2, -3), left.Min, PointComparer);
            Assert.Equal(new Point(5, 3, 3), left.Max, PointComparer);
            Assert.Equal(new Point(-1, 3, -3), right.Min, PointComparer);
            Assert.Equal(new Point(5, 8, 3), right.Max, PointComparer);
        }

        [Fact]
        public void SplittingZwideBox_ShouldSplitBoxAlongZ()
        {
            var box = new BoundingBox(new Point(-1, -2, -3), new Point(5, 3, 7));
            var (left, right) = box.SplitBounds();
            Assert.Equal(new Point(-1, -2, -3), left.Min, PointComparer);
            Assert.Equal(new Point(5, 3, 2), left.Max, PointComparer);
            Assert.Equal(new Point(-1, -2, 2), right.Min, PointComparer);
            Assert.Equal(new Point(5, 3, 7), right.Max, PointComparer);
        }
    }
}