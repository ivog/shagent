using System.IO;

namespace SHAgent
{
    public class FileSystemAdapter : IFileSystem
    {
        public bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }
    }
}