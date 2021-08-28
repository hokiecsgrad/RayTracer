namespace RayTracer.Cli.Scenes
{
    public interface IScene
    {
        public (World, Camera) Setup(int width, int height, double fov);
    }
}