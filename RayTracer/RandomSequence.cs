using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class RandomSequence : ISequence
    {
        private readonly Random rng = new Random();

        public double Next()
        {
            return this.rng.NextDouble();
        }
    }
}
