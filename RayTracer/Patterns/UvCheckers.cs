using System;

namespace RayTracer
{
    public class UvCheckers
    {
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public Color a { get; set; } = Color.White;
        public Color b { get; set; } = Color.Black;

        public UvCheckers(int width, int height, Color a, Color b)
        {
            this.Width = width;
            this.Height = height;
            this.a = a;
            this.b = b;
        }

        public Color UvPatternAt(double u, double v)
        {
            var u2 = Math.Floor(u * this.Width);
            var v2 = Math.Floor(v * this.Height);

            if ((u2 + v2) % 2 == 0)
                return this.a;
            else
                return this.b;
        }

        public (double, double) SphericalMap(Point point)
        {
            // compute the azimuthal angle
            // -π < theta <= π
            // angle increases clockwise as viewed from above,
            // which is opposite of what we want, but we'll fix it later.
            var theta = Math.Atan2(point.x, point.z);

            var radius = point.Magnitude();

            // compute the polar angle
            // 0 <= phi <= π
            var phi = Math.Acos(point.y / radius);

            // -0.5 < raw_u <= 0.5
            var raw_u = theta / (2 * Math.PI);

            // 0 <= u < 1
            // here's also where we fix the direction of u. Subtract it from 1,
            // so that it increases counterclockwise as viewed from above.
            var u = 1 - (raw_u + 0.5);

            // we want v to be 0 at the south pole of the sphere,
            // and 1 at the north pole, so we have to "flip it over"
            // by subtracting it from 1.
            var v = 1 - (phi / Math.PI);

            return (u, v);
        }


        // So, C#'s Mod operator (%) is actually a "remainder" 
        // operator.  This method is the correct formula for
        // calculating Modulus, which was important here.
        private double Modulus(double a, double b)
        {
            return a - b * Math.Floor(a / b);
        }
        
        public (double, double) PlanarMap(Point point)
        {
            var u = Modulus(point.x, 1.0);
            var v = Modulus(point.z, 1.0);

            return (u, v);
        }

        public (double, double) CylindricalMap(Point point)
        {
            // compute the azimuthal angle, same as with SphericalMap()
            var theta = Math.Atan2(point.x, point.z);
            var raw_u = theta / (2 * Math.PI);
            var u = 1 - (raw_u + 0.5);

            // let v go from 0 to 1 between whole units of y
            var v = Modulus(point.y, 1);

            return (u, v);
        }
    }
}