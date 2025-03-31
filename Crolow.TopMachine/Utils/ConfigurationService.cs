using System.Text.Json;

namespace Crolow.TopMachine.Utils;

public static class ConfigurationService
{
    private static readonly string AppDataPath =
        FileSystem.AppDataDirectory; // Platform-specific writable directory.

    public static T LoadConfiguration<T>(string fileName)
    {
        string filePath = Path.Combine(AppDataPath, fileName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Configuration file not found: {fileName}");

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json) ?? throw new InvalidOperationException("Invalid configuration format.");
    }

    public static void SaveConfiguration<T>(string fileName, T configuration)
    {
        string filePath = Path.Combine(AppDataPath, fileName);

        var json = JsonSerializer.Serialize(configuration, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }

    public static void EnsureConfigurationsExist()
    {
        string[] configFiles = { "appsettings.json", "appsettings.dev.json", "appsettings.prod.json" };

        foreach (var fileName in configFiles)
        {
            string sourcePath = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);
            string destinationPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            if (!File.Exists(destinationPath))
            {
                File.Copy(sourcePath, destinationPath);
            }
        }
    }

}
