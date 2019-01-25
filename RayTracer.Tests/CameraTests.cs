using System;
using Xunit;

namespace RayTracer.Tests
{
    public class CameraTests
    {
        [Fact]
        public void ConstructingACamera_ShouldWork()
        {
            var c = new Camera(160, 120, Math.PI/2);
            Assert.Equal(160, c.HSize);
            Assert.Equal(120, c.VSize);
            Assert.Equal(Math.PI/2, c.FieldOfView);
            Assert.True(c.Transform.Equals(new Matrix(new double[,] { {1, 0, 0, 0}, {0, 1, 0, 0}, {0, 0, 1, 0}, {0, 0, 0, 1} })));
        }

        [Fact]
        public void ThePixelSizeForAHorizontalCanvas_ShouldBeCorrect()
        {
            var c = new Camera(200, 125, Math.PI/2);
            Assert.True(Math.Abs(0.01 - c.PixelSize) < 0.000001);
        }

        [Fact]
        public void ThePixelSizeForAVerticalCanvas_ShouldBeCorrect()
        {
            var c = new Camera(125, 200, Math.PI/2);
            Assert.True(Math.Abs(0.01 - c.PixelSize) < 0.000001);
        }

        [Fact]
        public void ConstructingARayThroughTheCenterOfTheCanvas_ShouldWork()
        {
            var c = new Camera(201, 101, Math.PI/2);
            var r = c.RayForPixel(100, 50);
            Assert.True(r.Origin.Equals(new Point(0, 0, 0)));
            Assert.True(r.Direction.Equals(new Vector(0, 0, -1)));
        }
        
        [Fact]
        public void ConstructingARayThroughACornerOfTheCanvas_ShouldWork()
        {
            var c = new Camera(201, 101, Math.PI/2);
            var r = c.RayForPixel(0, 0);
            Assert.True(r.Origin.Equals(new Point(0, 0, 0)));
            Assert.True(r.Direction.Equals(new Vector(0.66519, 0.33259, -0.66851)));
        }
        
        [Fact]
        public void ConstructingARayWhenTheCameraIsTransformed_ShouldWork()
        {
            var c = new Camera(201, 101, Math.PI/2);
            c.Transform = Transformation.Rotation_y(Math.PI/4) * Transformation.Translation(0, -2, 5);
            var r = c.RayForPixel(100, 50);
            Assert.True(r.Origin.Equals(new Point(0, 2, -5)));
            Assert.True(r.Direction.Equals(new Vector(Math.Sqrt(2)/2, 0, -Math.Sqrt(2)/2)));
        }

        [Fact]
        public void RenderingAWorldWithACamera_ShouldWork()
        {
            var world = new World();
            world.CreateDefaultWorld();
            var c = new Camera(11, 11, Math.PI/2);
            var from = new Point(0, 0, -5);
            var to = new Point(0, 0, 0);
            var up = new Vector(0, 1, 0);
            c.Transform = Transformation.ViewTransform(from, to, up);
            var image = c.Render(world);
            Assert.True(image.GetPixel(5, 5).Equals(new Color(0.38066, 0.47583, 0.2855)));
        }
    }
}
