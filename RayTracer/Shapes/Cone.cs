using System;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer
{
    public class Cone : Shape
    {
        private const double EPSILON = 0.00001;

        public double Maximum { get; set; } = double.PositiveInfinity;

        public double Minimum { get; set; } = double.NegativeInfinity;

        public bool IsClosed { get; set; } = false;

        // a helper function to reduce duplication.
        // checks to see if the intersection at `t` is within a radius
        // of y (the radius of your cone) from the y axis.
        private bool CheckCap(Ray ray, double t, double y)
        {
            var x = ray.Origin.x + t * ray.Direction.x;
            var z = ray.Origin.z + t * ray.Direction.z;
            return (x*x + z*z <= Math.Abs(y));
        }
        
        private List<Intersection> IntersectCaps(Ray ray)
        {
            var xs = new List<Intersection>();

            // caps only matter if the cone is closed, and might possibly be
            // intersected by the ray.
            if (!this.IsClosed)
                return xs;

            if (ray.Direction.y < EPSILON)
                return xs;

            // check for an intersection with the lower end cap by intersecting
            // the ray with the plane at y=cone.minimum
            var t = (this.Minimum - ray.Origin.y) / ray.Direction.y;
            if (CheckCap(ray, t, this.Minimum))
                xs.Add( new Intersection(t, this) );

            // check for an intersection with the upper end cap by intersecting
            // the ray with the plane at y=cone.maximum
            t = (this.Maximum - ray.Origin.y) / ray.Direction.y;
            if (CheckCap(ray, t, this.Maximum))
                xs.Add( new Intersection(t, this) );

            return xs;
        }

        public override List<Intersection> LocalIntersect(Ray ray)
        {
            var xs = new List<Intersection>();

            var o = ray.Origin;
            var d = ray.Direction;

            var a = d.x * d.x - d.y * d.y + d.z * d.z;
            var b = 2 * o.x * d.x - 2 * o.y * d.y + 2 * o.z * d.z;
            var c = o.x * o.x - o.y * o.y + o.z * o.z;

            if (Math.Abs(0 - a) <= EPSILON && Math.Abs(0 - b) > EPSILON)
            {
                var t0 = -c / (2 * b);
                xs.Add(new Intersection(t0, this));
            }
            else if (Math.Abs(0 - a) > EPSILON) 
            {
                var disc = b*b - 4 * a * c;
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
            // compute the square of the distance from the y axis
            var dist = Math.Pow(local_point.x, 2) + Math.Pow(local_point.z, 2);
            if (dist < 1 && local_point.y >= this.Maximum - EPSILON)
            {
                return new Vector(0, 1, 0);
            }
            else if (dist < 1 && local_point.y <= this.Minimum + EPSILON)
            {
                return new Vector(0, -1, 0);
            }

            var y = Math.Sqrt(dist);
            if (local_point.y > 0) 
            {
                y = -y;
            }

            return new Vector(local_point.x, y, local_point.z);
        }

        public override BoundingBox GetBounds()
        {
            var limit = Math.Max(
                Math.Abs(Minimum),
                Math.Abs(Maximum));

            return new BoundingBox(
                new Point(-limit, Minimum, -limit),
                new Point(limit, Maximum, limit));
        }

        public override void Divide(int threshold) { }
    }
}