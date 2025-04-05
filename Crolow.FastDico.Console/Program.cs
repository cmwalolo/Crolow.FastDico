using Crolow.FastDico.Console;
using Crolow.FastDico.Dawg;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.Interfaces;
using Crolow.FastDico.ScrabbleApi.Utils;
using Crolow.FastDico.Utils;


TilesUtils.configuration = ConfigReader.ReadLetterConfig("FR");

var tester = new Tester();
tester.TestDawg(false);
//tester.TestGadDag(false);

//var ScrabbleAI = new ScrabbleAI("GridConfigs_FR.Json", "FR Normal");
//ScrabbleAI.StartGame();
public class Tester
{
    public Tester()
    {

    }
    public void TestDawg(bool compile)
    {

        DawgCompiler dawg = new DawgCompiler();
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
            string.Join("\n", newDawg.SearchBySuffix("forage")));  // 2 (bat, batman)


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

        GadDagCompiler gaddag = new GadDagCompiler();
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
            string.Join("\n", newDawg.SearchBySuffix("forage")));  // 2 (bat, batman)
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

