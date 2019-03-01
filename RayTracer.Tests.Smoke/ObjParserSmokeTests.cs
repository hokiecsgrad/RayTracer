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
            var objFile = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/models/teapot-low.obj";
            FileStream instream = File.OpenRead(objFile);
            StreamReader reader = new StreamReader(instream);
            var objData = reader.ReadToEnd();
            var parser = new ObjParser(objData);
            parser.Parse();            
            var teapot = new Group();
            teapot.AddGroups(parser.ObjToGroup());
            //teapot.Divide(200);

            World world = new World();
            world.Shapes = new List<Shape> {teapot};

            // ======================================================
            // light sources
            // ======================================================

            world.Light = new PointLight(new Point(1, 10, 20), new Color(1, 1, 1));

            // ======================================================
            // the camera
            // ======================================================

            Camera camera = new Camera(400, 300, Math.PI/2);
            camera.Transform = Transformation.ViewTransform(
                                new Point(25, 20, 15), // view from
                                new Point(0, 8, 4),// view to
                                new Vector(0, 0, 1));    // vector up

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