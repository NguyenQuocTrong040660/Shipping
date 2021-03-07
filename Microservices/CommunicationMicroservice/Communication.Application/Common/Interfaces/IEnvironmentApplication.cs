namespace Communication.Application.Interfaces
{
    public interface IEnvironmentApplication
    {
        string WebRootPath { get; }
        string EnvironmentName { get; }
    }
}
