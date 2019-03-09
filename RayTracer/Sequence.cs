using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Sequence : ISequence
    {
        private List<double> Numbers { get; set; }
        private int CurrIndex { get; set; }

        public Sequence(List<double> numbers)
        {
            this.Numbers = numbers;
            this.CurrIndex = 0;
        }

        public double Next()
        {
            return this.Numbers[CurrIndex++ % Numbers.Count];
        }
    }
}