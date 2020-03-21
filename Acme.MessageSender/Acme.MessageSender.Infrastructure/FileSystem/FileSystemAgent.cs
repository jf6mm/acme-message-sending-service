using Acme.MessageSender.Infrastructure.Interfaces;
using System.IO;

namespace Acme.MessageSender.Infrastructure.FileSystem
{
    public class FileSystemAgent : IFileSystemAgent
    {
        public byte[] ReadFileBytes(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            return File.ReadAllBytes(filePath);
        }

        public void WriteFileBytes(string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        public void CreateDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        public string ReadFileText(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            return File.ReadAllText(filePath);
        }

        public void WriteFileText(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }
    }
}
