using System;

namespace RayTracer
{
    public class Intersection : IComparable
    {
        public double Time { get; }
        public Sphere Object { get; }

        public Intersection(double t, Sphere obj)
        {
            Time = t;
            Object = obj;
        }

        public int CompareTo(object value)
        {
            return this.Time.CompareTo((value as Intersection).Time);
        }
        
    }
}