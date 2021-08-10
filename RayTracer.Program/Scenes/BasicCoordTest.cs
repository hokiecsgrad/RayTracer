using System;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program.Scenes
{
    class BasicCoordTestScene : IScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                new Point(0, 0, -20), // view from
                                new Point(0, 0, 0),// view to
                                new Vector(0, 1, 0)),   // vector up

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(0, 100, -100),
                new Color(0.25, 0.25, 0.25)
            );

            var light2 = new PointLight(
                new Point(0, -100, -100),
                new Color(0.25, 0.25, 0.25)
            );

            var light3 = new PointLight(
                new Point(-100, 0, -100),
                new Color(0.25, 0.25, 0.25)
            );

            var light4 = new PointLight(
                new Point(100, 0, -100),
                new Color(0.25, 0.25, 0.25)
            );

            var yellow = new Color(1, 1, 0);
            var cyan = new Color(0, 1, 1);
            var red = new Color(1, 0, 0);
            var blue = new Color(0, 0, 1);
            var brown = new Color(1, 0.5, 0);
            var green = new Color(0, 1, 0);
            var purple = new Color(1, 0, 1);
            var white = new Color(1, 1, 1);

            var left = new UvAlignCheck(yellow, cyan, red, blue, brown);
            var front = new UvAlignCheck(cyan, red, yellow, brown, green);
            var right = new UvAlignCheck(red, yellow, purple, green, white);
            var back = new UvAlignCheck(green, purple, cyan, white, blue);
            var up = new UvAlignCheck(brown, cyan, purple, red, yellow);
            var down = new UvAlignCheck(purple, brown, green, blue, white);

            var mapCubeMaterial = new Material()
            {
                Pattern = new CubeMap(left, front, right, back, up, down),
                Ambient = 0.2,
                Specular = 0.0,
                Diffuse = 0.8,
            };

            var mappedCube1 = new Cube()
            {
                Transform = Transformation.Translation(-6, 2, 0) *
                    Transformation.Rotation_x(0.7854) *
                    Transformation.Rotation_y(0.7854),
                Material = mapCubeMaterial,
            };

            var mappedCube2 = new Cube()
            {
                Transform = Transformation.Translation(-2, 2, 0) *
                    Transformation.Rotation_x(0.7854) *
                    Transformation.Rotation_y(2.3562),
                Material = mapCubeMaterial,
            };

            var mappedCube3 = new Cube()
            {
                Transform = Transformation.Translation(2, 2, 0) *
                    Transformation.Rotation_x(0.7854) *
                    Transformation.Rotation_y(3.927),
                Material = mapCubeMaterial,
            };

            var mappedCube4 = new Cube()
            {
                Transform = Transformation.Translation(6, 2, 0) *
                    Transformation.Rotation_x(0.7854) *
                    Transformation.Rotation_y(5.4978),
                Material = mapCubeMaterial,
            };

            var mappedCube5 = new Cube()
            {
                Transform = Transformation.Translation(-6, -2, 0) *
                    Transformation.Rotation_x(-0.7854) *
                    Transformation.Rotation_y(0.7854),
                Material = mapCubeMaterial,
            };

            var mappedCube6 = new Cube()
            {
                Transform = Transformation.Translation(-2, -2, 0) *
                    Transformation.Rotation_x(-0.7854) *
                    Transformation.Rotation_y(2.3562),
                Material = mapCubeMaterial,
            };

            var mappedCube7 = new Cube()
            {
                Transform = Transformation.Translation(2, -2, 0) *
                    Transformation.Rotation_x(-0.7854) *
                    Transformation.Rotation_y(3.927),
                Material = mapCubeMaterial,
            };

            var mappedCube8 = new Cube()
            {
                Transform = Transformation.Translation(6, -2, 0) *
                    Transformation.Rotation_x(-0.7854) *
                    Transformation.Rotation_y(5.4978),
                Material = mapCubeMaterial,
            };

            World world = new World();
            world.Shapes = new List<Shape> { mappedCube1, mappedCube2, mappedCube3, mappedCube4, mappedCube5, mappedCube6, mappedCube7, mappedCube8 };
            world.Lights = new List<ILight> { light, light2, light3, light4 };

            return (world, camera);
        }
    }
}