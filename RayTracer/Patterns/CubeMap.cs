using System;

namespace RayTracer
{
    public class CubeMap : Pattern
    {
        public IUvPattern Front { get; set; }
        public IUvPattern Back { get; set; }
        public IUvPattern Left { get; set; }
        public IUvPattern Right { get; set; }
        public IUvPattern Up { get; set; }
        public IUvPattern Down { get; set; }

        public CubeMap(IUvPattern left, IUvPattern front, IUvPattern right, IUvPattern back, IUvPattern up, IUvPattern down)
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