using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class World
    {
        public PointLight Light { get; set; }
        public List<Sphere> Shapes { get; set; }

        public World() {}

        public void CreateDefaultWorld()
        {
            Light = new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1));
            var s1 = new Sphere();
            var m = new Material(new Color(0.8, 1.0, 0.6), 0.1, 0.7, 0.2, 200.0);
            s1.Material = m;
            var s2 = new Sphere();
            s2.Transform = Transformation.Scaling(0.5, 0.5, 0.5);
            Shapes = new List<Sphere> {s1, s2};
        }

        public List<Intersection> Intersect(Ray ray)
        {
            List<Intersection> intersections = new List<Intersection>();
            foreach (var shape in Shapes)
                intersections.AddRange(ray.Intersect(shape));
            intersections = intersections.OrderBy(i => i.Time).ToList();
            return intersections;
        }

        public Color ShadeHit(Comps comps)
        {
            // For multiple world level lights, loop over the lights and call this multiple times
            return comps.Object.Material.Lighting(this.Light, comps.Point, comps.Eye, comps.Normal);
        }

        public Color ColorAt(Ray ray)
        {
            var intersections = this.Intersect(ray);
            if (intersections.Count == 0) return new Color(0,0,0);
            var hit = ray.Hit(intersections);
            var comps = hit.PrepareComputations(ray);
            return ShadeHit(comps);
        }
    }
}
