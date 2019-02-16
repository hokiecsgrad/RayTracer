using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    // TODO: I don't really understand Structs.  I had to actually set the values of n1 and n2 
    // in the below class where I first create the Comps struct before I could use it.
    public struct Comps
    {
        public double Time;
        public Shape Object;
        public Point Point;
        public Vector Eye;
        public Vector Normal;
        public bool Inside;
        public Point OverPoint;
        public Point UnderPoint;
        public Vector Reflect;
        public double n1;
        public double n2;

        public Comps(double time, Shape shape, Point point, Vector eye, Vector norm, bool inside, Point over_point, Point under_point, Vector reflect, double nOne, double nTwo)
        {
            Time = time;
            Object = shape;
            Point = point;
            Eye = eye;
            Normal = norm;
            Inside = inside;
            OverPoint = over_point;
            UnderPoint = under_point;
            Reflect = reflect;
            n1 = nOne;
            n2 = nTwo;
        }
    }


    public class Intersection : IComparable
    {
        private const double EPSILON = 0.00001;
        public double Time { get; }
        public Shape Object { get; }

        public Intersection(double t, Shape obj)
        {
            Time = t;
            Object = obj;
        }

        public Comps PrepareComputations(Ray ray, List<Intersection> xs)
        {
            Comps comps;
            comps.n1 = 0.0;
            comps.n2 = 0.0;
            comps.Time = this.Time;
            comps.Object = this.Object;
            comps.Point = ray.Position(comps.Time);
            comps.Eye = -ray.Direction;
            comps.Normal = comps.Object.NormalAt(comps.Point);

            if (comps.Normal.Dot(comps.Eye) < 0)
            {
                comps.Inside = true;
                comps.Normal = -comps.Normal;
            }
            else
                comps.Inside = false;

            comps.Reflect = ray.Direction.Reflect(comps.Normal);

            var containers = new List<Shape>();
            foreach (var i in xs)
            {
                if (i == this) {
                    if (containers.Count == 0)
                        comps.n1 = 1.0;
                    else
                        comps.n1 = containers.Last().Material.RefractiveIndex;
                }

                if ( containers.Contains(i.Object) )
                    containers.Remove(i.Object);
                else
                    containers.Add(i.Object);
                
                if (i == this) {
                    if (containers.Count == 0)
                        comps.n2 = 1.0;
                    else
                        comps.n2 = containers.Last().Material.RefractiveIndex;
                    break;
                }
            }

            comps.OverPoint = comps.Point + comps.Normal * EPSILON;
            comps.UnderPoint = comps.Point - comps.Normal * EPSILON;

            return comps;
        }

        public int CompareTo(object value)
        {
            return this.Time.CompareTo((value as Intersection).Time);
        }
    }
}