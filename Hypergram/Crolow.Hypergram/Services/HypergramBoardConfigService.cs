using Kalow.Hypergram.Core.Solver.Utils;
using System.Text;

namespace Kalow.Hypergram.Core.Services;

public class HypergramBoardConfigService
{
    private int NbLetters = 0;
    private int[] Vowels, Consonants, Values, Amount;
    private char[] Ascii;
    private int dimX, dimY, dimRX, dimRY;
    private int[] LetterMultipliers, WordMultipliers;
    private System.Drawing.Color[] LetterColor, WordColor;

    public HypergramBoardConfigService()
    {
    }

    public HypergramBoardConfig Load(string Language)
    {
        string s;
        string[] v;

        if (!string.IsNullOrEmpty(Language)) Language = "FR";
        string path = "HYPERGRAM" + Language;

        string fn = "wwwroot\\Documents\\Topmachine\\Data6\\" + path + ".txt";
        byte[] b = File.ReadAllBytes(fn);

        using (MemoryStream ms = new MemoryStream(b))
        {
            ms.Position = 0;
            using (StreamReader sr = new StreamReader(ms))
            {
                return Load(Language, sr);
            }
        }
    }

    public HypergramBoardConfig Load(string Language, StringBuilder sb)
    {
        using (var reader = new StreamReader(
                    new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString()))))
        {
            return Load(Language, reader);
        }
    }
    public HypergramBoardConfig Load(string Language, StreamReader sr)
    {
        string[] v;
        string s;

        sr.ReadLine();

        // Letter Colors
        LetterColor = new System.Drawing.Color[4];
        s = sr.ReadLine();
        v = s.Split(new char[] { ',' });
        for (int x = 0; x < 4; x++)
        {
            LetterColor[x] = GetColor(v[x]);
        }
        sr.ReadLine();

        // Word Colors
        WordColor = new System.Drawing.Color[4];
        s = sr.ReadLine();
        v = s.Split(new char[] { ',' });
        for (int x = 0; x < 4; x++)
        {
            WordColor[x] = GetColor(v[x]);
        }
        sr.ReadLine();

        // Word Colors
        s = sr.ReadLine();
        NbLetters = int.Parse(s);
        sr.ReadLine();

        // Vowels
        Vowels = new int[NbLetters];
        s = sr.ReadLine();
        v = s.Split(new char[] { ',' });
        for (int x = 0; x < NbLetters; x++)
        {
            Vowels[x] = int.Parse(v[x]);
        }
        sr.ReadLine();

        // Consonants
        Consonants = new int[NbLetters];
        s = sr.ReadLine();
        v = s.Split(new char[] { ',' });
        for (int x = 0; x < NbLetters; x++)
        {
            Consonants[x] = int.Parse(v[x]);
        }
        sr.ReadLine();

        // Letter Total
        Amount = new int[NbLetters];
        s = sr.ReadLine();
        v = s.Split(new char[] { ',' });
        for (int x = 0; x < NbLetters; x++)
        {
            Amount[x] = int.Parse(v[x]);
        }
        sr.ReadLine();

        // Letter Values
        Values = new int[NbLetters];
        s = sr.ReadLine();
        v = s.Split(new char[] { ',' });
        for (int x = 0; x < NbLetters; x++)
        {
            Values[x] = int.Parse(v[x]);
        }
        s = sr.ReadLine();

        // ASCII Values
        Ascii = new char[NbLetters];
        s = sr.ReadLine();
        v = s.Split(new char[] { ',' });
        for (int x = 0; x < NbLetters; x++)
        {
            Ascii[x] = v[x][0];
        }
        //sr.ReadLine();
        HypergramBoardConfig cm = new HypergramBoardConfig();
        cm.SetTiles(NbLetters, Vowels, Consonants, Amount, Values, Ascii);
        return cm;
    }


    private char GetChar(int x)
    {
        return (char)Ascii[x];
    }

    private int GetCharFromAscii(int x)
    {
        for (int xx = 0; xx < NbLetters; xx++)
        {
            if (Ascii[xx] == x) return (char)xx;
        }

        return -1;
    }

    private System.Drawing.Color GetColor(int row, int col)
    {
        int x = GetLetterMultiplier(row, col);
        if (x > 1)
        {
            return LetterColor[x - 1];
        }

        x = GetWordMultiplier(row, col);
        if (x > 1)
        {
            return WordColor[x - 1];
        }

        return WordColor[0];
    }

    private int GetLetterMultiplier(int row, int col)
    {
        return LetterMultipliers[row * dimRX + col];
    }

    private int GetWordMultiplier(int row, int col)
    {
        return WordMultipliers[row * dimRX + col];
    }

    private System.Drawing.Color GetColor(string col)
    {
        System.Drawing.Color coll = System.Drawing.Color.FromName(col);

        if (coll != System.Drawing.Color.Empty) return coll;

        int[] cols = new int[3];

        cols[0] = int.Parse(col.Substring(0, 3));
        cols[1] = int.Parse(col.Substring(3, 3));
        cols[2] = int.Parse(col.Substring(6, 3));

        return System.Drawing.Color.FromArgb(cols[0], cols[1], cols[2]);
    }
}