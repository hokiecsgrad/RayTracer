using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class SphereInSphereSmokeTests
    {
        [Fact]
        public void RenderBasicScene()
        {
            var floor = new Plane();
            floor.Transform = Transformation.Translation(0, -10, 0);
            floor.Material = new Material();
            floor.Material.Pattern = new Checkers(new Color(0,0,0), new Color(1,1,1));
            floor.Material.Ambient = 0.1;
            floor.Material.Diffuse = 0.9;
            floor.Material.Specular = 0.9;
            floor.Material.Shininess = 200;

            var innerSphere = new Sphere();
            innerSphere.CastsShadow = false;
            innerSphere.Transform = Transformation.Scaling(0.5, 0.5, 0.5);
            innerSphere.Material = new Material();
            innerSphere.Material.Ambient = 0;
            innerSphere.Material.Diffuse = 0.1;
            innerSphere.Material.Specular = 0.9;
            innerSphere.Material.Shininess = 300;
            innerSphere.Material.Reflective = 1;
            innerSphere.Material.Transparency = 1;
            innerSphere.Material.RefractiveIndex = 1.00029;

            var outerSphere = new Sphere();
            outerSphere.CastsShadow = false;
            outerSphere.Material = new Material();
            outerSphere.Material.Ambient = 0;
            outerSphere.Material.Diffuse = 0.1;
            outerSphere.Material.Specular = 0.9;
            outerSphere.Material.Shininess = 300;
            outerSphere.Material.Reflective = 1;
            outerSphere.Material.Transparency = 1;
            outerSphere.Material.RefractiveIndex = 1.52;

            World world = new World();
            world.Shapes = new List<Shape> {floor, innerSphere, outerSphere};
            world.Lights = new List<ILight> { new PointLight(new Point(20, 10, 0), new Color(0.7, 0.7, 0.7)) };

            Camera camera = new Camera(300, 300, Math.PI/3);
            camera.Transform = Transformation.ViewTransform(new Point(0, 2.5, 0),
                                new Point(0, 0, 0),
                                new Vector(-1, 0, 0));

            Canvas canvas = camera.Render(world);

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/SphereInSphere.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}