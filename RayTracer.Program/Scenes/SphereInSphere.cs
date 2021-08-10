using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer.Program.Scenes
{
    public class SphereInSphereScene : IScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                new Point(0, 2.5, 0),
                                new Point(0, 0, 0),
                                new Vector(-1, 0, 0)),

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            var floor = new Plane()
            {
                Transform = Transformation.Translation(0, -10, 0),
                Material = new Material()
                {
                    Pattern = new Checkers(Color.Black, Color.White),
                    Ambient = 0.1,
                    Diffuse = 0.9,
                    Specular = 0.9,
                    Shininess = 200,
                },
            };

            var innerSphere = new Sphere()
            {
                Transform = Transformation.Scaling(0.5, 0.5, 0.5),
                Material = new Material()
                {
                    Ambient = 0.0,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 1,
                    RefractiveIndex = 1.00029,
                },
                HitBy = RayType.NoShadows,
            };

            var outerSphere = new Sphere()
            {
                Material = new Material()
                {
                    Ambient = 0.0,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 1,
                    RefractiveIndex = 1.52,
                },
                HitBy = RayType.NoShadows,
            };

            World world = new World();
            world.Shapes = new List<Shape> { floor, innerSphere, outerSphere };
            world.Lights = new List<ILight> { new PointLight(new Point(20, 10, 0), new Color(0.7, 0.7, 0.7)) };

            return (world, camera);
        }
    }
}