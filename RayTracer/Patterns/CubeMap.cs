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
            var face = TextureMapper.FaceFromPoint(point);
            var (u, v) = (0.0, 0.0);
            var color = Color.Black;

            switch (face)
            {
                case CubeFace.Front :
                    (u, v) = TextureMapper.CubeUvFront(point);
                    color = this.Front.UvPatternAt(u, v);
                    break;
                case CubeFace.Back :
                    (u, v) = TextureMapper.CubeUvBack(point);
                    color = this.Back.UvPatternAt(u, v);
                    break;
                case CubeFace.Left :
                    (u, v) = TextureMapper.CubeUvLeft(point);
                    color = this.Left.UvPatternAt(u, v);
                    break;
                case CubeFace.Right :
                    (u, v) = TextureMapper.CubeUvRight(point);
                    color = this.Right.UvPatternAt(u, v);
                    break;
                case CubeFace.Up :
                    (u, v) = TextureMapper.CubeUvUp(point);
                    color = this.Up.UvPatternAt(u, v);
                    break;
                case CubeFace.Down :
                    (u, v) = TextureMapper.CubeUvDown(point);
                    color = this.Down.UvPatternAt(u, v);
                    break;
            }

            return color;
        }
    }
}