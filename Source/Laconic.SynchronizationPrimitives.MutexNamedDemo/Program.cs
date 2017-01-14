using System;
using System.Threading;

namespace Laconic.SynchronizationPrimitives.MutexNamedDemo
{
    public class Program
    {
        private const int ThreadCount = 8;

        private static readonly bool IsNewMutex;
        private static readonly Mutex ConsoleMutex;

        static Program()
        {
           ConsoleMutex = new Mutex(false, typeof(Program).FullName + typeof(Mutex).FullName, out IsNewMutex);
        }

        public static void Main(string[] args)
        {
            Console.WriteLine($"{nameof(IsNewMutex)} = {IsNewMutex}");

            ConsoleMutex.WaitOne();
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
                try
                {
                    ConsoleMutex.WaitOne();
                    Console.SetCursorPosition(i, id + 2);
                    Console.Write((char) ('A' + id));
                    Thread.Sleep(200);
                    ConsoleMutex.ReleaseMutex();
                }
                catch (AbandonedMutexException)
                {
                    ConsoleMutex.ReleaseMutex();
                }
            }
        }
    }
}
