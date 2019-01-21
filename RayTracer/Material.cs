using System;

namespace RayTracer
{
    public class Material
    {
        private const double EPSILON = 0.00001;
        public Color MatColor { get; set; }
        public double Ambient { get; set; }
        public double Diffuse { get; set; }
        public double Specular { get; set; }
        public double Shininess { get; set; }

        public Material()
        {
            MatColor = new Color(1, 1, 1);
            Ambient = 0.1;
            Diffuse = 0.9;
            Specular = 0.9;
            Shininess = 200.0;
        }

        public Color Lighting(PointLight light, Point point, Vector eye, Vector normal)
        {
            Color ambient;
            Color diffuse;
            Color specular;

            // combine the surface color with the light's color/intensity
            var effective_color = this.MatColor * light.Intensity;
            // find the direction to the light source
            var lightv = (light.Position - point).Normalize();
            // compute the ambient contribution
            ambient = effective_color * this.Ambient;
            // light_dot_normal represents the cosine of the angle between the
            // light vector and the normal vector. A negative number means the
            // light is on the other side of the surface.
            var light_dot_normal = lightv.Dot(normal);
            if (light_dot_normal < 0)
            {
                diffuse = new Color(0, 0, 0);
                specular = new Color(0, 0, 0);
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
                    specular = new Color(0, 0, 0);
                else
                {
                    // compute the specular contribution
                    var factor = Math.Pow(reflect_dot_eye, this.Shininess);
                    specular = light.Intensity * this.Specular * factor;
                }
            }
            // Add the three contributions together to get the final shading
            return ambient + diffuse + specular;
        }

        public override bool Equals(Object other)
        {
            Material objMat = other as Material;

            if (objMat == null) {
                return false;
            }
 
            return (objMat.MatColor.Equals(this.MatColor) &&
                    (Math.Abs(objMat.Ambient - this.Ambient) < EPSILON) && 
                    (Math.Abs(objMat.Diffuse - this.Diffuse) < EPSILON) && 
                    (Math.Abs(objMat.Specular - this.Specular) < EPSILON) && 
                    (Math.Abs(objMat.Shininess - this.Shininess) < EPSILON));
        }
    }
}