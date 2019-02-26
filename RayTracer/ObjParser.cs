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
        private List<string> knownCommands = new List<string> {"v", "f", "g"};
        public Group DefaultGroup;
        public List<Group> Groups;

        public ObjParser(string obj) 
        { 
            data = obj;
            DefaultGroup = new Group();
            Groups = new List<Group>();
        }

        private List<Triangle> FanTriangulation(List<Point> vertices)
        {
            var triangles = new List<Triangle>();
            for (var index = 1; index < vertices.Count - 1; index++)
            {
                var tri = new Triangle(vertices[0], vertices[index], vertices[index+1]);
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
                    // tokenize each line
                    string[] commands = line.Split(new char[0]);
                    // first token should be a command
                    switch (commands[0])
                    {
                        case "v" : 
                            this.Vertices.Add( 
                                new Point(double.Parse(commands[1]), double.Parse(commands[2]), double.Parse(commands[3])) 
                                );
                            break;
                        case "f" :
                            var vertices = new List<Point>();
                            for (var i = 1; i < commands.Length; i++)
                                vertices.Add( this.Vertices[int.Parse(commands[i]) - 1] );
                            currentGroup.AddTriangles(FanTriangulation(vertices));
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