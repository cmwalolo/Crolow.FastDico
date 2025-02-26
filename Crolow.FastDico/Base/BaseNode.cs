using Crolow.Fast.Dawg.Utils;

namespace Crolow.Fast.Dawg.Dicos;

public class BaseNode
{
    /// <summary>
    /// Control & Letter could be combined and offer 
    /// even more compression. I left both separated
    /// for an eventually easier extension of the Letter 
    /// if you need to provide in your dictionary more
    /// characters.
    /// 
    /// In Gaddag the Letter 31 represents a pivot node.
    /// </summary>
    public byte Control;
    public byte Letter;
    public bool IsEnd { get { return (Control & DawgUtils.IsEnd) == DawgUtils.IsEnd; } }
    public void SetEnd() { Control |= DawgUtils.IsEnd; }

}
