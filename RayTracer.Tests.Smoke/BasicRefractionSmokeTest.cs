using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class BasicRefractionSmokeTests
    {
        [Fact]
        public void RenderBasicScene()
        {

            // ======================================================
            // define constants to avoid duplication
            // ======================================================

            var wallMaterial = new Material();
            wallMaterial.Pattern = new Stripe(new Color(0.45, 0.45, 0.45), new Color(0.55, 0.55, 0.55));
            wallMaterial.Pattern.Transform = Transformation.Scaling(0.25, 0.25, 0.25);
            wallMaterial.Ambient = 0;
            wallMaterial.Diffuse = 0.4;
            wallMaterial.Specular = 0;
            wallMaterial.Reflective = 0.3;

            // ======================================================
            // describe the elements of the scene
            // ======================================================

            // the checkered floor
            var floor = new Plane();
            floor.Transform = Transformation.Rotation_y(0.31415);
            floor.Material = new Material();
            floor.Material.Pattern = new Checkers(new Color(0.35, 0.35, 0.35), new Color(0.65, 0.65, 0.65));
            floor.Material.Pattern.Transform = Transformation.Translation(0, 0.01, 0);
            floor.Material.Specular = 0;
            floor.Material.Reflective = 0.4;

            // the ceiling
            var ceiling = new Plane();
            ceiling.Transform = Transformation.Translation(0, 5, 0);
            ceiling.Material = new Material();
            ceiling.Material.Color = new Color(0.8, 0.8, 0.8);
            ceiling.Material.Ambient = 0.3;
            ceiling.Material.Specular = 0;

            // west wall
            var westWall = new Plane();
            westWall.Transform = Transformation.Translation(-5, 0, 0) * Transformation.Rotation_z(1.5708) * Transformation.Rotation_y(1.5708);
            westWall.Material = wallMaterial;

            // east wall
            var eastWall = new Plane();
            eastWall.Transform = Transformation.Translation(5, 0, 0) * Transformation.Rotation_z(1.5708) * Transformation.Rotation_y(1.5708);
            eastWall.Material = wallMaterial;

            // north wall
            var northWall = new Plane();
            northWall.Transform = Transformation.Translation(0, 0, 5) * Transformation.Rotation_x(1.5708);
            northWall.Material = wallMaterial;

            // south wall
            var southWall = new Plane();
            southWall.Transform = Transformation.Translation(0, 0, -5) * Transformation.Rotation_x(1.5708);
            southWall.Material = wallMaterial;

            // ----------------------
            // background balls
            // ----------------------

            var bg1 = new Sphere();
            bg1.Transform = Transformation.Translation(4.6, 0.4, 1) * Transformation.Scaling(0.4, 0.4, 0.4);
            bg1.Material = new Material();
            bg1.Material.Color = new Color(0.8, 0.5, 0.3);
            bg1.Material.Shininess = 50;

            var bg2 = new Sphere();
            bg2.Transform = Transformation.Translation(4.7, 0.3, 0.4) * Transformation.Scaling(0.3, 0.3, 0.3);
            bg2.Material = new Material();
            bg2.Material.Color = new Color(0.9, 0.4, 0.5);
            bg2.Material.Shininess = 50;

            var bg3 = new Sphere();
            bg3.Transform = Transformation.Translation(-1, 0.5, 4.5) * Transformation.Scaling(0.5, 0.5, 0.5);
            bg3.Material = new Material();
            bg3.Material.Color = new Color(0.4, 0.9, 0.6);
            bg3.Material.Shininess = 50;

            var bg4 = new Sphere();
            bg4.Transform = Transformation.Translation(-1.7, 0.3, 4.7) * Transformation.Scaling(0.3, 0.3, 0.3);
            bg4.Material = new Material();
            bg4.Material.Color = new Color(0.4, 0.6, 0.9);
            bg4.Material.Shininess = 50;

            // ----------------------
            // foreground balls
            // ----------------------

            // red sphere
            var redSphere = new Sphere();
            redSphere.Transform = Transformation.Translation(-0.6, 1, 0.6);
            redSphere.Material = new Material();
            redSphere.Material.Color = new Color(1, 0.3, 0.2);
            redSphere.Material.Specular = 0.4;
            redSphere.Material.Shininess = 5;

            // blue glass sphere
            var blueGlassSphere = new Sphere();
            blueGlassSphere.Transform = Transformation.Translation(0.6, 0.7, -0.6) * Transformation.Scaling(0.7, 0.7, 0.7);
            blueGlassSphere.Material = new Material();
            blueGlassSphere.Material.Color = new Color(0, 0, 0.2);
            blueGlassSphere.Material.Ambient = 0;
            blueGlassSphere.Material.Diffuse = 0.4;
            blueGlassSphere.Material.Specular = 0.9;
            blueGlassSphere.Material.Shininess = 300;
            blueGlassSphere.Material.Reflective = 0.9;
            blueGlassSphere.Material.Transparency = 0.9;
            blueGlassSphere.Material.RefractiveIndex = 1.5;

            // green glass sphere
            var greenGlassSphere = new Sphere();
            greenGlassSphere.Transform = Transformation.Translation(-0.7, 0.5, -0.8) * Transformation.Scaling(0.5, 0.5, 0.5);
            greenGlassSphere.Material = new Material();
            greenGlassSphere.Material.Color = new Color(0, 0.2, 0);
            greenGlassSphere.Material.Ambient = 0;
            greenGlassSphere.Material.Diffuse = 0.4;
            greenGlassSphere.Material.Specular = 0.9;
            greenGlassSphere.Material.Shininess = 300;
            greenGlassSphere.Material.Reflective = 0.9;
            greenGlassSphere.Material.Transparency = 0.9;
            greenGlassSphere.Material.RefractiveIndex = 1.5;

            World world = new World();
            world.Shapes = new List<Shape> {floor, ceiling, westWall, eastWall, northWall, southWall, bg1, bg2, bg3, bg4, redSphere, blueGlassSphere, greenGlassSphere};

            // ======================================================
            // light sources
            // ======================================================

            world.Light = new PointLight(new Point(-4.9, 4.9, -1), new Color(1, 1, 1));

            // ======================================================
            // the camera
            // ======================================================

            Camera camera = new Camera(400, 300, 1.152);
            camera.Transform = Transformation.ViewTransform(
                                new Point(-2.6, 1.5, -3.9), // view from
                                new Point(-0.6, 1, -0.8),// view to
                                new Vector(0, 1, 0));    // vector up

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/BasicRefraction.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}