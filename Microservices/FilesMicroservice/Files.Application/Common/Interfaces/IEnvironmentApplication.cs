namespace Files.Application.Common.Interfaces
{
    public interface IEnvironmentApplication
    {
        string WebRootPath { get; }
        string EnvironmentName { get; }
    }
}
