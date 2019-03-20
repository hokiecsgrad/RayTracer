using System;

namespace RayTracer
{
    public class Canvas
    {
        public int Width, Height;
        private Color[,] _pixels;

        public Canvas(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this._pixels = new Color[width, height];
            for (int w = 0; w < width; w++)
                for (int h = 0; h < height; h++)
                    _pixels[w, h] = new Color(0,0,0);
        }

        public Color GetPixel(int width, int height)
        {
            if (width < 0) width = 0;
            if (width >= this.Width) width = this.Width - 1;
            if (height < 0) height = 0;
            if (height >= this.Height) height = this.Height - 1;
            return this._pixels[width, height];
        }

        public void SetPixel(int x, int y, Color color)
        {
            this._pixels[x, y] = color;
        }
    }
}
