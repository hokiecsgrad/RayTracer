using System;
using Xunit;

namespace RayTracer.Tests
{
    public class IntersectionTests
    {
        [Fact]
        public void CreatingIntersection_ShouldWork()
        {
            var sphere = new Sphere();
            var intersection = new Intersection(3.5, sphere);
            Assert.True(intersection.Time == 3.5);
            Assert.True(intersection.Object == sphere);
        }
    }
}
