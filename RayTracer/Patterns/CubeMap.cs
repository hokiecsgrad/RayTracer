using System;

namespace RayTracer
{
    public class CubeMap : Pattern
    {
        public UvAlignCheck Front { get; set; }
        public UvAlignCheck Back { get; set; }
        public UvAlignCheck Left { get; set; }
        public UvAlignCheck Right { get; set; }
        public UvAlignCheck Up { get; set; }
        public UvAlignCheck Down { get; set; }

        public CubeMap(UvAlignCheck left, UvAlignCheck front, UvAlignCheck right, UvAlignCheck back, UvAlignCheck up, UvAlignCheck down)
        {
            this.Front = front;
            this.Back = back;
            this.Left = left;
            this.Right = right;
            this.Up = up;
            this.Down = down;
        }

        public override Color PatternAt(Point point)
        {
            return Color.Black;
        }
    }
}