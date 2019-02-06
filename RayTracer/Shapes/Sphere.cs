using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Sphere : Shape
    {
        public Point Origin { get; }
        public double Radius { get; }

        public Sphere()
        {
            Origin = new Point(0, 0, 0);
            Radius = 1.0;
        }

        public override List<Intersection> Intersect(Ray r)
        {
            Ray transformedRay = r.Transform(this.Transform.Inverse());
            var shapeToRay = transformedRay.Origin - this.Origin;
            double a = transformedRay.Direction.Dot(transformedRay.Direction);
            double b = 2 * transformedRay.Direction.Dot(shapeToRay);
            double c = shapeToRay.Dot(shapeToRay) - 1;
            double discriminant = b*b - 4 * a * c;
            if ( discriminant < 0 )
                return new List<Intersection>();

            double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            return new List<Intersection> { new Intersection(t1, this), new Intersection(t2, this) };
        }
    }
}