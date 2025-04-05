using System.Diagnostics;

namespace Crolow.FastDico.ScrabbleApi.Utils
{
    public class StopWatcher : IDisposable
    {
        private Stopwatch stopwatch = new Stopwatch();
        public string Message { get; set; }
        public StopWatcher(string message)
        {
            this.Message = message;
            if (message != null)
            {
                Console.WriteLine("Starting : " + message);
                Console.WriteLine("-------------------------------------------");
            }
            stopwatch.Start();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            if (Message != null)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Stopping : " + Message + " : " + stopwatch.ElapsedMilliseconds + " ms");
            }
        }
    }
}
