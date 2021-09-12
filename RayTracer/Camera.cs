using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracer
{
    public class Camera
    {
        public IProgressMonitor ProgressMonitor { get; set; } =
            new ProgressMonitor();


        public int HSize { get; set; }

        public int VSize { get; set; }

        public double FieldOfView { get; set; }

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


        public Camera()
        {
            HSize = 400;
            VSize = 300;
            FieldOfView = 1.125;
            Transform = Matrix.Identity;
            CalculatePixelSize();
        }

        public Camera(int hsize, int vsize, double fov)
        {
            HSize = hsize;
            VSize = vsize;
            FieldOfView = fov;
            Transform = Matrix.Identity;
            CalculatePixelSize();
        }

        public void CalculatePixelSize()
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


        public Canvas Render(World world) =>
            Render(world, new DefaultSampler(this));

        public Canvas Render(World world, ISampler sampler)
        {
            Stats.Reset();
            this.ProgressMonitor.OnStarted();
            Canvas image = new Canvas(this.HSize, this.VSize);

            if (ShouldRunMultiThreaded())
            {
                Parallel.For(0, this.VSize, y =>
                {
                    this.ProgressMonitor.OnRowStarted(y);

                    for (int x = 0; x < this.HSize; x++)
                    {
                        var color = sampler.Sample(x, y, world);
                        image.SetPixel(x, y, color);
                    }

                    this.ProgressMonitor.OnRowFinished(y);
                });
            }
            else
            {
                for (int y = 0; y < this.VSize; y++)
                {
                    this.ProgressMonitor.OnRowStarted(y);
                    for (int x = 0; x < this.HSize; x++)
                    {
                        var color = sampler.Sample(x, y, world);
                        image.SetPixel(x, y, color);
                    }
                    this.ProgressMonitor.OnRowFinished(y);
                }
            }

            this.ProgressMonitor.OnFinished();

            return image;
        }

        private bool ShouldRunMultiThreaded()
            => true;
    }
}