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

        // This might help clear some of this stuff up:
        // http://graphics.stanford.edu/courses/cs148-10-summer/docs/2006--degreve--reflection_refraction.pdf
        public double Schlick()
        {
            // find the cosine of the angle between the eye and normal vectors
            var cos = Eye.Dot(Normal);
            // total internal reflection can only occur if n1 > n2
            if (n1 > n2)
            {
                var n = n1 / n2;
                var sin2_t = n*n * (1.0 - cos*cos);
                if (sin2_t > 1.0)
                    return 1.0;

                // compute cosine of theta_t using trig identity
                var cos_t = Math.Sqrt(1.0 - sin2_t);
                // when n1 > n2, use cos(theta_t) instead
                cos = cos_t;
            }
            var r0 = Math.Pow((n1 - n2) / (n1 + n2), 2);
            return r0 + (1 - r0) * Math.Pow(1 - cos, 5);
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