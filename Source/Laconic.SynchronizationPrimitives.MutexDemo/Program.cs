using System;
using System.Threading;

namespace Laconic.SynchronizationPrimitives.MutexDemo
{
    public class Program
    {
        private const int ThreadCount = 8;

        private static readonly Mutex ConsoleMutex = new Mutex(true);

        public static void Main(string[] args)
        {
            for (var i = 0; i < ThreadCount; i++)
            {
                var thread = new Thread(ThreadMain);
                thread.Start(i);
            }

            ConsoleMutex.ReleaseMutex();

            Console.ReadLine();
        }

        private static void ThreadMain(object o)
        {
            var id = (int) o;

            for (var i = 0; i < Console.BufferWidth; i++)
            {
                ConsoleMutex.WaitOne();
                Console.SetCursorPosition(i, id);
                Console.Write((char) ('A' + id));
                Thread.Sleep(100);
                ConsoleMutex.ReleaseMutex();
            }
        }
    }
}
