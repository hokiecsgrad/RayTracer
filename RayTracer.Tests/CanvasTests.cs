using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace RayTracer.Tests
{
    public class CanvasTests
    {
        const double epsilon = 0.0001;

        static readonly IEqualityComparer<Color> ColorComparer =
            Color.GetEqualityComparer(epsilon);

        [Fact]
        public void CreateDefaultCanvas_ShouldReturnAllBlackCanvas()
        {
            var myCanvas = new Canvas(10, 20);
            Assert.Equal(10, myCanvas.Width);
            Assert.Equal(20, myCanvas.Height);
            for (int w = 0; w < myCanvas.Width; w++)
                for (int h = 0; h < myCanvas.Height; h++)
                    Assert.Equal(new Color(0,0,0), myCanvas.GetPixel(w,h));
        }

        [Fact]
        public void SetPixelOnCanvas_ShouldChangeOnePixel()
        {
            var myCanvas = new Canvas(10, 20);
            var red = new Color(1, 0, 0);
            myCanvas.SetPixel(2, 3, red);
            Assert.Equal(new Color(1, 0, 0), myCanvas.GetPixel(2, 3));
        }

        [Fact]
        public void SaveBlankCanvas_ShouldWriteCanvasToStream()
        {
            var writer = new StringWriter();
            var myCanvas = new Canvas(5, 3);
            PpmWriter.WriteCanvasToPpm(writer, myCanvas);
            writer.Close();
            Assert.Equal(
@"P3
5 3
255
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
", writer.ToString());
        }

        [Fact]
        public void SaveColoredCanvas_ShouldWriteCanvasToStream()
        {
            var writer = new StringWriter();
            var myCanvas = new Canvas(5, 3);
            var red = new Color(1.5, 0, 0);
            var blueish = new Color(0, 0.5, 0);
            var green = new Color(-0.5, 0, 1);
            myCanvas.SetPixel(0, 0, red);
            myCanvas.SetPixel(2, 1, blueish);
            myCanvas.SetPixel(4, 2, green);
            PpmWriter.WriteCanvasToPpm(writer, myCanvas);
            writer.Close();
            Assert.Equal(
@"P3
5 3
255
255 0 0 0 0 0 0 0 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 128 0 0 0 0 0 0 0 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 255 
", writer.ToString());
        }

        [Fact]
        public void PpmFileWithLongLines_ShouldSplitLinesAt70CharsMax()
        {
            var writer = new StringWriter();
            var myCanvas = new Canvas(10, 2);
            var myColor = new Color(1, 0.8, 0.6);
            for (int w = 0; w < 10; w++)
                for (int h = 0; h < 2; h++)
                    myCanvas.SetPixel(w, h, myColor);
            PpmWriter.WriteCanvasToPpm(writer, myCanvas);
            writer.Close();
            Assert.Equal(
@"P3
10 2
255
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 
153 255 204 153 255 204 153 255 204 153 255 204 153 
255 204 153 255 204 153 255 204 153 255 204 153 255 204 153 255 204 
153 255 204 153 255 204 153 255 204 153 255 204 153 
", writer.ToString());
        }

        [Fact]
        public void PpmFiles_ShoudBeTerminatedWithNewLine()
        {
            var writer = new StringWriter();
            var myCanvas = new Canvas(5, 3);
            PpmWriter.WriteCanvasToPpm(writer, myCanvas);
            writer.Close();
            Assert.Equal("\n", writer.ToString().Substring(writer.ToString().Length - 1));
        }

        [Fact]
        public void ReadingPpmFileWithTheWrongMagicNumber_ShouldFail()
        {
            var file = 
@"P32
1 1
255
0 0 0";

            var reader = new StringReader(file);
            var canvas = PpmReader.ReadCanvasFromPpm(reader);
            Assert.Equal(0, canvas.Width);
            Assert.Equal(0, canvas.Height);
        }

        [Fact]
        public void ReadingPpmFile_ShouldReturnCanvasWithCorrectDimensions()
        {
            var file =
@"P3
10 2
255
0 0 0  0 0 0  0 0 0  0 0 0  0 0 0
0 0 0  0 0 0  0 0 0  0 0 0  0 0 0
0 0 0  0 0 0  0 0 0  0 0 0  0 0 0
0 0 0  0 0 0  0 0 0  0 0 0  0 0 0";

            var reader = new StringReader(file);
            var canvas = PpmReader.ReadCanvasFromPpm(reader);
            Assert.Equal(10, canvas.Width);
            Assert.Equal(2, canvas.Height);
        }

        [Theory]
        [MemberData(nameof(GetCanvasColorData))]
        public void ReadingPixelDataFromPpmFile_ShouldCreateCanvas(int x, int y, Color expected)
        {
            var file = 
@"P3
4 3
255
255 127 0  0 127 255  127 255 0  255 255 255
0 0 0  255 0 0  0 255 0  0 0 255
255 255 0  0 255 255  255 0 255  127 127 127";

            var reader = new StringReader(file);
            var canvas = PpmReader.ReadCanvasFromPpm(reader);
            Assert.Equal(expected, canvas.GetPixel(x, y), ColorComparer);
        }

        public static IEnumerable<object[]> GetCanvasColorData()
        {
            var allData = new List<object[]>
            {
                new object[] { 0, 0, new Color(1, 0.498, 0) },
                new object[] { 1, 0, new Color(0, 0.498, 1) },
                new object[] { 2, 0, new Color(0.498, 1, 0) },
                new object[] { 3, 0, new Color(1, 1, 1) },
                new object[] { 0, 1, new Color(0, 0, 0) },
                new object[] { 1, 1, new Color(1, 0, 0) },
                new object[] { 2, 1, new Color(0, 1, 0) },
                new object[] { 3, 1, new Color(0, 0, 1) },
                new object[] { 0, 2, new Color(1, 1, 0) },
                new object[] { 1, 2, new Color(0, 1, 1) },
                new object[] { 2, 2, new Color(1, 0, 1) },
                new object[] { 3, 2, new Color(0.498, 0.498, 0.498) },
            };

            return allData;
        }

        [Fact]
        public void ReadingPpmFile_ShouldIgnoreCommentLines()
        {
            var file =
@"P3
# this is a comment
2 1
# this, too
255
# another comment
255 255 255
# oh, no, comments in the pixel data!
255 0 255";

            var reader = new StringReader(file);
            var canvas = PpmReader.ReadCanvasFromPpm(reader);
            Assert.Equal(Color.White, canvas.GetPixel(0, 0));
            Assert.Equal(new Color(1, 0, 1), canvas.GetPixel(1, 0));
        }

        [Fact]
        public void ReadingPpmFile_ShouldAllowAnRgbTripleToSpanLines()
        {
            var file = 
@"P3
1 1
255
51
153

204";

            var reader = new StringReader(file);
            var canvas = PpmReader.ReadCanvasFromPpm(reader);
            Assert.Equal(new Color(0.2, 0.6, 0.8), canvas.GetPixel(0, 0));
        }

        [Fact]
        public void ReadingPpmFile_ShouldRespectTheScaleSetting()
        {
            var file =
@"P3
2 2
100
100 100 100  50 50 50
75 50 25  0 0 0";

            var reader = new StringReader(file);
            var canvas = PpmReader.ReadCanvasFromPpm(reader);
            Assert.Equal(new Color(0.75, 0.5, 0.25), canvas.GetPixel(0, 1));
        }
    }
}
