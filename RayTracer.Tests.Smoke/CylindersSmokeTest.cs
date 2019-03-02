using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class CylindersSmokeTests
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

            var cyl = new Cylinder();
            cyl.Minimum = 0;
            cyl.Maximum = 0.75;
            cyl.IsClosed = true;
            cyl.Transform = Transformation.Translation(-1, 0, 1) * Transformation.Scaling(0.5, 1, 0.5);
            cyl.Material = new Material();
            cyl.Material.Color = new Color(0, 0, 0.6);
            cyl.Material.Diffuse = 0.1;
            cyl.Material.Specular = 0.9;
            cyl.Material.Shininess = 300;
            cyl.Material.Reflective = 0.9;

            var decCyl1 = new Cylinder();
            decCyl1.Minimum = 0;
            decCyl1.Maximum = 0.3;
            decCyl1.IsClosed = true;
            decCyl1.Transform = Transformation.Translation(0,0,-0.75) * Transformation.Scaling(0.05, 1, 0.05);
            decCyl1.Material = new Material();
            decCyl1.Material.Color = new Color(1, 0, 0);
            decCyl1.Material.Ambient = 0.1;
            decCyl1.Material.Diffuse = 0.9;
            decCyl1.Material.Specular = 0.9;
            decCyl1.Material.Shininess = 300;

            var decCyl2 = new Cylinder();
            decCyl2.Minimum = 0;
            decCyl2.Maximum = 0.3;
            decCyl2.IsClosed = true;
            decCyl2.Transform = Transformation.Translation(0,0,-2.25) * Transformation.Rotation_y(-0.15) * Transformation.Translation(0, 0, 1.5) * Transformation.Scaling(0.05, 1, 0.05);
            decCyl2.Material = new Material();
            decCyl2.Material.Color = new Color(1, 1, 0);
            decCyl2.Material.Ambient = 0.1;
            decCyl2.Material.Diffuse = 0.9;
            decCyl2.Material.Specular = 0.9;
            decCyl2.Material.Shininess = 300;

            var decCyl3 = new Cylinder();
            decCyl3.Minimum = 0;
            decCyl3.Maximum = 0.3;
            decCyl3.IsClosed = true;
            decCyl3.Transform = Transformation.Translation(0,0,-2.25) * Transformation.Rotation_y(-0.3) * Transformation.Translation(0, 0, 1.5) * Transformation.Scaling(0.05, 1, 0.05);
            decCyl3.Material = new Material();
            decCyl3.Material.Color = new Color(0, 1, 0);
            decCyl3.Material.Ambient = 0.1;
            decCyl3.Material.Diffuse = 0.9;
            decCyl3.Material.Specular = 0.9;
            decCyl3.Material.Shininess = 300;

            var decCyl4 = new Cylinder();
            decCyl4.Minimum = 0;
            decCyl4.Maximum = 0.3;
            decCyl4.IsClosed = true;
            decCyl4.Transform = Transformation.Translation(0,0,-2.25) * Transformation.Rotation_y(-0.45) * Transformation.Translation(0, 0, 1.5) * Transformation.Scaling(0.05, 1, 0.05);
            decCyl4.Material = new Material();
            decCyl4.Material.Color = new Color(0, 1, 1);
            decCyl4.Material.Ambient = 0.1;
            decCyl4.Material.Diffuse = 0.9;
            decCyl4.Material.Specular = 0.9;
            decCyl4.Material.Shininess = 300;

            var glassCyl = new Cylinder();
            glassCyl.Minimum = 0.0001;
            glassCyl.Maximum = 0.5;
            glassCyl.IsClosed = true;
            glassCyl.Transform = Transformation.Translation(0, 0, -1.5) * Transformation.Scaling(0.33, 1, 0.33);
            glassCyl.Material = new Material();
            glassCyl.Material.Color = new Color(0.25, 0, 0);
            glassCyl.Material.Diffuse = 0.1;
            glassCyl.Material.Specular = 0.9;
            glassCyl.Material.Shininess = 300;
            glassCyl.Material.Reflective = 0.9;
            glassCyl.Material.Transparency = 0.9;
            glassCyl.Material.RefractiveIndex = 1.5;

            var conCentCyl1 = new Cylinder();
            conCentCyl1.Minimum = 0;
            conCentCyl1.Maximum = 0.2;
            conCentCyl1.IsClosed = false;
            conCentCyl1.Transform = Transformation.Translation(1, 0, 0) * Transformation.Scaling(0.8, 1, 0.8);
            conCentCyl1.Material = new Material();
            conCentCyl1.Material.Color = new Color(1, 1, 0.3);
            conCentCyl1.Material.Ambient = 0.1;
            conCentCyl1.Material.Diffuse = 0.8;
            conCentCyl1.Material.Specular = 0.9;
            conCentCyl1.Material.Shininess = 300;

            var conCentCyl2 = new Cylinder();
            conCentCyl2.Minimum = 0;
            conCentCyl2.Maximum = 0.3;
            conCentCyl2.IsClosed = false;
            conCentCyl2.Transform = Transformation.Translation(1, 0, 0) * Transformation.Scaling(0.6, 1, 0.6);
            conCentCyl2.Material = new Material();
            conCentCyl2.Material.Color = new Color(1, 0.9, 0.4);
            conCentCyl2.Material.Ambient = 0.1;
            conCentCyl2.Material.Diffuse = 0.8;
            conCentCyl2.Material.Specular = 0.9;
            conCentCyl2.Material.Shininess = 300;

            var conCentCyl3 = new Cylinder();
            conCentCyl3.Minimum = 0;
            conCentCyl3.Maximum = 0.4;
            conCentCyl3.IsClosed = false;
            conCentCyl3.Transform = Transformation.Translation(1, 0, 0) * Transformation.Scaling(0.4, 1, 0.4);
            conCentCyl3.Material = new Material();
            conCentCyl3.Material.Color = new Color(1, 0.8, 0.5);
            conCentCyl3.Material.Ambient = 0.1;
            conCentCyl3.Material.Diffuse = 0.8;
            conCentCyl3.Material.Specular = 0.9;
            conCentCyl3.Material.Shininess = 300;

            var conCentCyl4 = new Cylinder();
            conCentCyl4.Minimum = 0;
            conCentCyl4.Maximum = 0.5;
            conCentCyl4.IsClosed = true;
            conCentCyl4.Transform = Transformation.Translation(1, 0, 0) * Transformation.Scaling(0.2, 1, 0.2);
            conCentCyl4.Material = new Material();
            conCentCyl4.Material.Color = new Color(1, 0.7, 0.6);
            conCentCyl4.Material.Ambient = 0.1;
            conCentCyl4.Material.Diffuse = 0.8;
            conCentCyl4.Material.Specular = 0.9;
            conCentCyl4.Material.Shininess = 300;

            World world = new World();
            world.Shapes = new List<Shape> {floor, cyl, decCyl1, decCyl2, decCyl3, decCyl4, glassCyl, conCentCyl1, conCentCyl2, conCentCyl3, conCentCyl4};
  
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

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/Cylinders.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}