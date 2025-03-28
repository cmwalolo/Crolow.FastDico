using Crolow.TopMachine.Data.Interfaces;
using Kalow.Apps.Common.DataTypes;
using LiteDB;

namespace Crolow.TopMachine.Data;
public class FileRepository : IDisposable, IFileRepository
{
    private string DatabaseName = "Media";
    private readonly string _databasePath;
    private LiteDatabase db;

    public FileRepository(DatabaseSettings settings)
    {
        _databasePath = settings.Values.Where(p => p.Name == DatabaseName).First().ConnectionString;
        db = new LiteDatabase(_databasePath);
    }

    /// <returns>The ID of the stored file.</returns>
    public string UploadFile(KalowId id, string filePath, byte[] bytes, Dictionary<string, string> metadata)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var fileStorage = db.GetStorage<string>("files", "chunks");
        var fileId = id == KalowId.Empty ? KalowId.NewObjectId().ToString() : id.ToString();

        var mappedMetadata = BsonMapper.Global.ToDocument(metadata);

        using MemoryStream stream = new MemoryStream(bytes);
        fileStorage.Upload(fileId, Path.GetFileName(filePath), stream, mappedMetadata);

        return fileId;
    }

    public string UploadFile(KalowId id, string filePath, Stream bytes, Dictionary<string, string> metadata)
    {
        var fileStorage = db.GetStorage<string>("files", "chunks");
        var fileId = id == KalowId.Empty ? KalowId.NewObjectId().ToString() : id.ToString();

        BsonDocument mappedMetadata = metadata != null ? BsonMapper.Global.ToDocument(metadata) : null;

        fileStorage.Upload(fileId, Path.GetFileName(filePath), bytes, mappedMetadata);

        return fileId;
    }

    public byte[] GetFile(KalowId fileId)
    {
        var fileStorage = db.GetStorage<string>("files", "chunks");

        var fileInfo = fileStorage.FindById(fileId.ToString());
        if (fileInfo == null) return null;

        var file = new byte[fileInfo.Length];
        using (var stream = fileInfo.OpenRead())
        {
            stream.ReadExactly(file, 0, (int)fileInfo.Length);
        }
        return file;
    }

    public void Dispose()
    {
        if (db != null)
        {
            db.Dispose();
            db = null;
        }
    }
}
