using Kalow.Apps.Common.DataTypes;

namespace Crolow.TopMachine.Data.Interfaces
{
    public interface IFileRepository
    {
        byte[] GetFile(KalowId fileId);
        string UploadFile(KalowId id, string filePath, Stream bytes, Dictionary<string, string> metadata);
    }
}