using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class SequenceTests
    {
        [Fact]
        public void NumberGenerator_ShouldReturnsCyclicSequenceOfNumbers()
        {
            var gen = new Sequence(new List<double> { 0.1, 0.5, 1.0 });
            Assert.Equal(0.1, gen.Next());
            Assert.Equal(0.5, gen.Next());
            Assert.Equal(1.0, gen.Next());
            Assert.Equal(0.1, gen.Next());
        }
    }
}