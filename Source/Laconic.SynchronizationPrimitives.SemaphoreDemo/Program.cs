using System;
using System.Threading;

namespace Laconic.SynchronizationPrimitives.SemaphoreDemo
{
    public class Program
    {
        private const int ThreadCount = 8;
        private const int SemaphoreCount = 3;

        private static readonly Semaphore ConsoleSemaphore = new Semaphore(0, SemaphoreCount);

        public static void Main(string[] args)
        {
            for (var i = 0; i < ThreadCount; i++)
            {
                var thread = new Thread(ThreadMain);
                thread.Start(i);
            }

            ConsoleSemaphore.Release(SemaphoreCount);

            Console.ReadLine();
        }

        private static void ThreadMain(object o)
        {
            var id = (int) o;

            for (var i = 0; i < Console.BufferWidth; i++)
            {
                ConsoleSemaphore.WaitOne();
                SyncConsole.Write(i, id, (char) ('A' + id));
                Thread.Sleep(600);
                ConsoleSemaphore.Release();
            }
        }
    }

    public static class SyncConsole
    {
        private static readonly Mutex Mutex = new Mutex();

        public static void Write(int left, int top, char c)
        {
            Mutex.WaitOne();
            Console.SetCursorPosition(left, top);
            Console.Write(c);
            Mutex.ReleaseMutex();
        }
    }
}
