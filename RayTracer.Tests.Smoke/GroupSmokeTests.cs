using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class GroupSmokeTests
    {
        private Shape hexagon_corner()
        {
            var corner = new Sphere();
            corner.Transform = Transformation.Translation(0, 0, -1) * Transformation.Scaling(0.25, 0.25, 0.25);
            return corner;
        }

        private Shape hexagon_edge()
        {
            var edge = new Cylinder();
            edge.Minimum = 0;
            edge.Maximum = 1;
            edge.Transform = Transformation.Translation(0, 0, -1) *
                                Transformation.Rotation_y(-Math.PI/6) *
                                Transformation.Rotation_z(-Math.PI/2) *
                                Transformation.Scaling(0.25, 1, 0.25);
            /*
            edge.Transform = Transformation.Scaling(0.25, 1, 0.25) * 
                                Transformation.Rotation_z(-Math.PI/2) *
                                Transformation.Rotation_y(-Math.PI/6) *
                                Transformation.Translation(0, 0, -1);
            */
            return edge;
        }

        private Shape hexagon_side()
        {
            var side = new Group();
            side.AddShape(hexagon_corner());
            side.AddShape(hexagon_edge());
            return side;
        }

        private Group hexagon()
        {
            var hex = new Group();
            for (int n = 0; n <= 5; n++)
            {
                var side = hexagon_side();
                side.Transform = Transformation.Rotation_y(n*Math.PI/3);
                hex.AddShape(side);
            }
            return hex;
        }

        [Fact]
        public void RenderBasicScene()
        {
            World world = new World();
            world.Shapes = new List<Shape> { hexagon() };

            // ======================================================
            // light sources
            // ======================================================

            world.Lights = new List<ILight> { new PointLight(new Point(0, 6.9, -5), new Color(1, 1, 0.9)) };

            // ======================================================
            // the camera
            // ======================================================

            Camera camera = new Camera(400, 200, 0.785);
            camera.Transform = Transformation.ViewTransform(
                                new Point(5, 4, 0), // view from
                                new Point(0, 0, 0),// view to
                                new Vector(0, 1, 0));    // vector up

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/Group.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}