using System.Reflection;
using System.Text;

namespace MauiBlazorWeb.Shared.Services
{
    public class ResourceReaderService
    {
        public StringBuilder GetBoardConfigServiceStream()
        {
            return GetTextStream("Resources.Data.HYPERGRAMFR.txt");
        }
        public byte[] GetDawgStream()
        {
            return GetStream("Resources.Data.fr.dawg");
        }

        public StringBuilder GetTextStream(string path)
        {
            var info = Assembly.GetExecutingAssembly().GetName();
            var name = info.Name;

            var s = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            Console.WriteLine(s);

            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"{name}.{path}")!)
            {
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    return new StringBuilder(content);
                }
            }
        }


        public byte[] GetStream(string path)
        {
            var info = Assembly.GetExecutingAssembly().GetName();
            var name = info.Name;
            using var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"{name}.{path}")!;

            var b = new byte[stream.Length];
            stream.Read(b, 0, (int)stream.Length);
            return b;
        }


    }
}
