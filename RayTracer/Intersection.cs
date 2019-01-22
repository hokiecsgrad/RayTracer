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

        public Comps(double time, Sphere shape, Point point, Vector eye, Vector norm, bool inside)
        {
            Time = time;
            Object = shape;
            Point = point;
            Eye = eye;
            Normal = norm;
            Inside = inside;
        }
    }


    public class Intersection : IComparable
    {
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

            return comps;
        }

        public int CompareTo(object value)
        {
            return this.Time.CompareTo((value as Intersection).Time);
        }
    }
}