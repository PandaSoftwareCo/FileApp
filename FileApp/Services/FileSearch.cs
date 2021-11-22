using FileApp.Interfaces;
using FileApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileApp.Services
{
    public class FileSearch : IFileSearch
    {
        public List<SearchResult> SearchFiles(string path, string pattern, string term)
        {
            var files = (from file in Directory.EnumerateFiles(path, pattern, SearchOption.AllDirectories)
                         from line in File.ReadLines(file)
                         //where line.Contains("GraphQL")
                         where line.Contains(term)
                         select new SearchResult
                         {
                             File = file,
                             Line = line.Trim()
                         }).ToList();

            return files;
        }
    }
}
