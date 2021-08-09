using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer.Program
{
    public class ClearCubeScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                new Point(-5, 1.6, -5), // view from
                                new Point(0, 1.6, 0),// view to
                                new Vector(0, 1, 0)),   // vector up

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            var floor = new Plane()
            {
                Material = new Material()
                {
                    Pattern = new Checkers(Color.White, Color.Black),
                    Ambient = 0.1,
                    Diffuse = 0.9,
                    Specular = 0.9,
                    Shininess = 200,
                },
            };

            var westWall = new Plane()
            {
                Transform = Transformation.Translation(-5, 0, 0) *
                            Transformation.Rotation_z(1.5708) *
                            Transformation.Rotation_y(1.5708),
                Material = new Material()
                {
                    Pattern = new Checkers(Color.White, Color.Black),
                    Ambient = 0.1,
                    Diffuse = 0.9,
                    Specular = 0.9,
                    Shininess = 200,
                },
            };

            var eastWall = new Plane()
            {
                Transform = Transformation.Translation(5, 0, 0) *
                            Transformation.Rotation_z(1.5708) *
                            Transformation.Rotation_y(-1.5708),

                Material = new Material()
                {
                    Pattern = new Checkers(Color.White, Color.Black),
                    Ambient = 0.1,
                    Diffuse = 0.9,
                    Specular = 0.9,
                    Shininess = 200,
                },
            };

            var northWall = new Plane()
            {
                Transform = Transformation.Translation(0, 0, 5) *
                            Transformation.Rotation_x(1.5708),

                Material = new Material()
                {
                    Pattern = new Checkers(Color.White, Color.Black),
                    Ambient = 0.1,
                    Diffuse = 0.9,
                    Specular = 0.9,
                    Shininess = 200,
                },
            };

            var cube = new Cube()
            {
                Transform = Transformation.Translation(0, 1.1, 0),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.75,
                    RefractiveIndex = 1.2,
                    Color = new Color(1, 0, 0),
                },
                HitBy = RayType.NoShadows,
            };

            var sphere = new Sphere()
            {
                Transform = Transformation.Translation(2, 1.1, 0) * Transformation.Scaling(0.8, 0.8, 0.8),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.75,
                    RefractiveIndex = 1.2,
                    Color = new Color(0, 0, 1),
                },
                HitBy = RayType.NoShadows,
            };

            World world = new World();
            world.Shapes = new List<Shape> { floor, cube, westWall, eastWall, northWall, sphere };
            world.Lights = new List<ILight> { new PointLight(new Point(-3, 4, 3), new Color(0.7, 0.7, 0.7)) };

            return (world, camera);
        }
    }
}