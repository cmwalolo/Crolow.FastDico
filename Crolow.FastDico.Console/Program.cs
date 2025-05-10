using Crolow.FastDico.Common.Interfaces.Dictionaries;
using Crolow.FastDico.Common.Models.Common;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.Console;
using Crolow.FastDico.Dawg;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.ScrabbleApi.Extensions;
using Crolow.FastDico.ScrabbleApi.Factories;
using Crolow.FastDico.ScrabbleApi.Utils;
using Crolow.FastDico.Utils;
using Crolow.TopMachine.Core.Json;
using Kalow.Apps.Common.JsonConverters;
using Newtonsoft.Json;
using System.Text;

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Converters = { new KalowIdConverter() },
    ContractResolver = new CustomContractResolver()
};

var container = new ConfigReader().LoadConfig("FR 7/7 Difficile");
// We set the current Language to the selected config
TilesUtils.configuration = ConfigLoader.ReadLetterConfig(container.LetterConfig);

var tester = new Tester();
//tester.TestDawg(false);
//tester.TestGadDag(false);

GadDagDictionary gaddag = new GadDagDictionary();
gaddag.ReadFromFile("gaddag_fr.gz");

var factory = new ToppingFactory();
var game = factory.CreateGame(container, gaddag);

ApplicationContext.CurrentGame = game;

using (StopWatcher stopwatch = new StopWatcher("Game started"))
{
    game.ControllersSetup.ScrabbleEngine.StartGame();
}
PrintGame.PrintGrid(game);
Console.ReadLine();

public class PrintGame
{
    public static void PrintGrid(CurrentGame game)
    {
#if DEBUG
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<link rel=\"stylesheet\" href=\"grid.css\" />");
        sb.AppendLine("</head>");

        sb.AppendLine("<body><div id='grid'>");

        sb.AppendLine("<table class='results' style='float:right'>");
        sb.AppendLine("<tr><th>#</th><th>Rack</th><th>Word</th><th>pos</th><th>pts</th></tr>");
        int ndx = 1;
        foreach (var r in game.GameObjects.RoundsPlayed)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{ndx++}</td>");
            sb.AppendLine($"<td>{r.Rack.GetString()}</td>");
            sb.AppendLine($"<td>{r.GetWord(true)}</td>");
            sb.AppendLine($"<td>{r.GetPosition()}</td>");
            sb.AppendLine($"<td>{r.Points}</td>");
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</table>");

        sb.AppendLine("<table class='board'>");
        sb.AppendLine("<tr><td></td>");
        for (int col = 1; col < game.GameObjects.Board.CurrentBoard[0].SizeH - 1; col++)
        {
            sb.AppendLine($"<td class='border'>{col}</td>");
        }
        sb.AppendLine("</tr>");

        for (int x = 1; x < game.GameObjects.Board.CurrentBoard[0].SizeH - 1; x++)
        {
            var cc = ((char)(x + 64));
            sb.AppendLine($"<tr><td class='border'>{cc}</td>");
            for (int y = 1; y < game.GameObjects.Board.CurrentBoard[0].SizeH - 1; y++)
            {
                var sq = game.GameObjects.Board.GetSquare(0, y, x);
                var cclass = $"cell cell-{sq.LetterMultiplier} cell{sq.WordMultiplier}";

                if (sq.Status == 1)
                {
                    cclass += sq.CurrentLetter.IsJoker ? " tileJoker" : " tile";
                }

                sb.AppendLine($"<td class='{cclass}'>");
                if (sq.Status == 1)
                {
                    var c = (char)(sq.CurrentLetter.Letter + 97);
                    sb.AppendLine(char.ToUpper(c).ToString());
                }
                sb.AppendLine("</td>");
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");

        sb.AppendLine("</grid></body></html>");
        System.IO.File.WriteAllText("output.html", sb.ToString());
#endif
        // We are done 
    }
}

public class Tester
{
    public Tester()
    {

    }
    public void TestDawg(bool compile)
    {

        DawgDictionary dawg = new DawgDictionary();
        if (compile)
        {
            List<string> words = System.IO.File.ReadAllLines("C:\\dev\\ODS9-complet.txt").Select(p => p.ToLower()).ToList();
            dawg.Build(words);
            dawg.SaveToFile("dawg_data.gz");
            Console.WriteLine($"DAWG saved to dawg_data.gz");
        }

        dawg.ReadFromFile("dawg_data.gz");
        Console.WriteLine("DAWG loaded from file");


        IDawgSearch newDawg = new DawgSearch(dawg.Root);
        // Test searching
        Console.WriteLine(newDawg.SearchWord("sufa"));   // False
        Console.WriteLine(newDawg.SearchWord("chien"));   // True
        Console.WriteLine(newDawg.SearchWord("dormir"));   // False
        Console.WriteLine(newDawg.SearchWord("fechier"));   // False

        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("chi?ent")));   // False
        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("*baties")));   // False
        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("*forage")));   // False
        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("*urb*forage*")));   // False

        Console.WriteLine(
            string.Join("\n", newDawg.SearchByPrefix("batissa")));  // 2 (bat, batman)

        Console.WriteLine(
            string.Join("\n", newDawg.SearchBySuffix("forage", 3)));  // 2 (bat, batman)


        Console.WriteLine("\nAnagrams");
        Console.WriteLine("--------------");
        Console.WriteLine(" artesien ? " + newDawg.SearchWord("artesien"));
        Console.WriteLine(string.Join("\n", newDawg.FindAllWordsFromLetters("arteisn")));   // False
        Console.WriteLine(string.Join("\n", newDawg.FindAllWordsFromLetters("afusbi")));
        Console.WriteLine("\nSmaller words");
        Console.WriteLine("--------------");
        Console.WriteLine(string.Join("\n", newDawg.FindAllWordsContainingLetters("piboutre")));   // False


        Console.WriteLine("\n# of words:" + newDawg.SearchByPattern("*").Count);

    }

    public void TestGadDag(bool compile)
    {
        // GADDAG 

        GadDagDictionary gaddag = new GadDagDictionary();
        if (compile)
        {
            List<string> words = System.IO.File.ReadAllLines("C:\\dev\\ODS9-complet.txt").Select(p => p.ToLower()).ToList();
            gaddag.Build(words);
            gaddag.SaveToFile("gaddag_fr.gz");
            Console.WriteLine($"GADDAG saved to GADDAG_data.gz");
        }

        gaddag.ReadFromFile("gaddag_fr.gz");
        Console.WriteLine("DAWG loaded from file");
        Console.WriteLine("--------------");

        IDawgSearch newDawg = new GadDagSearch(gaddag.Root);
        // Test searching
        Console.WriteLine("\nWord Search");
        Console.WriteLine("--------------");

        Console.WriteLine(newDawg.SearchWord("tamerz"));   // False
        Console.WriteLine(newDawg.SearchWord("chien"));   // True
        Console.WriteLine(newDawg.SearchWord("dormir"));   // False
        Console.WriteLine(newDawg.SearchWord("fechier"));   // False

        Console.WriteLine("\nPattern Search");
        Console.WriteLine("--------------");

        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("?chi?ent")));   // False
        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("*baties")));   // False
        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("*forage")));   // False
        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("*urb?forage*")));   // False
        Console.WriteLine(string.Join("\n", newDawg.SearchByPattern("?gourmand?")));   // False

        Console.WriteLine("\nPrefix Search");
        Console.WriteLine("--------------");

        Console.WriteLine(
            string.Join("\n", newDawg.SearchByPrefix("forage")));  // 2 (bat, batman)

        Console.WriteLine("\nSuffix Search");
        Console.WriteLine("--------------");
        Console.WriteLine(
            string.Join("\n", newDawg.SearchBySuffix("forage", 3)));  // 2 (bat, batman)
        Console.WriteLine(
            string.Join("\n", newDawg.SearchBySuffix("mplacais")));  // 2 (bat, batman)
        Console.WriteLine(
            string.Join("\n", newDawg.SearchBySuffix("placais")));  // 2 (bat, batman)


        Console.WriteLine("\nAnagrams");
        Console.WriteLine("--------------");
        Console.WriteLine(" artesien ? " + newDawg.SearchWord("artesien"));
        Console.WriteLine(string.Join("\n", newDawg.FindAllWordsFromLetters("swapez?")));   // False

        Console.WriteLine("\nSmaller words");
        Console.WriteLine("--------------");
        Console.WriteLine(string.Join("\n", newDawg.FindAllWordsContainingLetters("cuba")));   // False


        Console.WriteLine("# of words:" + newDawg.SearchByPattern("*").Count);
    }

}

