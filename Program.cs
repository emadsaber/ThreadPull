using System.Collections.Concurrent;
using System.Text;

namespace ThreadPull
{
    internal class Program
    {
        static string[] delimiters = new string[] { "-", "=", ".", "$", "%", "^", "&", "*", "÷", "×", ":", "~", "’", "؟", "+", ".", ";", "\"", "@", "#" };
        static ConcurrentDictionary<int, StringBuilder> threadOutputs = new ConcurrentDictionary<int, StringBuilder>();
        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("Press n to start a thread!");
            while (true)
            {
                if (Console.ReadKey(intercept: true).Key == ConsoleKey.N)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), DateTime.Now);

            }
        }

        static void DoWork(object state)
        {
            var threadNo = threadOutputs.Count + 1; // +1 for the first line (Press n to start a thread!)
            var rndDelimiter = new Random().Next(0, delimiters.Length);
            var rndSleep = new Random().Next(20, 3000);
            var delimiter = delimiters[rndDelimiter];

            var output = threadOutputs.GetOrAdd(threadNo, new StringBuilder($"Thread {threadNo,2}:"));
            while (true)
            {
                Console.CursorVisible = false;
                lock (Console.Out)
                {
                    if (output.Length > 100+10) //10 for the header "Thread xx:"
                    {
                        output.Clear(); //to fix spanning on multiple line
                        output.Append($"Thread {threadNo,2}: Completed!");
                        Console.SetCursorPosition(0, threadNo);
                        Console.Write(output.ToString());
                    }
                    output.Append(delimiter);
                    Console.SetCursorPosition(0, threadNo);
                    Console.Write(output.ToString());
                }
                Thread.Sleep(rndSleep);
            }
        }
    }
}
