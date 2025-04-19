using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class BasicSceneWithXYZTests : TestBase
    {
        [Fact]
        public void RenderBasicScene()
        {
            Cylinder xBase = new Cylinder();
            xBase.Maximum = 2;
            xBase.Minimum = 0;
            xBase.Transform = Transformation.Scaling(0.1, 1, 1);
            xBase.Material = new Material();
            xBase.Material.Color = new Color(1, 0.0, 0.0);
            xBase.Material.Specular = 50;
            xBase.Material.Diffuse = 0.1;
            xBase.Material.Ambient = 0.7;

            Cone xTop = new Cone();
            xTop.Maximum = 2.2;
            xTop.Minimum = 2.0;
            //xTop.Transform = Transformation.Translation(0, 2, 0) *
            //Transformation.Scaling(0.2, 0.2, 0.2);
            xTop.Material = new Material();
            xTop.Material.Color = new Color(1, 0.0, 0.0);
            xTop.Material.Specular = 50;
            xTop.Material.Ambient = 0.7;

            Cylinder yBase = new Cylinder();
            //yBase.Transform = Transformation.Scaling(0, 3, 0) *
            //                    Transformation.Rotation_z(90 * Math.PI / 180);
            yBase.Material = new Material();
            yBase.Material.Color = new Color(0, 1, 0);
            yBase.Material.Specular = 0;

            Cone yTop = new Cone();
            yTop.Transform = Transformation.Translation(0, 0, 0);
            yTop.Material = new Material();
            yTop.Material.Color = new Color(0, 1, 0);

            Cylinder zBase = new Cylinder();
            //zBase.Transform = Transformation.Scaling(0, 3, 0) *
            //                    Transformation.Rotation_y(-90 * Math.PI / 180);
            zBase.Material = new Material();
            zBase.Material.Color = new Color(0, 0, 1);
            zBase.Material.Specular = 0;

            Cone zTop = new Cone();
            yTop.Transform = Transformation.Translation(0, 0, 0);
            yTop.Material = new Material();
            yTop.Material.Color = new Color(0, 0, 1);

            World world = new World();
            world.Shapes = new List<Shape> { xBase, xTop };
            world.Lights = new List<ILight> { new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)) };

            Camera camera = new Camera(200, 120, Math.PI / 3);
            camera.Transform = Transformation.ViewTransform(new Point(0, 5, 5),
                                new Point(0, 1, 0),
                                new Vector(0, 1, 0));

            Canvas canvas = camera.Render(world);


            string filename = imagePath.ToString() + "BasicSceneWithXYZ.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}