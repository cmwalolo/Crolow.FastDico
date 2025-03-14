using Crolow.FastDico.Utils;

namespace Crolow.FastDico.Dicos;

public class LetterNode
{
    /// <summary>
    /// In Gaddag the Letter 31 represents a pivot node.
    /// </summary>
    public byte Control;
    public byte Letter;
    public bool IsEnd { get { return (Control & DawgUtils.IsEnd) == DawgUtils.IsEnd; } }
    public bool IsPivot { get { return Letter == DawgUtils.PivotByte; } }

    public void SetEnd() { Control |= DawgUtils.IsEnd; }

    public List<LetterNode> Children = new List<LetterNode>();
}
