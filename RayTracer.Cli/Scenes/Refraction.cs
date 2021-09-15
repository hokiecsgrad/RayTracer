using System;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Cli.Scenes
{
    class RefractionScene : IScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov)
            {
                Transform = Transformation.ViewTransform(
                                new Point(-2.6, 1.5, -3.9), // view from
                                new Point(-0.6, 1, -0.8),// view to
                                new Vector(0, 1, 0)),   // vector up

                ProgressMonitor = new ParallelConsoleProgressMonitor(height),
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(-4.9, 4.9, -1),
                new Color(1, 1, 1)
            );

            var wallMaterial = new Material()
            {
                Pattern = new Stripe(
                            new Color(0.45, 0.45, 0.45),
                            new Color(0.55, 0.55, 0.55))
                {
                    Transform = Transformation.Rotation_y(Math.PI / 2) *
                                    Transformation.Scaling(0.25, 0.25, 0.25),
                },
                Ambient = 0,
                Diffuse = 0.4,
                Specular = 0,
                Reflective = 0.3,
            };

            var floor = new Plane()
            {
                Transform = Transformation.Rotation_y(0.31415),
                Material = new Material()
                {
                    Pattern = new Checkers(new Color(0.35, 0.35, 0.35), new Color(0.65, 0.65, 0.65))
                    {
                        Transform = Transformation.Translation(0, 0.01, 0)
                    },
                    Specular = 0,
                    Reflective = 0.4,
                }
            };

            // the ceiling
            var ceiling = new Plane()
            {
                Transform = Transformation.Translation(0, 5, 0),
                Material = new Material()
                {
                    Color = new Color(0.8, 0.8, 0.8),
                    Ambient = 0.3,
                    Specular = 0
                }
            };

            // west wall
            var westWall = new Plane()
            {
                Transform = Transformation.Translation(-5, 0, 0) *
                            Transformation.Rotation_z(1.5708) *
                            Transformation.Rotation_y(1.5708),
                Material = wallMaterial,
            };

            // east wall
            var eastWall = new Plane()
            {
                Transform = Transformation.Translation(5, 0, 0) *
                            Transformation.Rotation_z(1.5708) *
                            Transformation.Rotation_y(1.5708),
                Material = wallMaterial,
            };

            // north wall
            var northWall = new Plane()
            {
                Transform = Transformation.Translation(0, 0, 5) *
                            Transformation.Rotation_x(1.5708),
                Material = wallMaterial,
            };

            // south wall
            var southWall = new Plane()
            {
                Transform = Transformation.Translation(0, 0, -5) *
                            Transformation.Rotation_x(1.5708),
                Material = wallMaterial,
            };

            // ----------------------
            // background balls
            // ----------------------

            var bgGroup1 = new Group();
            var bg1 = new Sphere()
            {
                Transform = Transformation.Translation(4.6, 0.4, 1) *
                            Transformation.Scaling(0.4, 0.4, 0.4),
                Material = new Material()
                {
                    Color = new Color(0.8, 0.5, 0.3),
                    Shininess = 50,
                },
            };
            bgGroup1.AddShape(bg1);

            var bg2 = new Sphere()
            {
                Transform = Transformation.Translation(4.7, 0.3, 0.4) *
                            Transformation.Scaling(0.3, 0.3, 0.3),
                Material = new Material()
                {
                    Color = new Color(0.9, 0.4, 0.5),
                    Shininess = 50,
                },
            };
            bgGroup1.AddShape(bg2);

            var boundingBoxMaterial = new Material()
            {
                Color = new Color(1, 1, 0),
                Ambient = 0.2,
                Diffuse = 0.0,
                Specular = 0.0,
                Shininess = 0,
                Reflective = 0.0,
                Transparency = 0.8,
                RefractiveIndex = 1,
            };
            boundingBoxMaterial = new Material()
            {
                Color = new Color(0, 0, 0),
                Ambient = 0.0,
                Diffuse = 0.0,
                Specular = 0.0,
                Shininess = 0,
                Reflective = 0.0,
                Transparency = 1,
                RefractiveIndex = 1,
            };

            var boxGroup1 = new Cube(bgGroup1.GetBounds().Min, bgGroup1.GetBounds().Max)
            {
                Material = boundingBoxMaterial,
                HitBy = RayType.Primary,
            };

            var bgGroup2 = new Group();
            var bg3 = new Sphere()
            {
                Transform = Transformation.Translation(-1, 0.5, 4.5) *
                            Transformation.Scaling(0.5, 0.5, 0.5),
                Material = new Material()
                {
                    Color = new Color(0.4, 0.9, 0.6),
                    Shininess = 50,
                },
            };
            bgGroup2.AddShape(bg3);

            var bg4 = new Sphere()
            {
                Transform = Transformation.Translation(-1.7, 0.3, 4.7) *
                            Transformation.Scaling(0.3, 0.3, 0.3),
                Material = new Material()
                {
                    Color = new Color(0.4, 0.6, 0.9),
                    Shininess = 50,
                },
            };
            bgGroup2.AddShape(bg4);
            var boxGroup2 = new Cube(bgGroup2.GetBounds().Min, bgGroup2.GetBounds().Max)
            {
                Material = boundingBoxMaterial,
                HitBy = RayType.Primary,
            };

            // ----------------------
            // foreground balls
            // ----------------------
            var fgGroup = new Group();
            // red sphere
            var redSphere = new Sphere()
            {
                Transform = Transformation.Translation(-0.6, 1, 0.6),
                Material = new Material()
                {
                    Color = new Color(1, 0.3, 0.2),
                    Specular = 0.4,
                    Shininess = 5,
                },
            };
            fgGroup.AddShape(redSphere);

            // blue glass sphere
            var blueGlassSphere = new Sphere()
            {
                Transform = Transformation.Translation(0.6, 0.7, -0.6) *
                            Transformation.Scaling(0.7, 0.7, 0.7),
                Material = new Material()
                {
                    Color = new Color(0, 0, 0.2),
                    Ambient = 0,
                    Diffuse = 0.4,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 0.9,
                    Transparency = 0.9,
                    RefractiveIndex = 1.5,
                },
            };
            fgGroup.AddShape(blueGlassSphere);

            // green glass sphere
            var greenGlassSphere = new Sphere()
            {
                Transform = Transformation.Translation(-0.7, 0.5, -0.8) *
                            Transformation.Scaling(0.5, 0.5, 0.5),
                Material = new Material()
                {
                    Color = new Color(0, 0.2, 0),
                    Ambient = 0,
                    Diffuse = 0.4,
                    Specular = 0.9,
                    Shininess = 300,
                    Reflective = 0.9,
                    Transparency = 0.9,
                    RefractiveIndex = 1.4,
                },
            };
            fgGroup.AddShape(greenGlassSphere);
            var boxFgGroup = new Cube(fgGroup.GetBounds().Min, fgGroup.GetBounds().Max)
            {
                Material = boundingBoxMaterial,
                HitBy = RayType.Primary,
            };

            World world = new World();
            world.Shapes = new List<Shape> { floor, ceiling, westWall, eastWall, northWall, southWall, bgGroup1, boxGroup1, bgGroup2, boxGroup2, fgGroup, boxFgGroup };
            world.Lights = new List<ILight> { light };

            return (world, camera);
        }
    }
}