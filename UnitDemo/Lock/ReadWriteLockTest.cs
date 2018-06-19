using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitDemo.Lock
{
    public class ReadWriteLockTest
    {
        private static ReaderWriterLock rw = new ReaderWriterLock();

        private static List<int> items = new List<int>();

        public static void Read()
        {

            while (true)
            {
                Console.WriteLine(Thread.CurrentContext.ContextID);
                try
                {

                    rw.AcquireReaderLock(60000);

                    if (items.Count > 0)
                    {
                        //int result = items[items.Count - 1];

                        Console.WriteLine("----" + items.Count + "----");
                    }
                }
                finally
                {
                    rw.ReleaseReaderLock();
                }
            }

        }

        static int _writeCount = 0;
        public static void Write()
        {

            while (true)
            {
                // Console.WriteLine(Thread.CurrentContext.ContextID);


                rw.AcquireWriterLock(60000);



                try
                {
                    items.Add(_writeCount++);
                }
                finally
                {
                    rw.ReleaseWriterLock();
                }
            }
        }

        public static void Excute()
        {
            new Thread(delegate()
            {

                Read();
            }).Start();

            new Thread(delegate()
            {

                Read();
            }).Start();

            new Thread(delegate()
            {

                Read();
            }).Start();

            new Thread(delegate()
            {

                Write();
            }).Start();
        }
    }


    public class ReadWriteLockSlimTest
    {
        private static ReaderWriterLockSlim rw = new ReaderWriterLockSlim();

        static List<int> items = new List<int>();

        public static void Read()
        {
            while (true)
            {
                rw.TryEnterReadLock(1000);

                try
                {
                    Console.WriteLine("----Sum Count:" + items.Count + "-------");
                }

                finally
                {
                    rw.ExitReadLock();
                }
            }
        }

        public static void Write()
        {
            while (true)
            {

                try
                {
                    rw.TryEnterWriteLock(1000);

                    items.Add(new Random().Next(1000));
                }
                finally
                {
                    rw.ExitWriteLock();
                }

            }
        }

        public static void ReadThreadCount()
        {
            while (true)
            {
                Console.WriteLine("Read Count" + rw.CurrentReadCount);
                Console.WriteLine("Watiing Read Count" + rw.WaitingReadCount);
            }

        }

        public static void Excute()
        {
            new Thread(delegate()
            {
                Read();
            }).Start();

            new Thread(delegate()
            {
                Read();
            }).Start();

            new Thread(delegate()
            {
                Read();
            }).Start();

            new Thread(delegate()
            {
                Write();
            }).Start();

            //new Thread(delegate()
            //{
            //    ReadThreadCount();
            //}).Start();
        }
    }
}
