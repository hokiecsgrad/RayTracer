using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class Ray
    {
        public Point Origin { get; }
        public Vector Direction { get; }

        public Ray(Point origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Point Position(double t)
        {
            Point newPosition = Origin + Direction * t;
            return newPosition;
        }

        public List<Intersection> Intersect(Sphere s)
        {
            Ray transformedRay = Transform(s.Transform.Inverse());
            var sphereToRay = transformedRay.Origin - s.Origin;
            double a = transformedRay.Direction.Dot(transformedRay.Direction);
            double b = 2 * transformedRay.Direction.Dot(sphereToRay);
            double c = sphereToRay.Dot(sphereToRay) - 1;
            double discriminant = b*b - 4 * a * c;
            if ( discriminant < 0 )
                return new List<Intersection>();

            double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            return new List<Intersection> { new Intersection(t1, s), new Intersection(t2, s) };
        }

        public Intersection Hit(List<Intersection> intersections)
        {
            // Todo: Is this in the right place?  It doesn't use anything
            // from Ray class.
            intersections = intersections.OrderBy(i => i.Time).ToList();
            for (int i = 0; i < intersections.Count; i++)
                if (intersections[i].Time >= 0.0) return intersections[i];
            return null;
        }

        public Ray Transform(Matrix transform)
        {
            var newLocation = transform * Origin;
            var newDirection = transform * Direction;
            return new Ray(newLocation, newDirection);
        }
    }
}