/*
    Code originally authored by @basp on Github
    https://github.com/basp/pixie.net/blob/master/src/Pixie.Core/ApproxEqualityComparer.cs
*/

using System;
using System.Collections.Generic;

namespace RayTracer
{
    internal abstract class ApproxEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly double epsilon;

        public ApproxEqualityComparer(double epsilon = 0.000001)
        {
            this.epsilon = epsilon;
        }

        public abstract bool Equals(T x, T y);

        public abstract int GetHashCode(T obj);

        protected bool ApproxEqual(double v1, double v2) =>
            Math.Abs(v1 - v2) < this.epsilon;
    }
}