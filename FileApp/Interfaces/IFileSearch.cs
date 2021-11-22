using FileApp.Models;
using System.Collections.Generic;

namespace FileApp.Interfaces
{
    public interface IFileSearch
    {
        List<SearchResult> SearchFiles(string path, string pattern, string term);
    }
}
