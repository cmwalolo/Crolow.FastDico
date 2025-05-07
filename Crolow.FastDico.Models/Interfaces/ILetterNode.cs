namespace Crolow.FastDico.Dicos
{
    public interface ILetterNode
    {
        public List<ILetterNode> Children { get; set; }
        byte Control { get; set; }
        byte Letter { get; set; }
        bool IsEnd { get; }
        bool IsPivot { get; }

        void SetEnd();
    }
}