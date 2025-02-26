using Crolow.Fast.Dawg.Utils;

namespace Crolow.Fast.Dawg.ScrabbleApi;

public class Rack
{
    public List<byte> Letters { get; private set; }

    public Rack(List<byte> letters)
    {
        Letters = new List<byte>(letters);
    }

    // Example: Convert from human-readable string to byte representation
    public static Rack FromString(string letters)
    {
        var bytes = DawgUtils.ConvertWordToBytes(letters); // Reuse existing utility
        return new Rack(bytes);
    }
}
