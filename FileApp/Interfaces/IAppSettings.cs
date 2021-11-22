namespace FileApp.Interfaces
{
    public interface IAppSettings
    {
        string Path { get; set; }
        string Pattern { get; set; }
        string Term { get; set; }
    }
}
