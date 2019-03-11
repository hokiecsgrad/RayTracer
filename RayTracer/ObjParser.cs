using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class ObjParser
    {
        private string data;

        public Point Min { get; private set; } = new Point(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);

        public Point Max { get; private set; } = new Point(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);

        public List<Point> Vertices { get; } = new List<Point>();

        public List<Vector> Normals { get; } = new List<Vector>();

        public List<Shape> Groups { get; } = new List<Shape>();

        private bool IsDirty { get; set; } = true;

        public ObjParser(string obj) 
        { 
            data = obj;
        }

        private List<Shape> FanTriangulation(List<Point> vertices, List<Vector> normals)
        {
            var triangles = new List<Shape>();
            for (var index = 1; index < vertices.Count - 1; index++)
            {
                Triangle tri;
                if (normals.Count > index)
                {
                    tri = new SmoothTriangle(
                            vertices[0], 
                            vertices[index], 
                            vertices[index+1], 
                            normals[0], 
                            normals[index], 
                            normals[index+1]);
                }
                else
                {
                    tri = new Triangle(
                            vertices[0], 
                            vertices[index], 
                            vertices[index+1]);
                }
                triangles.Add(tri);
            }
            return triangles;
        }

        public void Parse()
        {
            using (StringReader reader = new StringReader(data))
            {
                string line;
                Group currentGroup = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty) continue;

                    string[] commands = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                    switch (commands[0])
                    {
                        case "v" : 
                            var px = double.Parse(commands[1]);
                            var py = double.Parse(commands[2]);
                            var pz = double.Parse(commands[3]);

                            this.Vertices.Add(new Point(px, py, pz)); 

                            this.IsDirty = true;

                            break;

                        case "vn" :
                            var vx = double.Parse(commands[1]);
                            var vy = double.Parse(commands[2]);
                            var vz = double.Parse(commands[3]);

                            this.Normals.Add(new Vector(vx, vy, vz));

                            break;
                            
                        case "f" :
                            this.CalculateMinAndMax();
                            this.Normalize();

                            var vertices = new List<Point>();
                            var normals = new List<Vector>();

                            if (currentGroup == null) 
                            {
                                currentGroup = new Group("Default");
                                this.Groups.Add(currentGroup);
                            }

                            for (var i = 1; i < commands.Length; i++)
                            {
                                int vertNum;
                                int normNum;

                                if (commands[i].Contains("/"))
                                {
                                    vertNum = int.Parse(commands[i].Split('/')[0]);
                                    normNum = int.Parse(commands[i].Split('/')[2]);
                                    normals.Add( this.Normals[normNum - 1] );
                                } 
                                else
                                    vertNum = int.Parse(commands[i]);

                                vertices.Add( this.Vertices[vertNum - 1] );
                            }

                            currentGroup.AddShapes(FanTriangulation(vertices, normals));

                            break;

                        case "g" :
                            var group = new Group();
                            group.Name = commands[1];
                            this.Groups.Add(group);
                            currentGroup = group;

                            break;

                        default : 
                            break;
                    }
                }
            }

            this.CalculateMinAndMax();
        }

        private void CalculateMinAndMax()
        {
            if (!this.Vertices.Any()) return;
            if (!this.IsDirty) return;

            var minX = this.Vertices.Min(point => point.x);
            var minY = this.Vertices.Min(point => point.y);
            var minZ = this.Vertices.Min(point => point.z);
            this.Min = new Point(minX, minY, minZ);

            var maxX = this.Vertices.Max(point => point.x);
            var maxY = this.Vertices.Max(point => point.y);
            var maxZ = this.Vertices.Max(point => point.z);
            this.Max = new Point(maxX, maxY, maxZ);
        }

        public void Normalize()
        {
            if (! this.IsDirty) return;

            var sx = this.Max.x - this.Min.x;
            var sy = this.Max.y - this.Min.y;
            var sz = this.Max.z - this.Min.z;

            var scale = Math.Max(sx, Math.Max(sy, sz)) / 2;

            foreach (var vertex in this.Vertices)
            {
                vertex.x = (vertex.x - (this.Min.x + sx / 2)) / scale;
                vertex.y = (vertex.y - (this.Min.y + sy / 2)) / scale;
                vertex.z = (vertex.z - (this.Min.z + sz / 2)) / scale;
            }

            this.CalculateMinAndMax();
            this.IsDirty = false;
        }
    }
}