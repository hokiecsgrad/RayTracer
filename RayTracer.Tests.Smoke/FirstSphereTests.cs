using System;
using System.IO;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class SphereTests
    {
        [Fact]
        public void ProjectSphereAgainstWall()
        {
            Point ray_origin = new Point(0, 0, -5);
            int canvas_pixels = 100;
            double wall_z = 10.0;
            double wall_size = 7.0;

            Canvas canvas = new Canvas(canvas_pixels, canvas_pixels);
            Color color = new Color(1, 0, 0);
            Sphere shape = new Sphere();
            //shape.Transform = Transformation.Scaling(1, 0.5, 1);
            //shape.Transform = Transformation.Scaling(0.5, 1, 1);
            //shape.Transform = Transformation.Rotation_z(Math.PI / 4) * Transformation.Scaling(0.5, 1, 1);
            //shape.Transform = Transformation.Shearing(1, 0, 0, 0, 0, 0) * Transformation.Scaling(0.5, 1, 1);

            double pixel_size = wall_size / canvas_pixels;
            double half = wall_size / 2;

            for (int y = 0; y < canvas.Height; y++)
            {
                // compute the world y coordinate (top = +half, bottom = -half)
                double world_y = half - pixel_size * y;
                for (int x = 0; x < canvas.Width; x++)
                {
                    double world_x = -half + pixel_size * x;
                    Point position = new Point(world_x, world_y, wall_z);
                    Ray r = new Ray(ray_origin, (position - ray_origin).Normalize());
                    Intersection[] xs = r.Intersect(shape);
                    if (r.Hit(xs) != null)
                        canvas.SetPixel(x, canvas.Height - y, color);
                }
            }

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/FirstSphereSmokeTest.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}