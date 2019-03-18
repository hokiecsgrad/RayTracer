using System;

namespace RayTracer
{
    public class Material : IEquatable<Material>
    {
        public Color Color { get; set; }

        public Pattern Pattern { get; set; }
        public Pattern SpecularMap { get; set; }
        public Pattern BumpMap { get; set; }

        public double Ambient { get; set; }

        public double Diffuse { get; set; }

        public double Specular { get; set; }

        public double Shininess { get; set; }

        public double Reflective { get; set; }

        public double Transparency { get; set; }

        public double RefractiveIndex { get; set; }


        public Material()
        {
            Color = Color.White;
            Pattern = null;
            SpecularMap = null;
            BumpMap = null;
            Ambient = 0.1;
            Diffuse = 0.9;
            Specular = 0.9;
            Shininess = 200.0;
            Reflective = 0.0;
            Transparency = 0.0;
            RefractiveIndex = 1.0;
        }

        public Material(Color color, double ambient, double diffuse, double specular, double shininess)
        {
            Color = color;
            Pattern = null;
            SpecularMap = null;
            BumpMap = null;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
            Reflective = 0.0;
            Transparency = 0.0;
            RefractiveIndex = 1.0;
        }

        public Material(Color color, Pattern pattern, double ambient, double diffuse, double specular, double shininess)
        {
            Color = color;
            Pattern = pattern;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
            Reflective = 0.0;
            Transparency = 0.0;
            RefractiveIndex = 1.0;
        }

        public Color Lighting(Shape shape, ILight light, Point point, Vector eye, Vector normal, double intensity = 1.0)
        {
            Color ambient;
            Color diffuse;
            Color specular;
            Color specColor;
            Color color;

            if (this.Pattern != null)
            {
                color = this.Pattern.PatternAtShape(shape, point);
                specColor = this.SpecularMap.PatternAtShape(shape, point);
            }
            else
            {
                color = this.Color;
                specColor = Color.Black;
            }

            // combine the surface color with the light's color/intensity
            var effective_color = color * light.Color;
            // compute the ambient contribution
            ambient = effective_color * this.Ambient;

            var sum = Color.Black;
            foreach (Point sample in light.Sample())
            {
                // find the direction to the light source
                var lightv = (sample - point).Normalize();
                // light_dot_normal represents the cosine of the angle between the
                // light vector and the normal vector. A negative number means the
                // light is on the other side of the surface.
                var light_dot_normal = lightv.Dot(normal);
                if (light_dot_normal < 0)
                {
                    diffuse = Color.Black;
                    specular = Color.Black;
                } 
                else 
                {
                    // compute the diffuse contribution
                    diffuse = effective_color * this.Diffuse * light_dot_normal;
                    // reflect_dot_eye represents the cosine of the angle between the
                    // reflection vector and the eye vector. A negative number means the
                    // light reflects away from the eye.
                    var reflect = -lightv.Reflect(normal);
                    var reflect_dot_eye = reflect.Dot(eye);
                    if (reflect_dot_eye <= 0)
                        specular = Color.Black;
                    else
                    {
                        // compute the specular contribution
                        var factor = Math.Pow(reflect_dot_eye, this.Shininess);
                        if (this.SpecularMap != null)
                            specular = light.Color * this.Specular * specColor * factor;
                        else 
                            specular = light.Color * this.Specular * factor;
                    }
                }
                sum = sum + diffuse;
                sum = sum + specular;
            }

            return ambient + (sum / light.Samples) * intensity;
        }

        public bool Equals(Material other) =>
            this.Ambient == other.Ambient &&
            this.Diffuse == other.Diffuse &&
            this.Specular == other.Specular &&
            this.Shininess == other.Shininess &&
            this.Color.Equals(other.Color);
    }
}