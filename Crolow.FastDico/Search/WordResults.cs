namespace Crolow.FastDico.Search;

public class WordResults
{
    public List<Word> Words = new List<Word>();
    public class Word
    {
        public List<Tile> Tiles;
        public int Status;

        public Word()
        {
            Tiles = new List<Tile>();
        }
        public Word(List<Tile> tiles, int status = 0)
        {
            Tiles = tiles;
            Status = status;
        }

        public Word(Word copy)
        {
            Tiles = copy.Tiles.ToList();
        }
    }
    public class Tile
    {
        public byte Letter;
        public bool IsJoker;
        public int Status;

        public Tile(byte letter, bool isJoker, int status)
        {
            Letter = letter;
            IsJoker = isJoker;
            Status = status;
        }
    }
}


//public class Program
//{
//    public static void Main()
//    {

//    }
//}
