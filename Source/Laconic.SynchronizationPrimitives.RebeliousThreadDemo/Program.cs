using System;
using System.Threading;

namespace Laconic.SynchronizationPrimitives.RebeliousThreadDemo
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
                if (id != 0)
                {
                    ConsoleMutex.WaitOne();
                }
                Console.SetCursorPosition(i, id);
                Console.Write((char) ('A' + id));
                Thread.Sleep(200);
                if (id != 0)
                {
                    ConsoleMutex.ReleaseMutex();
                }
            }
        }
    }
}
