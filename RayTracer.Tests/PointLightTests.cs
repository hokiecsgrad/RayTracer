using System;
using Xunit;

namespace RayTracer.Tests
{
    public class PointLightTests
    {
        [Fact]
        public void PointLight_ShouldHavePositionAndIntensity()
        {
            var intensity = new Color(1, 1, 1);
            var position = new Point(0, 0, 0);
            var light = new PointLight(position, intensity);
            Assert.True(light.Position.Equals(position));
            Assert.True(light.Intensity.Equals(intensity));
        }
    }
}