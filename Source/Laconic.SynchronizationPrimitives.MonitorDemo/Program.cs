using System;
using System.Threading;

namespace Laconic.SynchronizationPrimitives.MonitorDemo
{
    public class Program
    {
        private const int ThreadCount = 8;

        private static readonly object ConsoleLock = new object();

        public static void Main(string[] args)
        {
            for (var i = 0; i < ThreadCount; i++)
            {
                var thread = new Thread(ThreadMain);
                thread.Start(i);
            }

            Console.ReadLine();
        }

        private static void ThreadMain(object o)
        {
            var id = (int) o;

            for (var i = 0; i < Console.BufferWidth; i++)
            {
                var lockWasTaken = false;
                var temp = ConsoleLock;
                try
                {
                    Monitor.Enter(temp, ref lockWasTaken);

                    Console.SetCursorPosition(i, id);
                    Console.Write((char) ('A' + id));
                    Thread.Sleep(200);
                }
                finally
                {
                    if (lockWasTaken)
                    {
                        Monitor.Exit(temp);
                    }
                }
            }
        }
    }
}
