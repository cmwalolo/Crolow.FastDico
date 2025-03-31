namespace Crolow.TopMachine.Startup
{
    public class EnsureFiles
    {
        public static async Task CopyAssetsToAppDataDirectoryAsync()
        {
            // List of files to copy
            var files = new[] { "gaddag_fr.gz" };

            foreach (var file in files)
            {
                try
                {
                    // Open the file from the app package
                    using var stream = await FileSystem.OpenAppPackageFileAsync(file);

                    // Define the target path in AppDataDirectory
                    string targetPath = Path.Combine(FileSystem.AppDataDirectory, file);

                    if (!File.Exists(targetPath))
                    {
                        // Copy the file to AppDataDirectory
                        using var fileStream = File.Create(targetPath);
                        await stream.CopyToAsync(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
