using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class ConeSmokeTests
    {
        [Fact]
        public void RenderBasicScene()
        {
            var floor = new Plane();
            floor.Material = new Material();
            floor.Material.Pattern = new Checkers(new Color(0.5, 0.5, 0.5), new Color(0.75, 0.75, 0.75));
            floor.Material.Pattern.Transform = Transformation.Rotation_y(0.3) * Transformation.Scaling(0.25, 0.25, 0.25);
            floor.Material.Ambient = 0.2;
            floor.Material.Diffuse = 0.9;
            floor.Material.Specular = 0;

            var cone = new Cone();
            cone.Minimum = 0;
            cone.Maximum = 1;
            cone.IsClosed = true;
            cone.Transform = Transformation.Translation(-1, 0, 1) * Transformation.Scaling(0.5, 1, 0.5);
            cone.Material = new Material();
            cone.Material.Color = new Color(0, 0, 0.6);
            cone.Material.Diffuse = 0.1;
            cone.Material.Specular = 0.9;
            cone.Material.Shininess = 300;
            cone.Material.Reflective = 0.9;

            var cone2 = new Cone();
            cone2.Minimum = -1;
            cone2.Maximum = 1;
            cone2.IsClosed = true;
            cone2.Transform = Transformation.Translation(0,0,-0.75) * Transformation.Scaling(0.05, 1, 0.05);
            cone2.Material = new Material();
            cone2.Material.Color = new Color(1, 0, 0);
            cone2.Material.Ambient = 0.1;
            cone2.Material.Diffuse = 0.9;
            cone2.Material.Specular = 0.9;
            cone2.Material.Shininess = 300;

            var cone3 = new Cone();
            cone3.Minimum = -1;
            cone3.Maximum = 0;
            cone3.IsClosed = true;
            cone3.Transform = Transformation.Translation(0,0,-2.25) * Transformation.Rotation_y(-0.15) * Transformation.Translation(0, 0, 1.5) * Transformation.Scaling(0.05, 1, 0.05);
            cone3.Material = new Material();
            cone3.Material.Color = new Color(1, 1, 0);
            cone3.Material.Ambient = 0.1;
            cone3.Material.Diffuse = 0.9;
            cone3.Material.Specular = 0.9;
            cone3.Material.Shininess = 300;

            var cone4 = new Cone();
            cone4.Minimum = 0;
            cone4.Maximum = 2;
            cone4.IsClosed = false;
            cone4.Transform = Transformation.Translation(0,0,-2.25) * Transformation.Rotation_y(-0.3) * Transformation.Translation(0, 0, 1.5) * Transformation.Scaling(0.05, 1, 0.05);
            cone4.Material = new Material();
            cone4.Material.Color = new Color(0, 1, 0);
            cone4.Material.Ambient = 0.1;
            cone4.Material.Diffuse = 0.9;
            cone4.Material.Specular = 0.9;
            cone4.Material.Shininess = 300;

            World world = new World();
            world.Shapes = new List<Shape> {floor, cone, cone2, cone3, cone4};
  
            // ======================================================
            // light sources
            // ======================================================

            world.Light = new PointLight(new Point(1, 6.9, -4.9), new Color(1, 1, 1));

            // ======================================================
            // the camera
            // ======================================================

            Camera camera = new Camera(400, 200, 0.314);
            camera.Transform = Transformation.ViewTransform(
                                new Point(8, 3.5, -9), // view from
                                new Point(0, 0.3, 0),// view to
                                new Vector(0, 1, 0));    // vector up

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/Cones.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}