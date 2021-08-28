using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RayTracer.Program.Scenes
{
    public class ClearCubeScene : IScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                //new Point(-8.75, 10, -8.75), // view from
                                new Point(-8.75, 1.75, -8.75), // view from
                                new Point(0, 1.75, 0), // view to
                                new Vector(0, 1, 0)),  // vector up

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

            var cube1 = new Cube()
            {
                Transform = Transformation.Translation(-4, 1.1, -4) * Transformation.Scaling(0.5, 2, 0.5),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.9,
                    RefractiveIndex = 1.52,
                    Color = new Color(0, 0, 0),
                },
                HitBy = RayType.NoShadows,
            };

            var cube2 = new Cube()
            {
                Transform = Transformation.Translation(-4, 1.1, -2.75) * Transformation.Scaling(0.5, 2, 0.5),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.9,
                    RefractiveIndex = 1.52,
                    Color = new Color(0, 0, 0),
                },
                HitBy = RayType.NoShadows,
            };

            var cube3 = new Cube()
            {
                Transform = Transformation.Translation(-2.75, 1.1, -3.9) * Transformation.Scaling(0.5, 2, 0.5),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.9,
                    RefractiveIndex = 1.52,
                    Color = new Color(0, 0, 0),
                },
                HitBy = RayType.NoShadows,
            };

            var cube4 = new Cube()
            {
                Transform = Transformation.Translation(-2.75, 1.1, -2.75) * Transformation.Scaling(0.5, 2, 0.5),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.9,
                    RefractiveIndex = 1.52,
                    Color = new Color(0, 0, 0),
                },
                HitBy = RayType.NoShadows,
            };

            var sphere = new Sphere()
            {
                Transform = Transformation.Translation(3, 1.5, -3) * Transformation.Scaling(1.25, 1.25, 1.25),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.9,
                    RefractiveIndex = 1.52,
                    Color = new Color(0, 0, 0),
                },
                HitBy = RayType.NoShadows,
            };

            var sphere2 = new Sphere()
            {
                Transform = Transformation.Translation(0, 1.5, -3) * Transformation.Scaling(1.25, 1.25, 1.25),
                Material = new Material()
                {
                    Ambient = 0.03,
                    Diffuse = 0.1,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 1,
                    Transparency = 0.9,
                    RefractiveIndex = 1.52,
                    Color = new Color(0, 0, 0),
                },
                HitBy = RayType.NoShadows,
            };

            World world = new World();
            world.Shapes = new List<Shape> { floor, eastWall, northWall, cube1, cube2, cube3, cube4, sphere, sphere2 };
            world.Lights = new List<ILight> { new PointLight(new Point(-3, 4, 3), new Color(0.7, 0.7, 0.7)) };

            return (world, camera);
        }
    }
}