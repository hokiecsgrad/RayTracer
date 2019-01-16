using System;
using System.IO;
using Xunit;

namespace RayTracer.Tests
{
    public class CanvasTests
    {
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
    }
}
