using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class World
    {
        public PointLight Light { get; set; }
        public List<Shape> Shapes { get; set; }

        public World() {}

        public void CreateDefaultWorld()
        {
            Light = new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1));
            var s1 = new Sphere();
            var m = new Material(new Color(0.8, 1.0, 0.6), 0.1, 0.7, 0.2, 200.0);
            s1.Material = m;
            var s2 = new Sphere();
            s2.Transform = Transformation.Scaling(0.5, 0.5, 0.5);
            Shapes = new List<Shape> {s1, s2};
        }

        public List<Intersection> Intersect(Ray ray)
        {
            List<Intersection> intersections = new List<Intersection>();
            foreach (var shape in Shapes)
                intersections.AddRange(shape.Intersect(ray));
            intersections = intersections.OrderBy(i => i.Time).ToList();
            return intersections;
        }

        public Color ShadeHit(Comps comps, int remaining = 5)
        {
            var shadowed = this.IsShadowed(comps.OverPoint);
            // TODO: For multiple world level lights, loop over the lights and call this multiple times
            var surface = comps.Object.Material.Lighting(comps.Object, this.Light, comps.Point, comps.Eye, comps.Normal, shadowed);
            var reflected = this.ReflectedColor(comps, remaining);
            return surface + reflected;
        }

        // TODO: Moved method Hit from Ray to World b/c it didn't really
        // make sense in the Hit class, but now that I'm using it in the
        // world class, it doesn't really make sense here either.  Find a
        // home for this method.
        public Intersection Hit(List<Intersection> intersections)
        {
            intersections = intersections.OrderBy(i => i.Time).ToList();
            for (int i = 0; i < intersections.Count; i++)
                if (intersections[i].Time >= 0.0) return intersections[i];
            return null;
        }

        public Color ReflectedColor(Comps comps, int remaining = 5)
        {
            if (remaining < 1 || comps.Object.Material.Reflective == 0)
                return new Color(0, 0, 0);

            var reflectRay = new Ray(comps.OverPoint, comps.Reflect);
            var color = this.ColorAt(reflectRay, remaining - 1);
            return color * comps.Object.Material.Reflective;
        }

        public Color ColorAt(Ray ray, int remaining = 5)
        {
            var intersections = this.Intersect(ray);
            if (intersections.Count == 0) return new Color(0,0,0);
            var hit = this.Hit(intersections);
            if (hit == null) return new Color(0,0,0);
            var comps = hit.PrepareComputations(ray);
            return ShadeHit(comps, remaining);
        }

        public bool IsShadowed(Point point)
        {
            var v = this.Light.Position - point;
            var distance = v.Magnitude();
            var direction = v.Normalize();
            var r = new Ray(point, direction);
            var intersections = this.Intersect(r);
            var h = this.Hit(intersections);
            if (h != null && h.Time < distance)
                return true;
            else
                return false;
        }
    }
}
