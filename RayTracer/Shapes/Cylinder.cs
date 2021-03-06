using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Cylinder : Shape
    {
        private const double EPSILON = 0.00001;

        public double Maximum { get; set; } = double.PositiveInfinity;

        public double Minimum { get; set; } = double.NegativeInfinity;

        public bool IsClosed { get; set; } = false;


        // a helper function to reduce duplication.
        // checks to see if the intersection at `t` is within a radius
        // of 1 (the radius of your cylinders) from the y axis.
        private bool CheckCap(Ray ray, double t)
        {
            var x = ray.Origin.x + t * ray.Direction.x;
            var z = ray.Origin.z + t * ray.Direction.z;
            return (x * x + z * z) <= 1;
        }
        
        private List<Intersection> IntersectCaps(Ray ray)
        {
            var xs = new List<Intersection>();

            // caps only matter if the cylinder is closed, and might possibly be
            // intersected by the ray.
            if (!this.IsClosed)
                return xs;

            if (Math.Abs(ray.Direction.y) < EPSILON)
                return xs;

            // check for an intersection with the lower end cap by intersecting
            // the ray with the plane at y=cyl.minimum
            var t = (this.Minimum - ray.Origin.y) / ray.Direction.y;
            if (this.CheckCap(ray, t))
            {
                xs.Add( new Intersection(t, this) );
            }

            // check for an intersection with the upper end cap by intersecting
            // the ray with the plane at y=cyl.maximum
            t = (this.Maximum - ray.Origin.y) / ray.Direction.y;
            if (this.CheckCap(ray, t))
            {
                xs.Add( new Intersection(t, this) );
            }

            return xs;
        }

        public override List<Intersection> LocalIntersect(Ray ray)
        {
            var xs = new List<Intersection>();

            var a = Math.Pow(ray.Direction.x, 2) + Math.Pow(ray.Direction.z, 2);

            // ray is parallel to the y axis
            if (Math.Abs(a) > EPSILON) 
            {
                var b = 
                    2 * ray.Origin.x * ray.Direction.x + 
                    2 * ray.Origin.z * ray.Direction.z;

                var c = Math.Pow(ray.Origin.x, 2) + Math.Pow(ray.Origin.z, 2) - 1;

                var disc = b * b - 4 * a * c;
                // ray does not intersect the cylinder
                if (disc < 0) return new List<Intersection>();
                
                var t0 = (-b - Math.Sqrt(disc)) / (2 * a);
                var t1 = (-b + Math.Sqrt(disc)) / (2 * a);

                if (t0 > t1) 
                {
                    var temp = t0;
                    t0 = t1;
                    t1 = temp;
                }

                var y0 = ray.Origin.y + t0 * ray.Direction.y;
                if (this.Minimum < y0 && y0 < this.Maximum)
                    xs.Add(new Intersection(t0, this));
                
                var y1 = ray.Origin.y + t1 * ray.Direction.y;
                if (this.Minimum < y1 && y1 < this.Maximum)
                    xs.Add(new Intersection(t1, this));
            }

            xs.AddRange( IntersectCaps(ray) );

            return xs;
        }

        public override Vector LocalNormalAt(Point local_point, Intersection hit = null)
        {
            var dist = Math.Pow(local_point.x, 2) + Math.Pow(local_point.z, 2);

            if (dist < 1 && local_point.y >= this.Maximum - EPSILON)
            {
                return new Vector(0, 1, 0);
            }
            else if (dist < 1 && local_point.y <= this.Minimum + EPSILON)
            {
                return new Vector(0, -1, 0);
            }

            return new Vector(local_point.x, 0, local_point.z);
        }

        public override BoundingBox GetBounds() =>
            new BoundingBox(
                new Point(-1, Minimum, -1), 
                new Point(1, Maximum, 1));

        public override void Divide(int threshold) { }
    }
}