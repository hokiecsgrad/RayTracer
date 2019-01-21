using System;

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

        public Intersection[] Intersect(Sphere s)
        {
            var sphereToRay = Origin - s.Origin;
            double a = Direction.Dot(Direction);
            double b = 2 * Direction.Dot(sphereToRay);
            double c = sphereToRay.Dot(sphereToRay) - 1;
            double discriminant = b*b - 4 * a * c;
            if ( discriminant < 0 )
                return new Intersection[] {};

            double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            return new Intersection[] { new Intersection(t1, s), new Intersection(t2, s) };
        }

        public Intersection Hit(Intersection[] intersections)
        {
            Array.Sort(intersections);
            for (int i = 0; i < intersections.Length; i++)
                if (intersections[i].Time >= 0.0) return intersections[i];
            return null;
        }
    }
}