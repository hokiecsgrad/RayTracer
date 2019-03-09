using System;

namespace RayTracer
{
    public abstract class ISampler 
    {
        protected Camera Camera;

        public ISampler(Camera camera)
        {
            this.Camera = camera;
        }

        public abstract Color Sample(int x, int y, World world);
    }
}