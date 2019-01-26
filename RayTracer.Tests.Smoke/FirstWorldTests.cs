using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class FirstWorldTests
    {
        [Fact]
        public void RenderBasicScene()
        {
            Sphere floor = new Sphere();
            floor.Transform = Transformation.Scaling(10, 0.01, 10);
            floor.Material = new Material();
            floor.Material.Color = new Color(1, 0.9, 0.9);
            floor.Material.Specular = 0;

            Sphere left_wall = new Sphere();
            left_wall.Transform = Transformation.Translation(0, 0, 5) *
                                    Transformation.Rotation_y(-Math.PI/4) * 
                                    Transformation.Rotation_x(Math.PI/2) *
                                    Transformation.Scaling(10, 0.01, 10);
            left_wall.Material = floor.Material;

            Sphere right_wall = new Sphere();
            right_wall.Transform = Transformation.Translation(0, 0, 5) *
                                    Transformation.Rotation_y(Math.PI/4) * 
                                    Transformation.Rotation_x(Math.PI/2) *
                                    Transformation.Scaling(10, 0.01, 10);
            right_wall.Material = floor.Material;

            Sphere middle = new Sphere();
            middle.Transform = Transformation.Translation(-0.5, 1, 0.5);
            middle.Material = new Material();
            middle.Material.Color = new Color(0.1, 1, 0.5);
            middle.Material.Diffuse = 0.7;
            middle.Material.Specular = 0.3;

            Sphere right = new Sphere();
            right.Transform = Transformation.Translation(1.5, 0.5, -0.5) * Transformation.Scaling(0.5, 0.5, 0.5);
            right.Material = new Material();
            right.Material.Color = new Color(0.5, 1, 0.1);
            right.Material.Diffuse = 0.7;
            right.Material.Specular = 0.3;

            Sphere left = new Sphere();
            left.Transform = Transformation.Translation(-1.5, 0.33, -0.75) * Transformation.Scaling(0.33, 0.33, 0.33);
            left.Material = new Material();
            left.Material.Color = new Color(1, 0.8, 0.1);
            left.Material.Diffuse = 0.7;
            left.Material.Specular = 0.3;

            World world = new World();
            world.Shapes = new List<Sphere> {left, right, middle, floor, left_wall, right_wall};
            world.Light = new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1));

            Camera camera = new Camera(200, 120, Math.PI/3);
            camera.Transform = Transformation.ViewTransform(new Point(0, 1.5, -5),
                                new Point(0, 1, 0),
                                new Vector(0, 1, 0));

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/FirstBasicSceneSmokeTest.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}