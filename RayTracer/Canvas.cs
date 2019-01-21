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
            return this._pixels[width, height];
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (x > 0 && x < Width && y > 0 && y < Height)
                this._pixels[x, y] = color;
        }
    }
}
