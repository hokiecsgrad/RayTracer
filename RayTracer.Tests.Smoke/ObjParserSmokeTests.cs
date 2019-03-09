using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class ObjParserSmokeTests
    {
        [Fact]
        public void RenderBasicScene()
        {
            var objFile = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/models/utah_teapot_hires.obj";
            //var objFile = "/Users/ryan.hagan/Documents/VSCode Proejects/RayTracer/RayTracer.Tests.Smoke/models/utah_teapot_hires.obj";
            FileStream instream = File.OpenRead(objFile);
            StreamReader reader = new StreamReader(instream);
            var objData = reader.ReadToEnd();
            var parser = new ObjParser(objData);
            parser.Parse();            
            var teapot = new Group();
            teapot.AddShapes(parser.Groups);
            teapot.Divide(200);

            World world = new World();
            world.Shapes = new List<Shape> {teapot};

            // ======================================================
            // light sources
            // ======================================================

            world.Lights = new List<ILight> { new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)) };

            // ======================================================
            // the camera
            // ======================================================

            Camera camera = new Camera(400, 300, Math.PI/2);
            camera.Transform = Transformation.ViewTransform(
                                new Point(-1.5, 1.5, 0), // view from
                                new Point(0, 0.5, 0),// view to
                                new Vector(0, 1, 0));    // vector up

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/SmoothTeapot.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();        
        }
    }
}