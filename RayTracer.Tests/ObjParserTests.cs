using System;
using Xunit;

namespace RayTracer.Tests
{
    public class ObjParserTests
    {
        [Fact]
        public void UnrecognizedLines_ShouldBeIgnoredAndCounted()
        {
            var gibberish = 
@"There was a young lady named Bright
who traveled much faster than light.
She set out one day
in a relative way,
and came back the previous night.";
            var parser = new ObjParser(gibberish);
            parser.Parse();
            Assert.Equal(5, parser.IgnoredCount);
        }

        [Fact]
        public void ParsingVertexRecords_ShouldReturnArrayOfPoints()
        {
            var file =
@"v -1 1 0
v -1.0000 0.5000 0.0000
v 1 0 0
v 1 1 0";
            
            var parser = new ObjParser(file);
            parser.Parse();
            Assert.True(parser.Vertices[0].Equals( new Point(-1, 1, 0) ));
            Assert.True(parser.Vertices[1].Equals( new Point(-1, 0.5, 0) ));
            Assert.True(parser.Vertices[2].Equals( new Point(1, 0, 0) ));
            Assert.True(parser.Vertices[3].Equals( new Point(1, 1, 0) ));
        }

        [Fact]
        public void ParsingTriangleFaces_ShouldReturnGroupsAndVetices()
        {
            var file =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
f 1 2 3
f 1 3 4";

            var parser = new ObjParser(file);
            parser.Parse();
            var g = parser.DefaultGroup;
            Triangle t1 = (object)g.GetShapes()[0] as Triangle;
            Triangle t2 = (object)g.GetShapes()[1] as Triangle;
            Assert.Equal(t1.p1, parser.Vertices[0]);
            Assert.Equal(t1.p2, parser.Vertices[1]);
            Assert.Equal(t1.p3, parser.Vertices[2]);
            Assert.Equal(t2.p1, parser.Vertices[0]);
            Assert.Equal(t2.p2, parser.Vertices[2]);
            Assert.Equal(t2.p3, parser.Vertices[3]);
        }

        [Fact]
        public void ParsingPolygons_ShouldBreakApartIntoTriangles()
        {
            var file =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
v 0 2 0
f 1 2 3 4 5";

            var parser = new ObjParser(file);
            parser.Parse();
            var g = parser.DefaultGroup;
            Triangle t1 = (object)g.GetShapes()[0] as Triangle;
            Triangle t2 = (object)g.GetShapes()[1] as Triangle;
            Triangle t3 = (object)g.GetShapes()[2] as Triangle;
            Assert.Equal(t1.p1, parser.Vertices[0]);
            Assert.Equal(t1.p2, parser.Vertices[1]);
            Assert.Equal(t1.p3, parser.Vertices[2]);
            Assert.Equal(t2.p1, parser.Vertices[0]);
            Assert.Equal(t2.p2, parser.Vertices[2]);
            Assert.Equal(t2.p3, parser.Vertices[3]);
            Assert.Equal(t3.p1, parser.Vertices[0]);
            Assert.Equal(t3.p2, parser.Vertices[3]);
            Assert.Equal(t3.p3, parser.Vertices[4]);
        }

        [Fact]
        public void ParsingFilesWithGroupNames_ShouldPutTrianglesInGivenGroups()
        {
            var file =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
g FirstGroup
f 1 2 3
g SecondGroup
f 1 3 4";

            var parser = new ObjParser(file);
            parser.Parse();
            var g = parser.Groups;
            Triangle t1 = (object)g[0].GetShapes()[0] as Triangle;
            Triangle t2 = (object)g[1].GetShapes()[0] as Triangle;
            Assert.Equal(t1.p1, parser.Vertices[0]);
            Assert.Equal(t1.p2, parser.Vertices[1]);
            Assert.Equal(t1.p3, parser.Vertices[2]);
            Assert.Equal(t2.p1, parser.Vertices[0]);
            Assert.Equal(t2.p2, parser.Vertices[2]);
            Assert.Equal(t2.p3, parser.Vertices[3]);
        }

        [Fact]
        public void LoadingObjFile_ShouldConvertEverythingToGroup()
        {
            var file =
@"v -1 1 0
v -1 0 0
v 1 0 0
v 1 1 0
g FirstGroup
f 1 2 3
g SecondGroup
f 1 3 4";

            var parser = new ObjParser(file);
            parser.Parse();
            var g = new Group();
            g.AddGroups(parser.ObjToGroup());
            Assert.True(true);
        }
    }
}