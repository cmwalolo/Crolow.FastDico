using Crolow.Fast.Dawg.Utils;

namespace Crolow.Fast.Dawg.Dicos;

public class BaseNode
{
    public byte Control;
    public byte Letter;
    public bool IsEnd { get { return (Control & DawgUtils.IsEnd) == DawgUtils.IsEnd; } }
    public void SetEnd() { Control |= DawgUtils.IsEnd; }

    public bool IsPivot { get { return (Control & DawgUtils.IsPivot) == DawgUtils.IsPivot; } }
    public bool IsStart { get { return (Control & DawgUtils.IsStart) == DawgUtils.IsStart; } }
    public void SetPivot() { Control |= DawgUtils.IsPivot; }
    public void SetStart() { Control |= DawgUtils.IsStart; }
}
