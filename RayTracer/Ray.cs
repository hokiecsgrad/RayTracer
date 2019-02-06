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
    }
}