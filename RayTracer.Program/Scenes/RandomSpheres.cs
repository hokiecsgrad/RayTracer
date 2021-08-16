using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer.Program.Scenes
{
    public class RandomSpheresScene : IScene
    {
        Random rng = new Random();

        public (World, Camera) Setup(int width, int height, double fov)
        {
            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                //new Point(-8.75, 10, -8.75), // view from
                                new Point(15, 5, 15), // view from
                                new Point(0, 2, 0), // view to
                                new Vector(0, 1, 0)),  // vector up

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            var floor = new Plane()
            {
                Material = new Material()
                {
                    Color = new Color(0.3, 0.3, 0.3),
                    Ambient = 0.5,
                    Diffuse = 0.9,
                    Specular = 0.9,
                    Shininess = 200,
                },
            };

            Sphere[] spheres = new Sphere[20];
            Point[] origins = new Point[20];
            for (int i = 0; i < spheres.Length; i++)
            {
                double scalingFactor = 0.0;
                double x = 0.0;
                double y = 0.0;
                double z = 0.0;

                do
                {
                    scalingFactor = rng.Next(10, 40) / 10.0;
                    x = rng.Next(-15, 15);
                    y = scalingFactor;
                    z = rng.Next(-15, 15);

                } while (IsColliding(origins, x, y, z, scalingFactor));

                origins[i] = new Point(x, y, z);

                spheres[i] = new Sphere()
                {
                    Transform =
                        Transformation.Translation(x, y, z) *
                        Transformation.Scaling(scalingFactor, scalingFactor, scalingFactor),

                    Material = new Material()
                    {
                        Color = new Color(rng.Next(10) / 10.0, rng.Next(10) / 10.0, rng.Next(10) / 10.0),
                        Ambient = rng.Next(10) / 10.0,
                        Diffuse = rng.Next(10) / 10.0,
                        Specular = rng.Next(10) / 10.0,
                        Shininess = rng.Next(50, 300),
                        Reflective = rng.Next(10) / 10.0,
                        Transparency = rng.Next(10) / 10.0,
                        RefractiveIndex = rng.Next(10, 30) / 10.0,
                    },
                };
            }

            World world = new World();
            world.Shapes = new List<Shape> { floor };
            world.Shapes.AddRange(spheres);
            world.Lights = new List<ILight> { new PointLight(new Point(0, 10, 0), new Color(0.7, 0.7, 0.7)) };

            return (world, camera);
        }

        private bool IsColliding(Point[] origins, double x, double y, double z, double scale)
        {
            for (int i = 0; i < origins.Length; i++)
                if (
                    origins[i] is not null &&
                    DistanceBetween(new Point(x, y, z), origins[i]) < (scale + origins[i].y + 1)
                    )
                    return true;

            return false;
        }

        private double DistanceBetween(Point a, Point b)
            => Math.Sqrt(Math.Pow(a.x - b.x, 2) +
                            Math.Pow(a.y - b.y, 2) +
                            Math.Pow(a.z - b.z, 2));
    }
}