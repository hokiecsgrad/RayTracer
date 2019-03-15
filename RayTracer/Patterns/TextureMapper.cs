using System;

namespace RayTracer
{
    public class TextureMapper
    {
        public static (double, double) SphericalMap(Point point)
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
        public static double Modulus(double a, double b)
        {
            return a - b * Math.Floor(a / b);
        }
        
        public static (double, double) PlanarMap(Point point)
        {
            var u = Modulus(point.x, 1.0);
            var v = Modulus(point.z, 1.0);

            return (u, v);
        }

        public static (double, double) CylindricalMap(Point point)
        {
            // compute the azimuthal angle, same as with SphericalMap()
            var theta = Math.Atan2(point.x, point.z);
            var raw_u = theta / (2 * Math.PI);
            var u = 1 - (raw_u + 0.5);

            // let v go from 0 to 1 between whole units of y
            var v = Modulus(point.y, 1);

            return (u, v);
        }

        public static CubeFace FaceFromPoint(Point point)
        {
            var abs_x = Math.Abs(point.x);
            var abs_y = Math.Abs(point.y);
            var abs_z = Math.Abs(point.z);
            var coord = Math.Max(abs_x, Math.Max(abs_y, abs_z));

            if (coord == point.x) return CubeFace.Right;
            if (coord == -point.x) return CubeFace.Left;
            if (coord == point.y) return CubeFace.Up;
            if (coord == -point.y) return CubeFace.Down;
            if (coord == point.z) return CubeFace.Front;

            return CubeFace.Back;
        }

        public static (double, double) CubeUvFront(Point point)
        {
            var u = Modulus((point.x + 1), 2.0) / 2.0;
            var v = Modulus((point.y + 1), 2.0) / 2.0;
            return (u, v);
        }

        public static (double, double) CubeUvBack(Point point)
        {
            var u = Modulus((1 - point.x), 2.0) / 2.0;
            var v = Modulus((point.y + 1), 2.0) / 2.0;
            return (u, v);
        }

        public static (double, double) CubeUvLeft(Point point)
        {
            var u = Modulus((point.z + 1), 2.0) / 2.0;
            var v = Modulus((point.y + 1), 2.0) / 2.0;
            return (u, v);
        }

        public static (double, double) CubeUvRight(Point point)
        {
            var u = Modulus((1 - point.z), 2.0) / 2.0;
            var v = Modulus((point.y + 1), 2.0) / 2.0;
            return (u, v);
        }

        public static (double, double) CubeUvUp(Point point)
        {
            var u = Modulus((point.x + 1), 2.0) / 2.0;
            var v = Modulus((1 - point.z), 2.0) / 2.0;
            return (u, v);
        }

        public static (double, double) CubeUvDown(Point point)
        {
            var u = Modulus((point.x + 1), 2.0) / 2.0;
            var v = Modulus((point.z + 1), 2.0) / 2.0;
            return (u, v);
        }
    }
}