using System;

namespace RayTracer
{
    public class TextureMap : Pattern
    {
        public IUvPattern Texture { get; set; }
        public Func<Point, (double, double)> Mapper { get; set; }

        public TextureMap(IUvPattern texture, Func<Point, (double, double)> mapper)
        {
            this.Texture = texture;
            this.Mapper = mapper;
        }

        public override Color PatternAt(Point point)
        {
            var (u, v) = this.Mapper(point);
            return Texture.UvPatternAt(u, v);
        }
    }
}