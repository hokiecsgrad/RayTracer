using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Camera
    {
        private static readonly Random rng = new Random();

        public int HSize { get; }

        public int VSize { get; }

        public double FieldOfView { get; }

        private Matrix _transform = Matrix.Identity;
        private Matrix _transformInverse = Matrix.Identity;

        public Matrix Transform 
        { 
            get { return _transform; } 
            set 
            {
                this._transform = value;
                this._transformInverse = this._transform.Inverse();
            }
        }

        public Matrix TransformInverse => this._transformInverse;

        public double PixelSize { get; private set; }

        public double HalfWidth { get; private set; }

        public double HalfHeight { get; private set; }

        public IProgressMonitor ProgressMonitor { get; set; } =
            new ProgressMonitor();


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

        public List<Ray> RayForPixel(double px, double py)
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
            Point pixel = this.TransformInverse * new Point(world_x, world_y, -1);
            Point origin = this.TransformInverse * new Point(0, 0, 0);
            Vector direction = (pixel - origin).Normalize();

            return new List<Ray> { new Ray(origin, direction) };
        }

        public List<Ray> RaysForPixel(double px, double py)
        {
            var rays = new List<Ray>();
            var inv = this.TransformInverse;
            var origin = inv * new Point(0, 0, 0);

            var pixelSize = this.PixelSize;
            var halfWidth = this.HalfWidth;
            var halfHeight = this.HalfHeight;

            for (var i = 0; i < 4; i++)
            {
                var xOffset = (px + 0.5);
                var yOffset = (py + 0.5);

                var rx = rng.NextDouble();
                var ry = rng.NextDouble();

                xOffset += (0.5 - rx);
                yOffset += (0.5 - ry);

                xOffset *= pixelSize;
                yOffset *= pixelSize;

                var worldX = halfWidth - xOffset;
                var worldY = halfHeight - yOffset;

                var pixel = inv * new Point(worldX, worldY, -1);
                var direction = (pixel - origin).Normalize();

                rays.Add(new Ray(origin, direction));
            }
            return rays;
        }

        public Canvas Render(World world)
        {
            Stats.Reset();
            this.ProgressMonitor.OnStarted();
            Canvas image = new Canvas(this.HSize, this.VSize);

            Parallel.For(0, this.VSize, y => 
            {
                this.ProgressMonitor.OnRowStarted(y);

                for (int x = 0; x < this.HSize; x++)
                {
                    Color color = Color.Black;
                    foreach (var ray in this.RaysForPixel(x, y))
                    {
                        Interlocked.Increment(ref Stats.PrimaryRays);
                        color += world.ColorAt(ray);
                    }
                    color = color * 0.25;
                    image.SetPixel(x, y, color);
                }

                this.ProgressMonitor.OnRowFinished(y);
            });
            
            this.ProgressMonitor.OnFinished();

            return image;
        }
    }
}