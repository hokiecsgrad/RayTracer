using System;
using System.Threading;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace RayTracer
{
    public class Camera
    {
        public int HSize { get; set; }

        public int VSize { get; set; }

        public double FieldOfView { get; set; }

        public Matrix Transform { get; set; }

        public double PixelSize { get; private set; }

        public double HalfWidth { get; private set; }

        public double HalfHeight { get; private set; }

        public Camera(int hsize, int vsize, double fov)
        {
            HSize = hsize;
            VSize = vsize;
            FieldOfView = fov;
            Transform = Matrix.Identity;
            CalculatePixelSize();
        }

        private void CalculatePixelSize()
        {
            double halfView = Math.Tan(this.FieldOfView / 2);
            double aspect = (double)this.HSize / (double)this.VSize;
            if (aspect >= 1.0)
            {
                this.HalfWidth = halfView;
                this.HalfHeight = halfView / aspect;
            } 
            else 
            {
                this.HalfWidth = halfView * aspect;
                this.HalfHeight = halfView;
            }
            this.PixelSize = (this.HalfWidth * 2) / this.HSize;
        }

        public Ray RayForPixel(double px, double py)
        {
            // the offset from the edge of the canvas to the pixel's center
            double xoffset = (px + 0.5) * this.PixelSize;
            double yoffset = (py + 0.5) * this.PixelSize;
            // the untransformed coordinates of the pixel in world-space.
            // (remember that the camera looks toward -z, so +x is to the *left*.)
            double world_x = this.HalfWidth - xoffset;
            double world_y = this.HalfHeight - yoffset;
            // using the camera matrix, transform the canvas point and the origin,
            // and then compute the ray's direction vector.
            // (remember that the canvas is at z=-1)
            Point pixel = this.Transform.Inverse() * new Point(world_x, world_y, -1);
            Point origin = this.Transform.Inverse() * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();
            return new Ray(origin, direction);
        }

        public Canvas Render(World world)
        {
            Canvas image = new Canvas(this.HSize, this.VSize);

            Stats.Reset();
            for (int y = 0; y < this.VSize; y++)
                for (int x = 0; x < this.HSize; x++)
                {
                    Interlocked.Increment(ref Stats.PrimaryRays);
                    Ray ray = this.RayForPixel(x, y);
                    Color color = world.ColorAt(ray);
                    image.SetPixel(x, y, color);
                }
            
            return image;
        }
    }
}