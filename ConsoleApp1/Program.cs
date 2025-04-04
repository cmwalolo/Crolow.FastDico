using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string indexPath = "C:\\Users\\llequ\\OneDrive\\Documenten\\TopMachineConfig\\Index"; // Set your Lucene index path
        string outputPath = "C:\\dev\\Crolow.FastDico\\jsondico"; // Output directory

        int batchSize = 100;

        var dir = FSDirectory.Open(new DirectoryInfo(indexPath));
        var reader = IndexReader.Open(dir, true);

        int docCount = reader.NumDocs();
        int fileIndex = 1;
        var documents = new List<Dictionary<string, object>>();

        for (int i = 0; i < docCount; i++)
        {
            var doc = reader.Document(i);
            var docData = new Dictionary<string, object>();

            foreach (Field field in doc.GetFields())
            {
                docData[field.Name()] = field.StringValue() ?? "";
            }

            documents.Add(docData);

            if (documents.Count >= batchSize || i == docCount - 1)
            {
                string jsonFile = Path.Combine(outputPath, $"data_{fileIndex}.json");
                System.IO.File.WriteAllText(jsonFile, JsonSerializer.Serialize(documents, new JsonSerializerOptions { WriteIndented = true }));
                documents.Clear();
                fileIndex++;
            }
        }

        Console.WriteLine("Extraction completed.");
    }
}