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

            var objFile = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Program/Scenes/teapot.obj";
            //var objFile = "/Users/ryan.hagan/Documents/VSCode Proejects/RayTracer/RayTracer.Program/Scenes/teapot.obj";
            FileStream instream = File.OpenRead(objFile);
            StreamReader reader = new StreamReader(instream);
            var objData = reader.ReadToEnd();
            var parser = new ObjParser(objData);
            parser.Parse();

            var teapot = new Group();
            var material = new Material()
            {
                Color = new Color(0.9, 0.9, 1),
                Ambient = 0.1,
                Diffuse = 0.6,
                Specular = 0.4,
                Shininess = 5,
                Reflective = 0.1,
            };
            teapot.AddShapes(parser.Groups);
            teapot.SetMaterial(material);

            var floor = new Plane()
            {
                Material = new Material()
                {
                    Pattern = new Checkers(Color.White, Color.Black),
                    Ambient = 0.9,
                    Diffuse = 0.2,
                    Specular = 0.0,
                },
                Transform = Transformation.Translation(0, -1, 0),
            };

            World world = new World();
            world.Shapes = new List<Shape> {floor, teapot};
            world.Lights = new List<ILight> {light};

            return (world, camera);
        }
    }
}