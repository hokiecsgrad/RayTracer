using System;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program
{
    class BasicCoordTestScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov) 
            {
                Transform = Transformation.ViewTransform(
                                new Point(0, 0, 10), // view from
                                new Point(0, 0, 0),// view to
                                new Vector(0, 1, 0)),   // vector up
                
                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(5, 5, -5),
                new Color(1, 1, 1)
            );

            var sphere = new Sphere()
            {
                Transform = Transformation.Scaling(0.25, 0.25, 0.25),
                Material = new Material()
                {
                    Color = Color.White,
                    Ambient = 0.1,
                    Specular = 0.4,
                    Shininess = 10,
                    Diffuse = 0.6,
                },
            };

            var right = new Sphere()
            {
                Transform = Transformation.Translation(3, 0, 0),
                Material = new Material()
                {
                    Color = new Color(1, 0, 0),
                    Ambient = 0.1,
                    Specular = 0.4,
                    Shininess = 10,
                    Diffuse = 0.6,
                },
            };

            var up = new Sphere()
            {
                Transform = Transformation.Translation(0, 3, 0),
                Material = new Material()
                {
                    Color = new Color(0, 1, 0),
                    Ambient = 0.1,
                    Specular = 0.4,
                    Shininess = 10,
                    Diffuse = 0.6,
                },
            };

            var behind = new Sphere()
            {
                Transform = Transformation.Translation(0, 0, -3),
                Material = new Material()
                {
                    Color = new Color(0, 0, 1),
                    Ambient = 0.1,
                    Specular = 0.4,
                    Shininess = 10,
                    Diffuse = 0.6,
                },
            };

            World world = new World();
            world.Shapes = new List<Shape> {sphere, up, right, behind};
            world.Lights = new List<ILight> {light};

            return (world, camera);
        }
    }
}