using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class PatternsWithShapesTests : TestBase
    {
        [Fact]
        public void RenderBasicScene()
        {
            Plane floor = new Plane();
            floor.Material = new Material();
            floor.Material.Pattern = new Checkers(new Color(0.85, 0, 0), new Color(0.85, 0.85, 0.85));
            floor.Material.Pattern.Transform = Transformation.Translation(0, 0.01, 0);

            Sphere left = new Sphere();
            left.Transform = Transformation.Translation(-1.40, 0.33, -0.50) * Transformation.Scaling(0.33, 0.33, 0.33);
            left.Material = new Material();
            left.Material.Pattern = new Gradient(new Color(0.85, 0, 0), new Color(0.85, 0.85, 0.85));
            left.Material.Pattern.Transform = Transformation.Scaling(2, 1, 1) * Transformation.Translation(-0.5, 0.0, 0.0);

            Sphere mid = new Sphere();
            mid.Transform = Transformation.Translation(-1, 1, 0.75);
            mid.Material = new Material();
            mid.Material.Pattern = new Gradient(new Color(0.85, 0, 0), new Color(0.85, 0.85, 0.85));
            mid.Material.Pattern.Transform = Transformation.Scaling(2, 1, 1) * Transformation.Translation(-0.5, 0.0, 0.0);

            Sphere right = new Sphere();
            right.Transform = Transformation.Translation(-0.25, 0.5, -0.5) * Transformation.Scaling(0.5, 0.5, 0.5);
            right.Material = new Material();
            right.Material.Pattern = new Gradient(new Color(0.85, 0, 0), new Color(0.85, 0.85, 0.85));
            right.Material.Pattern.Transform = Transformation.Scaling(2, 1, 1) * Transformation.Translation(-0.5, 0.0, 0.0);

            World world = new World();
            world.Shapes = new List<Shape> {floor, left, mid, right};
            world.Lights = new List<ILight> { new PointLight(new Point(-10, 10, -10), new Color(1, 1, 1)) };

            Camera camera = new Camera(200, 120, Math.PI/3);
            camera.Transform = Transformation.ViewTransform(new Point(0, 1.5, -5),
                                new Point(0, 1, 0),
                                new Vector(0, 1, 0));

            Canvas canvas = camera.Render(world);


            string filename = imagePath.ToString() + "PatternsWithShapes.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}