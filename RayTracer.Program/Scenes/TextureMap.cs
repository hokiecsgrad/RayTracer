using System;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program
{
    class TextureMapScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov) 
            {
                Transform = Transformation.ViewTransform(
                                new Point(-10, 0, 0), // view from
                                new Point(0, 0, 0),// view to
                                new Vector(0, 1, 0)),   // vector up
                
                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(-10, 10, -10),
                new Color(1, 1, 1)
            );

            var sphere = new Sphere()
            {
                Transform = Transformation.Translation(0, 0, -2),
                Material = new Material()
                {
                    Pattern = new TextureMap(
                        new UvCheckers(20, 10, new Color(0, 0.5, 0), new Color(1, 1, 1)),
                        TextureMapper.SphericalMap
                    ),
                    Ambient = 0.1,
                    Specular = 0.4,
                    Shininess = 10,
                    Diffuse = 0.6,
                },
            };

            var floor = new Plane()
            {
                Material = new Material()
                {
                    Pattern = new TextureMap(
                        new UvCheckers(2, 2, new Color(0, 0.5, 0), new Color(1, 1, 1)),
                        TextureMapper.PlanarMap
                    ),
                    Ambient = 0.1,
                    Specular = 0.0,
                    Shininess = 10,
                    Diffuse = 0.9,
                }
            };

            var cyl = new Cylinder()
            {
                Minimum = 0,
                Maximum = 1,
                Transform = 
                    Transformation.Translation(0, -0.5, 0) * 
                    Transformation.Scaling(1, 3.1415, 1),
                Material = new Material()
                {
                    Pattern = new TextureMap(
                        new UvCheckers(16, 8, new Color(0, 0.5, 0), new Color(1, 1, 1)),
                        TextureMapper.CylindricalMap
                    ),
                    Ambient = 0.1,
                    Specular = 0.6,
                    Shininess = 15,
                    Diffuse = 0.8,
                },
            };

            World world = new World();
            world.Shapes = new List<Shape> {cyl, sphere};
            world.Lights = new List<ILight> {light};

            return (world, camera);
        }
    }
}