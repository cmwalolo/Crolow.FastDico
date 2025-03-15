using System.Diagnostics;

namespace Crolow.FastDico.ScrabbleApi.Utils
{
    public class StopWatcher : IDisposable
    {
        private Stopwatch stopwatch = new Stopwatch();
        private string message;
        public StopWatcher(string message)
        {
            this.message = message;
            Console.WriteLine("Starting : " + message);
            Console.WriteLine("-------------------------------------------");
            stopwatch.Start();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Stopping : " + message + " : " + stopwatch.ElapsedMilliseconds);
        }
    }
}
