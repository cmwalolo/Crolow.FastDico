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
            Console.WriteLine("-l :  Extract Lucene Index");
            Console.WriteLine("-w :  Create Wiki DB");
            Console.WriteLine("-k :  Link words to DB");

            args = new string[] { "-w" };
        }

        switch (args[0])
        {
            case "-l":
                Console.WriteLine("Starting Lucene Extractor...");
                LuceneExtractor.Start();
                break;
            case "-w":
                WiktionaryToDb.Start();
                break;
        }

    }
}