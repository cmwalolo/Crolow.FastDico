
using Kalow.Apps.Common.DataTypes;

namespace Crolow.Pix.Core.FileStorage
{
    public interface IFileRepository
    {
        byte[] GetFile(KalowId fileId);
        string UploadFile(KalowId id, string filePath, Stream bytes, Dictionary<string, string> metadata);
    }
}