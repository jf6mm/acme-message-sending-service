namespace Acme.MessageSender.Infrastructure.Interfaces
{
    public interface IFileSystemAgent
    {
        byte[] ReadFileBytes(string filePath);

        string ReadFileText(string filePath);

        void WriteFileBytes(string filePath, byte[] bytes);

        void WriteFileText(string filePath, string text);

        void CreateDirectory(string dirPath);
    }
}