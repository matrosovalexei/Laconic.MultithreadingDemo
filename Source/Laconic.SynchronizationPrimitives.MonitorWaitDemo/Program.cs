using System;
using System.Threading;

namespace Laconic.SynchronizationPrimitives.MonitorWaitDemo
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

            Thread.Sleep(10);
            Monitor.Enter(ConsoleLock);
            Monitor.PulseAll(ConsoleLock);
            Monitor.Exit(ConsoleLock);

            Console.ReadLine();
        }

        private static void ThreadMain(object o)
        {
            var id = (int) o;

            Monitor.Enter(ConsoleLock);

            for (var i = 0; i < Console.BufferWidth; i++)
            {
                Monitor.Wait(ConsoleLock);
                Console.SetCursorPosition(i, id);
                Console.Write((char)('A' + id));
                Thread.Sleep(200);
                Monitor.Pulse(ConsoleLock);
            }

            Monitor.Exit(ConsoleLock);
        }
    }
}
