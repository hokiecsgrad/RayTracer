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
        [MemberData(nameof(GetUvSphereMappingData))]
        public void UsingSphericalMappingOn3dPoint_ShouldTranslateFrom3dTo2d(Point point, double expectedU, double expectedV)
        {
            var checkers = new UvCheckers(2, 2, Color.Black, Color.White);
            var (u, v) = TextureMapper.SphericalMap(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetUvSphereMappingData()
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

        [Theory]
        [MemberData(nameof(GetUvMappingForPatternData))]
        public void TextureMapPattern_ShouldMapToSphereUsingSphereMap(Point point, Color expected)
        {
            var checkers = new UvCheckers(16, 8, Color.Black, Color.White);
            var pattern = new TextureMap(checkers, TextureMapper.SphericalMap);
            Assert.Equal(expected, pattern.PatternAt(point));
        }

        public static IEnumerable<object[]> GetUvMappingForPatternData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0.4315, 0.4670, 0.7719), Color.White },
                new object[] { new Point(-0.9654, 0.2552, -0.0534), Color.Black },
                new object[] { new Point(0.1039, 0.7090, 0.6975), Color.White },
                new object[] { new Point(-0.4986, -0.7856, -0.3663), Color.Black },
                new object[] { new Point(-0.0317, -0.9395, 0.3411), Color.Black },
                new object[] { new Point(0.4809, -0.7721, 0.4154), Color.Black },
                new object[] { new Point(0.0285, -0.9612, -0.2745), Color.Black },
                new object[] { new Point(-0.5734, -0.2162, -0.7903), Color.White },
                new object[] { new Point(0.7688, -0.1470, 0.6223), Color.Black },
                new object[] { new Point(-0.7652, 0.2175, 0.6060), Color.Black },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetUvPlaneMappingData))]
        public void UsingPlanarMappingOn3dPoint_ShouldTranslateFrom3dTo2d(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.PlanarMap(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetUvPlaneMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0.25, 0, 0.5), 0.25, 0.5 },
                new object[] { new Point(0.25, 0, -0.25), 0.25, 0.75 },
                new object[] { new Point(0.25, 0.5, -0.25), 0.25, 0.75 },
                new object[] { new Point(1.25, 0, 0.5), 0.25, 0.5 },
                new object[] { new Point(0.25, 0, -1.75), 0.25, 0.25 },
                new object[] { new Point(1, 0, -1), 0.0, 0.0 },
                new object[] { new Point(0, 0, 0), 0.0, 0.0 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetUvCylinderMappingData))]
        public void UsingCylindricalMappingOn3dPoint_ShouldTranslateFrom3dTo2d(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.CylindricalMap(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetUvCylinderMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0, 0, -1), 0.0, 0.0 },
                new object[] { new Point(0, 0.5, -1), 0.0, 0.5 },
                new object[] { new Point(0, 1, -1), 0.0, 0.0 },
                new object[] { new Point(0.70711, 0.5, -0.70711), 0.125, 0.5 },
                new object[] { new Point(1, 0.5, 0), 0.25, 0.5 },
                new object[] { new Point(0.70711, 0.5, 0.70711), 0.375, 0.5 },
                new object[] { new Point(0, -0.25, 1), 0.5, 0.75 },
                new object[] { new Point(-0.70711, 0.5, 0.70711), 0.625, 0.5 },
                new object[] { new Point(-1, 1.25, 0), 0.75, 0.25 },
                new object[] { new Point(-0.70711, 0.5, -0.70711), 0.875, 0.5 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeMappingData))]
        public void UsingCubeMapping_ShouldLayoutTheAlignCheckPattern(double u, double v, Color expected)
        {
            var main = new Color(1, 1, 1);
            var ul = new Color(1, 0, 0);
            var ur = new Color(1, 1, 0);
            var bl = new Color(0, 1, 0);
            var br = new Color(0, 1, 1);
            var pattern = new UvAlignCheck(main, ul, ur, bl, br);
            var c = pattern.UvPatternAt(u, v);
            Assert.Equal(expected, c);
        }

        public static IEnumerable<object[]> GetCubeMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { 0.5, 0.5, new Color(1, 1, 1) },
                new object[] { 0.1, 0.9, new Color(1, 0, 0) },
                new object[] { 0.9, 0.9, new Color(1, 1, 0) },
                new object[] { 0.1, 0.1, new Color(0, 1, 0) },
                new object[] { 0.9, 0.1, new Color(0, 1, 1) },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeFaceMappingData))]
        public void PointOnCube_ShouldIdentifyTheFaceOfTheCube(Point point, CubeFace expected)
        {
            var face = TextureMapper.FaceFromPoint(point);
            Assert.Equal(expected, face);
        }

        public static IEnumerable<object[]> GetCubeFaceMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-1, 0.5, -0.25), CubeFace.Left },
                new object[] { new Point(1.1, -0.75, 0.8), CubeFace.Right },
                new object[] { new Point(0.1, 0.6, 0.9), CubeFace.Front },
                new object[] { new Point(-0.7, 0, -2), CubeFace.Back },
                new object[] { new Point(0.5, 1, 0.9), CubeFace.Up },
                new object[] { new Point(-0.2, -1.3, 1.1), CubeFace.Down },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeFrontFaceMappingData))]
        public void GivenPointOnFrontOfCube_ShouldReturnCorrectUvMapping(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.CubeUvFront(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetCubeFrontFaceMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-0.5, 0.5, 1), 0.25, 0.75 },
                new object[] { new Point(0.5, -0.5, 1), 0.75, 0.25 },
            };

            return allData;
        }


        [Theory]
        [MemberData(nameof(GetCubeBackFaceMappingData))]
        public void GivenPointOnBackOfCube_ShouldReturnCorrectUvMapping(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.CubeUvBack(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetCubeBackFaceMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(0.5, 0.5, -1), 0.25, 0.75 },
                new object[] { new Point(-0.5, -0.5, -1), 0.75, 0.25 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeLeftFaceMappingData))]
        public void GivenPointOnLeftOfCube_ShouldReturnCorrectUvMapping(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.CubeUvLeft(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetCubeLeftFaceMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-1, 0.5, -0.5), 0.25, 0.75 },
                new object[] { new Point(-1, -0.5, 0.5), 0.75, 0.25 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeRightFaceMappingData))]
        public void GivenPointOnRightOfCube_ShouldReturnCorrectUvMapping(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.CubeUvRight(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetCubeRightFaceMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(1, 0.5, 0.5), 0.25, 0.75 },
                new object[] { new Point(1, -0.5, -0.5), 0.75, 0.25 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeUpFaceMappingData))]
        public void GivenPointOnUpOfCube_ShouldReturnCorrectUvMapping(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.CubeUvUp(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetCubeUpFaceMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-0.5, 1, -0.5), 0.25, 0.75 },
                new object[] { new Point(0.5, 1, 0.5), 0.75, 0.25 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeDownFaceMappingData))]
        public void GivenPointOnDownOfCube_ShouldReturnCorrectUvMapping(Point point, double expectedU, double expectedV)
        {
            var (u, v) = TextureMapper.CubeUvDown(point);
            Assert.Equal(expectedU, u);
            Assert.Equal(expectedV, v);
        }

        public static IEnumerable<object[]> GetCubeDownFaceMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-0.5, -1, 0.5), 0.25, 0.75 },
                new object[] { new Point(0.5, -1, -0.5), 0.75, 0.25 },
            };

            return allData;
        }

        [Theory]
        [MemberData(nameof(GetCubeColorMappingData))]
        public void FindingTheColorsOnMappedCube_ShouldWork(Point point, Color expected)
        {
            var red = new Color(1, 0, 0);
            var yellow = new Color(1, 1, 0);
            var brown = new Color(1, 0.5, 0);
            var green = new Color(0, 1, 0);
            var cyan = new Color(0, 1, 1);
            var blue = new Color(0, 0, 1);
            var purple = new Color(1, 0, 1);
            var white = new Color(1, 1, 1);
            var left = new UvAlignCheck(yellow, cyan, red, blue, brown);
            var front = new UvAlignCheck(cyan, red, yellow, brown, green);
            var right = new UvAlignCheck(red, yellow, purple, green, white);
            var back = new UvAlignCheck(green, purple, cyan, white, blue);
            var up = new UvAlignCheck(brown, cyan, purple, red, yellow);
            var down = new UvAlignCheck(purple, brown, green, blue, white);
            var pattern = new CubeMap(left, front, right, back, up, down);
            Assert.Equal(expected, pattern.PatternAt(point));
        }

        public static IEnumerable<object[]> GetCubeColorMappingData()
        {
            var allData = new List<object[]>
            {
                new object[] { new Point(-1, 0, 0), new Color(1, 1, 0) },
                new object[] { new Point(-1, 0.9, -0.9), new Color(0, 1, 1) },
                new object[] { new Point(-1, 0.9, 0.9), new Color(1, 0, 0) },
                new object[] { new Point(-1, -0.9, -0.9), new Color(0, 0, 1) },
                new object[] { new Point(-1, -0.9, 0.9), new Color(1, 0.5, 0) },
                new object[] { new Point(0, 0, 1), new Color(0, 1, 1) },
                new object[] { new Point(-0.9, 0.9, 1), new Color(1, 0, 0) },
                new object[] { new Point(0.9, 0.9, 1), new Color(1, 1, 0) },
                new object[] { new Point(-0.9, -0.9, 1), new Color(1, 0.5, 0) },
                new object[] { new Point(0.9, -0.9, 1), new Color(0, 1, 0) },
                new object[] { new Point(1, 0, 0), new Color(1, 0, 0) },
                new object[] { new Point(1, 0.9, 0.9), new Color(1, 1, 0) },
                new object[] { new Point(1, 0.9, -0.9), new Color(1, 0, 1) },
                new object[] { new Point(1, -0.9, 0.9), new Color(0, 1, 0) },
                new object[] { new Point(1, -0.9, -0.9), new Color(1, 1, 1) },
                new object[] { new Point(0, 0, -1), new Color(0, 1, 0) },
                new object[] { new Point(0.9, 0.9, -1), new Color(1, 0, 1) },
                new object[] { new Point(-0.9, 0.9, -1), new Color(0, 1, 1) },
                new object[] { new Point(0.9, -0.9, -1), new Color(1, 1, 1) },
                new object[] { new Point(-0.9, -0.9, -1), new Color(0, 0, 1) },
                new object[] { new Point(0, 1, 0), new Color(1, 0.5, 0) },
                new object[] { new Point(-0.9, 1, -0.9), new Color(0, 1, 1) },
                new object[] { new Point(0.9, 1, -0.9), new Color(1, 0, 1) },
                new object[] { new Point(-0.9, 1, 0.9), new Color(1, 0, 0) },
                new object[] { new Point(0.9, 1, 0.9), new Color(1, 1, 0) },
                new object[] { new Point(0, -1, 0), new Color(1, 0, 1) },
                new object[] { new Point(-0.9, -1, 0.9), new Color(1, 0.5, 0) },
                new object[] { new Point(0.9, -1, 0.9), new Color(0, 1, 0) },
                new object[] { new Point(-0.9, -1, -0.9), new Color(0, 0, 1) },
                new object[] { new Point(0.9, -1, -0.9), new Color(1, 1, 1) },
            };

            return allData;
        }
    }
}