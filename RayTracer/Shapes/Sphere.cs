using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Sphere : Shape
    {
        public override List<Intersection> LocalIntersect(Ray r)
        {
            var shapeToRay = r.Origin - Point.Origin;

            double a = r.Direction.Dot(r.Direction);
            double b = 2 * r.Direction.Dot(shapeToRay);
            double c = shapeToRay.Dot(shapeToRay) - 1;

            double discriminant = b * b - 4 * a * c;
            if ( discriminant < 0 )
                return new List<Intersection>();

            double t1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            double t2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

            return new List<Intersection> { 
                new Intersection(t1, this), 
                new Intersection(t2, this) };
        }

        public override Vector LocalNormalAt(Point local_point, Intersection hit = null) =>
            local_point - Point.Origin;    


        public override BoundingBox GetBounds() =>
            new BoundingBox(
                new Point(-1, -1, -1), 
                new Point(1, 1, 1));

        public override void Divide(int threshold) { }
    }
}