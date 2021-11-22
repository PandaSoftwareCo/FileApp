using FileApp.Interfaces;

namespace FileApp.Models
{
    public class AppSettings : IAppSettings
    {
        public string Path { get; set; }
        public string Pattern { get; set; }
        public string Term { get; set; }
    }
}
