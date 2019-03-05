using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;


namespace RayTracer.Program
{
    class TeapotScene
    {
        public (World, Camera) Setup(int width, int height, double fov)
        {
            // ======================================================
            // the camera
            // ======================================================

            var camera = new Camera(width, height, fov) 
            {
                Transform = Transformation.ViewTransform(
                                new Point(1, 0.5, -1),    // view from
                                new Point(0, 0, 0),      // view to
                                new Vector(0, 1, 0)), // vector up
            };

            // ======================================================
            // light sources
            // ======================================================

            var light = new PointLight(
                new Point(5, 5, -5),
                new Color(1, 1, 1)
            );

            // ======================================================
            // describe the elements of the scene
            // ======================================================

            //var objFile = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/models/utah_teapot_hires.obj";
            var objFile = "/Users/ryan.hagan/Documents/VSCode Proejects/RayTracer/RayTracer.Program/Scenes/teapot.obj";
            FileStream instream = File.OpenRead(objFile);
            StreamReader reader = new StreamReader(instream);
            var objData = reader.ReadToEnd();
            var parser = new ObjParser(objData);
            parser.Parse();            
            var teapot = new Group();
            teapot.AddShapes(parser.Groups);

            World world = new World();
            world.Shapes = new List<Shape> {teapot};
            world.Lights = new List<PointLight> {light};

            return (world, camera);
        }
    }
}