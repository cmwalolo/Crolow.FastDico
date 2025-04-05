using Kalow.Apps.ApiTester;
using Kalow.Apps.Common.DataTypes;
using LuceneWordExtractor;

class Program
{
    static void Main(string[] args)
    {
        LiteDB.BsonMapper.Global.RegisterType<KalowId>
                (
                    (oid) => new LiteDB.ObjectId(oid.ToByteArray()),
                    (bson) => new KalowId(bson.AsObjectId.ToByteArray())
                );

        if (args.Length == 0)
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("-cw :  Clean Word Entries");
            Console.WriteLine("-l :  Extract Lucene Index");
            Console.WriteLine("-w :  Create Wiki DB");
            Console.WriteLine("-i :  Import All words");
            Console.WriteLine("-k {new} {onlyMultiple}:  Link words to DB {only unlined}");
            Console.WriteLine("-c {site} {new} {clean} :  Crawl {site} for undefined words");

            //args = new string[] { "-c", "Wikitionnaire", "new" };
            args = new string[] { "-k", "" };
            //args = new string[] { "-cw" };
            //args = new string[] { "-i" };
        }

        switch (args[0])
        {
            case "-cw":
                Console.WriteLine("Starting Lucene Extractor...");
                CleanWordEntries.Start();
                break;
            case "-c":
                new DictionaryCrawler().Start(args[1], (args.Length > 2 && args[2] == "new") ? true : false);
                break;
            case "-l":
                Console.WriteLine("Starting Lucene Extractor...");
                LuceneExtractor.Start();
                break;
            case "-w":
                WiktionaryToDb.Start();
                break;
            case "-i":
                ImportAllWords.Start();
                break;
            case "-k":
                LinkWordsToDb.Start((args.Length > 1 && args[1] == "new") ? true : false, (args.Length > 1 && args[1] == "onlyMultiple") ? true : false, (args.Length > 1 && args[1] == "clean") ? true : false);
                break;
        }

    }
}