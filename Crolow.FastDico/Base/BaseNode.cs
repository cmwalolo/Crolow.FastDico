using Crolow.Fast.Dawg.Utils;

namespace Crolow.Fast.Dawg.Dicos;

public class BaseNode
{
    /// <summary>
    /// In Gaddag the Letter 31 represents a pivot node.
    /// </summary>
    public byte Control;
    public byte Letter;
    public bool IsEnd { get { return (Control & DawgUtils.IsEnd) == DawgUtils.IsEnd; } }
    public void SetEnd() { Control |= DawgUtils.IsEnd; }

}
