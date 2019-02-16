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

        public Ray Transform(Matrix transform)
        {
            var newLocation = transform * Origin;
            var newDirection = transform * Direction;
            return new Ray(newLocation, newDirection);
        }

        // TODO: Moved method Hit from Ray to World b/c it didn't really
        // make sense in the Ray class, but now that I'm using it in the
        // world class, it doesn't really make sense here either.  Find a
        // home for this method.
        //
        // Moved back to Ray because reasons.
        public Intersection Hit(List<Intersection> intersections)
        {
            intersections = intersections.OrderBy(i => i.Time).ToList();
            for (int i = 0; i < intersections.Count; i++)
                if (intersections[i].Time >= 0.0) return intersections[i];
            return null;
        }
    }
}