using System;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program
{
    class AreaLightScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov) 
            {
                Transform = Transformation.ViewTransform(
                                new Point(3, 1.5, 0), // view from
                                new Point(0, 0, 0),// view to
                                new Vector(0, 1, 0)),   // vector up
                
                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new AreaLight(
                new Point(5, 5, -5),
                new Vector(4, 0, 0),
                8,
                new Vector(0, 4, 0),
                8,
                new Color(1, 1, 1)
            );

            var floor = new Plane()
            {
                Transform = Transformation.Translation(0, -1, 0),
                Material = new Material()
                {
                    Color = Color.White,
                },
            };

            var sphere = new Sphere()
            {
                Material = new Material()
                {
                    Color = new Color(1, 0, 0),
                }
            };
            

            World world = new World();
            world.Shapes = new List<Shape> {floor, sphere};
            world.Lights = new List<ILight> {light};

            return (world, camera);
        }
    }
}
