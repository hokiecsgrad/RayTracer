using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer.Cli.Scenes
{
    public class ThreeSpheresScene : IScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                new Point(4.5, 1, 0.5), // view from
                                new Point(0, 1, 0.5), // view to
                                new Vector(0, 1, 0)),  // vector up

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            var floor = new Plane()
            {
                Material = new Material()
                {
                    Pattern = new Checkers(new Color(0.7, 0.7, 0.7), Color.White),
                    Ambient = 0.1,
                    Diffuse = 0.9,
                    Specular = 0.9,
                    Shininess = 200,
                },
            };

            var cylinder = new Cylinder()
            {
                Transform = Transformation.Scaling(0.1, 5, 0.1),

                Material = new Material()
                {
                    Color = new Color(1, 0, 0),
                    Ambient = 0.4,
                    Specular = 0.4,
                    Shininess = 5,
                }
            };

            var redSphere = new Sphere()
            {
                Transform = Transformation.Translation(0, 1, 1.5),

                Material = new Material()
                {
                    Color = new Color(1, 0.3, 0.2),
                    Ambient = 0.4,
                    Specular = 0.4,
                    Shininess = 5,
                },
            };

            var blueGlassSphere = new Sphere()
            {
                Transform = Transformation.Translation(2, 0.7, 0.5) *
                            Transformation.Scaling(0.7, 0.7, 0.7),

                Material = new Material()
                {
                    Color = new Color(0, 0, 0.2),
                    Ambient = 0.8,
                    Specular = 0.4,
                    Shininess = 5,
                },
            };

            var greenGlassSphere = new Sphere()
            {
                Transform = Transformation.Translation(3, 0.5, 0) *
                            Transformation.Scaling(0.5, 0.5, 0.5),

                Material = new Material()
                {
                    Color = new Color(0, 0.2, 0),
                    Ambient = 0.8,
                    Specular = 0.4,
                    Shininess = 5,
                },
            };

            World world = new World();
            world.Shapes = new List<Shape> { floor, redSphere, blueGlassSphere, greenGlassSphere };
            world.Lights = new List<ILight> { new PointLight(new Point(2, 4, 1), new Color(1, 1, 1)) };

            return (world, camera);
        }
    }
}