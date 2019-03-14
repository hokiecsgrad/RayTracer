using System;

namespace RayTracer
{
    public class TextureMap : Pattern
    {
        public UvCheckers Texture { get; set; }

        public TextureMap(UvCheckers texture)
        {
            this.Texture = texture;
        }

        public override Color PatternAt(Point point)
        {
            var (u, v) = Texture.PlanarMap(point);
            return Texture.UvPatternAt(u, v);
        }
    }
}