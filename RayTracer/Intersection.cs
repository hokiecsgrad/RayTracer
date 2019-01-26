using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public struct Comps
    {
        public double Time;
        public Sphere Object;
        public Point Point;
        public Vector Eye;
        public Vector Normal;
        public bool Inside;
        public Point OverPoint;

        public Comps(double time, Sphere shape, Point point, Vector eye, Vector norm, bool inside, Point over_point)
        {
            Time = time;
            Object = shape;
            Point = point;
            Eye = eye;
            Normal = norm;
            Inside = inside;
            OverPoint = over_point;
        }
    }


    public class Intersection : IComparable
    {
        private const double EPSILON = 0.00001;
        public double Time { get; }
        public Sphere Object { get; }

        public Intersection(double t, Sphere obj)
        {
            Time = t;
            Object = obj;
        }

        public Comps PrepareComputations(Ray ray)
        {
            Comps comps;
            comps.Time = this.Time;
            comps.Object = this.Object;
            comps.Point = ray.Position(comps.Time);
            comps.Eye = -ray.Direction;
            comps.Normal = comps.Object.Normal_at(comps.Point);

            if (comps.Normal.Dot(comps.Eye) < 0)
            {
                comps.Inside = true;
                comps.Normal = -comps.Normal;
            }
            else
                comps.Inside = false;

            comps.OverPoint = comps.Point + comps.Normal * EPSILON;

            return comps;
        }

        public int CompareTo(object value)
        {
            return this.Time.CompareTo((value as Intersection).Time);
        }
    }
}