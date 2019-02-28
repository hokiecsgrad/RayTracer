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
            var objFile = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/models/teapot.obj";
            FileStream instream = File.OpenRead(objFile);
            StreamReader reader = new StreamReader(instream);
            var objData = reader.ReadToEnd();
            var parser = new ObjParser(objData);
            parser.Parse();            
            var teapot = new Group();
            teapot.AddGroups(parser.ObjToGroup());

            World world = new World();
            world.Shapes = new List<Shape> {teapot};

            // ======================================================
            // light sources
            // ======================================================

            world.Light = new PointLight(new Point(1, 6.9, -4.9), new Color(1, 1, 1));

            // ======================================================
            // the camera
            // ======================================================

            Camera camera = new Camera(400, 300, 0.314);
            camera.Transform = Transformation.ViewTransform(
                                new Point(15, 10, -15), // view from
                                new Point(0, 1.5, 0),// view to
                                new Vector(0, 1, 0));    // vector up

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/Teapot.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();        
        }
    }
}