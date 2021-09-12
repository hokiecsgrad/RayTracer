using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer.Cli.Scenes
{
    public class SphereScene : IScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                new Point(-5.0, 1.5, 0.0), // view from
                                new Point(0.0, 0.0, 0.0), // view to
                                new Vector(0, 1, 0)),  // vector up

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            Sphere sphere = new Sphere()
            {
                Transform = Transformation.Scaling(1.0, 1.0, 1.0) *
                            Transformation.Translation(0.0, 0.0, 0.0),

                Material = new Material()
                {
                    Color = new Color(0.8, 0.5, 0.3),
                    Ambient = 0.2,
                    Diffuse = 0.4,
                    Specular = 0.9,
                    Shininess = 50,
                },
            };

            ILight light = new PointLight()
            {
                Position = new Point(-1.0, 5.0, 0.0),
                Color = new Color(1, 1, 1),
            };

            World world = new World();
            world.Shapes = new List<Shape> { sphere };
            world.Lights = new List<ILight> { light };

            return (world, camera);
        }
    }
}