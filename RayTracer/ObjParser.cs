using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer
{
    public class ObjParser
    {
        public int IgnoredCount = 0;
        public string data;
        public List<Point> Vertices = new List<Point>();
        public List<Vector> Normals = new List<Vector>();
        private List<string> knownCommands = new List<string> {"v", "f", "g", "vn"};
        public Group DefaultGroup;
        public List<Group> Groups;

        public ObjParser(string obj) 
        { 
            data = obj;
            DefaultGroup = new Group();
            Groups = new List<Group>();
        }

        private List<Triangle> FanTriangulation(List<Point> vertices, List<Vector> normals)
        {
            var triangles = new List<Triangle>();
            for (var index = 1; index < vertices.Count - 1; index++)
            {
                Triangle tri;
                if (normals.Count > index)
                    tri = new SmoothTriangle(vertices[0], vertices[index], vertices[index+1], normals[0], normals[index], normals[index+1]);
                else
                    tri = new Triangle(vertices[0], vertices[index], vertices[index+1]);
                triangles.Add(tri);
            }
            return triangles;
        }

        public List<Group> ObjToGroup()
        {
            var groups = new List<Group>();
            if (this.DefaultGroup.GetShapes().Any())
                groups.Add(this.DefaultGroup);
            groups.AddRange(this.Groups);
            return groups;
        }

        public void Parse()
        {
            // read data line by line            
            using (StringReader reader = new StringReader(data))
            {
                string line;
                Group currentGroup = DefaultGroup;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == string.Empty) continue;
                    // tokenize each line
                    string[] commands = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    // first token should be a command
                    switch (commands[0])
                    {
                        case "v" : 
                            this.Vertices.Add( 
                                new Point(double.Parse(commands[1]), double.Parse(commands[2]), double.Parse(commands[3])) 
                                );
                            break;
                        case "vn" :
                            this.Normals.Add(
                                new Vector(double.Parse(commands[1]), double.Parse(commands[2]), double.Parse(commands[3]))
                                );
                            break;
                        case "f" :
                            var vertices = new List<Point>();
                            var normals = new List<Vector>();
                            for (var i = 1; i < commands.Length; i++)
                            {
                                int vertNum;
                                int normNum;
                                if (commands[i].Contains("/"))
                                {
                                    vertNum = int.Parse(commands[i].Split('/')[0]);
                                    normNum = int.Parse(commands[i].Split('/')[2]);
                                    normals.Add( this.Normals[normNum - 1] );
                                } else
                                    vertNum = int.Parse(commands[i]);
                                vertices.Add( this.Vertices[vertNum - 1] );
                            }
                            currentGroup.AddTriangles(FanTriangulation(vertices, normals));
                            break;
                        case "g" :
                            var group = new Group();
                            group.Name = commands[1];
                            this.Groups.Add(group);
                            currentGroup = group;
                            break;
                        default : 
                            IgnoredCount += 1; 
                            break;
                    }
                }
            }
        }
    }
}