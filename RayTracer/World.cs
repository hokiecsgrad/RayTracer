using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RayTracer
{
    public class World
    {
        public List<ILight> Lights { get; set; }
        public List<Shape> Shapes { get; set; }

        public World() {}

        public void CreateDefaultWorld()
        {
            this.Lights = new List<ILight> { new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)) };
            var s1 = new Sphere();
            var m = new Material(new Color(0.8, 1.0, 0.6), 0.1, 0.7, 0.2, 200.0);
            s1.Material = m;
            var s2 = new Sphere();
            s2.Transform = Transformation.Scaling(0.5, 0.5, 0.5);
            Shapes = new List<Shape> {s1, s2};
        }


        public List<Intersection> Intersect(Ray ray)
        {
            Interlocked.Increment(ref Stats.Tests);
            var xs = this.Shapes
                .Where(obj => (obj.HitBy & ray.Type) != 0)
                .SelectMany(x => x.Intersect(ray));
            return xs.OrderBy(i => i.Time).ToList();
        }

        public Color ShadeHit(Comps comps, int remaining = 5)
        {
            var result = Color.Black;

            foreach (var light in this.Lights)
            {
                var surface = comps.Object.Material.Lighting(
                    comps.Object, 
                    light, 
                    comps.OverPoint, 
                    comps.Eye, 
                    comps.Normal, 
                    light.IntensityAt(comps.OverPoint, this));
            
                var reflected = this.ReflectedColor(comps, remaining);
                var refracted = this.RefractedColor(comps, remaining);

                var material = comps.Object.Material;
                if (material.Reflective > 0 && material.Transparency > 0)
                {
                    var reflectance = comps.Schlick();
                    result += surface + reflected * reflectance + refracted * (1 - reflectance);
                } 
                else
                    result += surface + reflected + refracted;
            }

            return result;
        }

        public Color ReflectedColor(Comps comps, int remaining = 5)
        {
            if (remaining < 1 || comps.Object.Material.Reflective == 0)
                return Color.Black;
            
            var specularComp = Color.White;
            if (comps.Object.Material.SpecularMap != null)
                specularComp = comps.Object.Material.SpecularMap.PatternAtShape(comps.Object, comps.OverPoint);

            Interlocked.Increment(ref Stats.SecondaryRays);
            var reflectRay = new Ray(comps.OverPoint, comps.Reflect, RayType.Reflection);
            var color = this.ColorAt(reflectRay, remaining - 1);
            return color * comps.Object.Material.Reflective * specularComp;
        }

        public Color RefractedColor(Comps comps, int remaining = 5)
        {
            if (remaining < 1 || comps.Object.Material.Transparency == 0)
                return Color.Black;

            // Find the ratio of first index of refraction to the second.
            // (Yup, this is inverted from the definition of Snell's Law.)
            var n_ratio = comps.n1 / comps.n2;
            // cos(theta_i) is the same as the dot product of the two vectors
            var cos_i = comps.Eye.Dot(comps.Normal);
            // Find sin(theta_t)^2 via trigonometric identity
            var sin2_t = n_ratio*n_ratio * (1 - cos_i*cos_i);
            if (sin2_t > 1)
                return Color.Black;

            Interlocked.Increment(ref Stats.SecondaryRays);
            // Find cos(theta_t) via trigonometric identity
            var cos_t = Math.Sqrt(1.0 - sin2_t);
            // Compute the direction of the refracted ray
            var direction = comps.Normal * (n_ratio * cos_i - cos_t) - comps.Eye * n_ratio;
            // Create the refracted ray
            var refract_ray = new Ray(comps.UnderPoint, direction, RayType.Refraction);
            // Find the color of the refracted ray, making sure to multiply
            // by the transparency value to account for any opacity
            return this.ColorAt(refract_ray, remaining - 1) * comps.Object.Material.Transparency;
        }

        public Color ColorAt(Ray ray, int remaining = 5)
        {
            var intersections = new List<Intersection>();
            intersections = this.Intersect(ray);
            if (intersections.Count == 0) return new Color(0,0,0);

            var hit = ray.Hit(intersections);
            if (hit == null) return new Color(0,0,0);

            var comps = hit.PrepareComputations(ray, intersections);
            return ShadeHit(comps, remaining);
        }

        public bool IsShadowed(Point origin, Point light)
        {
            Interlocked.Increment(ref Stats.ShadowRays);
            var v = light - origin;
            var distance = v.Magnitude();
            var direction = v.Normalize();
            var r = new Ray(origin, direction, RayType.Shadow);
            var intersections = this.Intersect(r);
            var h = r.Hit(intersections);
            
            if (h != null && h.Time < distance)
                return true;
            else
                return false;
        }
    }
}
