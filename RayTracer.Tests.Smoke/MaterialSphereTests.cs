using System;
using System.IO;
using Xunit;

namespace RayTracer.Tests.Smoke
{
    public class SphereMaterialTests
    {
        [Fact]
        public void ProjectSphereWithMaterialAgainstWall()
        {
            Point ray_origin = new Point(0, 0, -5);
            int canvas_pixels = 100;
            double wall_z = 10.0;
            double wall_size = 7.0;

            Canvas canvas = new Canvas(canvas_pixels, canvas_pixels);
            Sphere shape = new Sphere();
            shape.Material.Color = new Color(1, 0.2, 1);
            //shape.Material.Specular = 0.1;
            //shape.Material.Diffuse = 0.1;
            //shape.Material.Ambient = 0.5;

            var light_position = new Point(-10, -10, -10);
            var light_color = new Color(1, 1, 1);
            var light = new PointLight(light_position, light_color);

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
                    var hit_intersect = r.Hit(xs);
                    if (hit_intersect != null)
                    {
                        var hit_point = r.Position(hit_intersect.Time);
                        var normal = shape.Normal_at(hit_point);
                        var eye = -r.Direction;
                        var hit_color = shape.Material.Lighting(light, hit_point, eye, normal);
                        canvas.SetPixel(x, canvas.Height - y, hit_color);
                    }
                }
            }

            var filename = "/Users/rhagan/VSCode Projects/RayTracer/RayTracer.Tests.Smoke/SphereWithMaterialSmokeTest.ppm";
            if (File.Exists(filename))
                File.Delete(filename);
            FileStream stream = File.OpenWrite(filename);
            StreamWriter writer = new StreamWriter(stream);
            PpmWriter.WriteCanvasToPpm(writer, canvas);
            writer.Close();
        }
    }
}