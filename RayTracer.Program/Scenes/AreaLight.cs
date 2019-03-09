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
                                new Point(-3, 1, 2.5), // view from
                                new Point(0, 0.5, 0),// view to
                                new Vector(0, 1, 0)),   // vector up
                
                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new AreaLight(
                new Point(-1, 2, 4),
                new Vector(2, 0, 0),
                10,
                new Vector(0, 2, 0),
                10,
                new Color(1.5, 1.5, 1.5)
            );

            var light2 = new PointLight(new Point(-1, 2, 4), Color.White);

            var cube = new Cube()
            {
                Material = new Material()
                {
                    Color = new Color(1.5, 1.5, 1.5),
                    Ambient = 1.0,
                    Diffuse = 0.0,
                    Specular = 0.0,
                    Transparency = 1.0,
                    RefractiveIndex = 1.0,
                },
                Transform = 
                    Transformation.Translation(0, 3, 4) *
                    Transformation.Scaling(1, 1, 0.01),
                CastsShadow = false,
            };

            var plane = new Plane()
            {
                Material = new Material()
                {
                    Color = Color.White,
                    Ambient = 0.025,
                    Diffuse = 0.67,
                    Specular = 0,
                },
            };

            var sphere = new Sphere()
            {
                Transform = 
                    Transformation.Translation(0.5, 0.5, 0) *
                    Transformation.Scaling(0.5, 0.5, 0.5),
                Material = new Material()
                {
                    Color = new Color(1, 0, 0),
                    Ambient = 0.1,
                    Specular = 0.0,
                    Diffuse = 0.6,
                    Reflective = 0.3,
                },
            };

            var sphere2 = new Sphere()
            {
                Transform =
                    Transformation.Translation(-0.25, 0.33, 0) *
                    Transformation.Scaling(0.33, 0.33, 0.33),
                Material = new Material()
                {
                    Color = new Color(0.5, 0.5, 1),
                    Ambient = 0.1,
                    Specular = 0.0,
                    Diffuse = 0.6,
                    Reflective = 0.3,
                },
            };

            World world = new World();
            world.Shapes = new List<Shape> {cube, plane, sphere, sphere2};
            world.Lights = new List<ILight> {light};

            return (world, camera);
        }
    }
}
