namespace RayTracer
{
    public interface IProgressMonitor
    {
        void OnStarted();

        void OnRowStarted(int row);

        void OnPixelStarted(int row, int col);

        void OnPixelFinished(int row, int col);

        void OnRowFinished(int row);

        void OnFinished();
    }
}