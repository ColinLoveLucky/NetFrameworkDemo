using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;

namespace UnitDemo.ThreadDemo
{
    //System.Threading.Timer
    //System.Timers.Timer
    ///BackgroundWorker
    ///WebClient
    /// <summary>
    /// 线程的同步上线文
    /// 如何处理被锁，什么情况下会出现锁
    /// 线程异常的处理
    /// CPU单个最大的线程数
    /// 线程池可以开放的线程数
    /// 为什么可以并行
    /// 幽灵线程
    /// 守护线程
    /// 线程池
    /// 多线程
    /// 同步线程
    /// ThreadState
    /// ThreadPriority
    /// WaitHandle
    ///  AutoResetEvent 
    ///  ManualResetEvent
    ///  ExecutionContext
    ///  Interlocked
    ///  Monitor
    ///  Mutex
    ///  CountdownEvent
    ///  AutoResetEvent
    ///  Semaphore
    ///  EventWaitHandle   
    ///  Thread
    ///  ThreadAbortException
    ///  ThreadPool
    ///  Timeout
    ///  Timer
    ///  ReaderWriterLock
    ///  ReaderWriterLockSlim
    ///  CLR 线程池
    ///  System.Threading.Tasks.Parallel
    ///  TPL
    /// </summary>
    public class ThreadT
    {
        public void ThreadShow()
        {
			Thread t = new Thread(ThreadAbort);
            t.Name = "八戒";
            t.Start();
            Thread.Sleep(5000);
            Console.WriteLine(t.ThreadState);
            Console.WriteLine("悟空:八戒，该起床了");
            t.Abort();
            //  ThreadState
        }
        public void ThreadAbort()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine(Thread.CurrentThread.Name + ":呼呼~~~~~");
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine(Thread.CurrentThread.Name + ":还早呢，我还要再睡会");
                Thread.ResetAbort();
            }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(Thread.CurrentThread.Name + ":开启新的呼呼~~~~~");
                Thread.Sleep(1000);
            }
        }
        public void ThreadInterrupt()
        {
            Console.WriteLine("------------Interrupt方法执行情况---------------");
            Thread t1 = new Thread(Interrupt);
            t1.Start();
            Thread.Sleep(1000);
            t1.Interrupt();
            t1.Join();

            Console.WriteLine("------------Abort方法执行情况---------------");
            Thread t2 = new Thread(Interrupt);
            t2.Start();
            Thread.Sleep(1000);
            t2.Abort();

            // IAsyncResult

        }
        public void Interrupt()
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Console.WriteLine("第" + i + "循环。");
                    Thread.Sleep(500);
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine("第" + i + "循环中，线程被中断，下次循环线程将继续运行。");
                }
                catch (ThreadAbortException e)
                {
                    Console.WriteLine("第" + i + "循环中，线程被终止，线程将不再继续运行");
                    Thread.ResetAbort();
                }
            }
        }
        public void ThreadAttr()
        {
            var thread = Thread.CurrentThread;
            var context = Thread.CurrentContext;
        }
    }
    public class MultiplyThread
    {
        public void ThreadReport()
        {
            Thread tLastMonth = new Thread(CalcuLastMonth);
            tLastMonth.Start();
            tLastMonth.Priority = ThreadPriority.Lowest;
            Thread tCompany = new Thread(CalcuCompany);
            tCompany.Start();
            tCompany.Priority = ThreadPriority.Highest;
            tLastMonth.Join();
            tCompany.Join();
            Console.WriteLine("顺利统计出数据");
        }
        public void DelegateReport()
        {

            //Action<object> delLastMonth = CalcuLastMonth;
            //var lastResult = delLastMonth.BeginInvoke(null, null, "上个月数据");
            //while (true)
            //{
            //    if (lastResult.IsCompleted)
            //    {
            //        Console.WriteLine(lastResult.AsyncState + "统计完成");
            //        break;
            //    }

            //}
            //Action<object> delCompany = CalcuCompany;
            //var comResult = delCompany.BeginInvoke(null, null, "夸客全年数据");
            //while (true)
            //{
            //    if (comResult.IsCompleted)
            //    {
            //        Console.WriteLine(comResult.AsyncState + "统计完成");
            //        break;
            //    }

            //}
            //Console.WriteLine("顺利统计出数据");

            //Action<object> delLastMonth = CalcuLastMonth;
            //var lastResult = delLastMonth.BeginInvoke(null, null, "上个月数据");
            //lastResult.AsyncWaitHandle.WaitOne();
            //Console.WriteLine(lastResult.AsyncState + "统计完成");
            //Action<object> delCompany = CalcuCompany;
            //var comResult = delCompany.BeginInvoke(null, null, "夸客全年数据");
            //comResult.AsyncWaitHandle.WaitOne();
            //Console.WriteLine(comResult.AsyncState + "统计完成");
            //Console.WriteLine("顺利统计出数据");

            //Action<object> delLastMonth = CalcuLastMonth;
            //var lastResult = delLastMonth.BeginInvoke(null, null, "上个月数据");
            //delLastMonth.EndInvoke(lastResult);
            //Console.WriteLine(lastResult.AsyncState + "统计完成");
            //Action<object> delCompany = CalcuCompany;
            //var comResult = delCompany.BeginInvoke(null, null, "夸客全年数据");
            //delCompany.EndInvoke(comResult);
            //Console.WriteLine(comResult.AsyncState + "统计完成");
            //Console.WriteLine("顺利统计出数据");


            ///委托与异步的关系
            ///异步是什么
            ///异步肯定有执行的方法，异步回调的方法，最终得到异步回调的结果
            ///异步有什么好处
            ///AsyncCallBack
            ///为什么这么设计这个异步的接口
            ///为什么设计AsyncCallBack这个实现类
            ///AsyncCallback 作为一个委托存在了
            ///开始的时候执行方法，当结束的时候自动通知，然后调用回调方法，
            ///异步的方法应该是用新的线程来执行不会影响主线程的情况下
            ///调用方法
            ///为什么会调用End方法的目的何在
            ///End方法做为返回结果的一个方法
            ///这个就是异步的实现思路
            ///start=》excute=》callback=》end
            ///应该是callback方法得到执行的结果吧
            ///callback可以得到执行的回调结果吧
            ///但是必须是状态被回执成功了之后才可以在callback里面顺利解释出执行的回调结果
            ///所以callback 被设计成为一个委托传入的参数即使IAsyncCallback接口对象
            ///所以才可以根据状态，得到最终的结果吧
            ///所以说是在执行完之后才执行的回调结果
           ///callback 是否应该设计在finally里面也是值得考究的
           ///如果callback 最终设计在finally里面那就是在执行完实际方法之后才会去执行callback的数据，
           ///那有必要在传入IAsyncResult对象的必要吗
           ///在callback里面也就没有必要在针对完成状态在此做2此判断了，
           ///如若是这种，那应该如何去设计这种callback来执行的顺序与实际的执行方法呢
           ///如果按照里面用线程池来实现实际的调用方法，是可以实现多线程调用的目的，然而callback确作为一种线性来被
           ///执行，是否违背了这种异步的思想呢
           ///异步应该是调用及执行，然后在去执行回调方法，或者不执行，如果有回调方法，回调方法里面应该去对状态做判断的
           ///只有状态成功的才可以取得回调的结果哈
           ///应该如何去取得回调的结果呢，这个应该如何去实现呢，是不是感觉到有点奇怪，
           ///所以还是要设计一个end的方法来获取回调的结果的，
           ///end里面用到的是线程阻塞，因为是在主线程上面执行的方法，不可以决定是主线程与子线程那个快，所以要在end连首先去判断状态是否成功
           ///如果状态没有改变，所以就阻塞主线程，等待，执行完成，然后在beigin连释放阻塞
           ///然后end里面在顺利执行取得结果
           ///所以callback是不需要线程阻塞的，所以是要在执行完成之后就被调用的，其所以要传入当前的this来作为参数，以便于可以取得结果哈
           ///这中间设计到callback的属性问题，前段有成功执行的callback，失败执行的callback，后台只有一个callback
           ///所以说这中间是不是缺少了什么，后台用异常来处理，看到人家写的代码，定义异常方法在，主方法End里面抛出异常
           ///试想一下如果没有调用end方法，异常是不是就被淹没了，根本不知道出错哈
           ///在想如果把异常放在begin里面，因为begin里面是用线程来实现的，子线程里面的异常是会被淹没的，不会抛出的
           ///那应该怎么处理呢，怎么处理这部分内容呢
           ///是不是一个该思考的维度呢，
            Func<int, int> funCalc = x => x;
            var asyncResult = funCalc.BeginInvoke(10, AsyncCallBack, funCalc);
            Console.ReadLine();

        }
        public void AsyncCallBack(IAsyncResult asyncResult)
        {
            Func<int, int> del = (Func<int, int>)asyncResult.AsyncState;
            var result = del.EndInvoke(asyncResult);
            Console.WriteLine("The Last output result is {0}", result);
        }
        public void ReturnCallBack(IAsyncResult asyncResult)
        {
            if (asyncResult.IsCompleted == false)
            {
                asyncResult.AsyncWaitHandle.WaitOne();
                asyncResult.AsyncWaitHandle.Close();
            }
            Console.WriteLine(asyncResult.AsyncState + "统计完成");
        }
        public void ThreadPoolReport()
        {
            // Mutex
            // Semaphore
            // EventWaitHandle
            //通过内核对象来通信
            AutoResetEvent auto = new AutoResetEvent(false);
            ManualResetEvent manu = new ManualResetEvent(false);
            EventWaitHandle a = new EventWaitHandle(false, EventResetMode.AutoReset);
            WaitHandle[] watiHandlers = new WaitHandle[]{
                 new AutoResetEvent(false),
                 new AutoResetEvent(false)
             };
            ThreadPool.QueueUserWorkItem(CalcuLastMonth, watiHandlers[0]);
            ThreadPool.QueueUserWorkItem(CalcuCompany, watiHandlers[1]);
            int maxThread = 0;
            int maxIO = 0;
            ThreadPool.GetMaxThreads(out maxThread, out maxIO);
            WaitHandle.WaitAll(watiHandlers);
            Console.WriteLine("顺利统计出数据");
        }
        public void TaskReport()
        {
            var taskLastMonth = Task.Run(() =>
             {
                 for (var i = 0; i < 10; i++)
                     Console.WriteLine(string.Format("正在计算上个月数据 第 {0} 条计算成功", i));
             });
            var taskCompany = Task.Run(() =>
            {
                for (var i = 0; i < 10; i++)
                    Console.WriteLine(string.Format("正在计算公司全年的营业额 第 {0} 条计算成功", i));
            });
            Task.WaitAll(taskLastMonth, taskCompany);
            Console.WriteLine("顺利统计出数据");
        }
        public void TaskFactoryReport()
        {
            var taskLastMonth = Task.Factory.StartNew(CalcuLastMonth, "上个月数据");
            var taskCompany = Task.Factory.StartNew(CalcuCompany, "夸客金融全年数据");
            var taskContiune = Task.Factory.ContinueWhenAll(new Task[] { taskLastMonth, taskCompany }, tasks =>
             {
                 foreach (Task item in tasks)
                 {
                     Console.WriteLine(item.AsyncState + "统计完成");
                 }
             });
            taskContiune.Wait();
        }
        public async void AsyncAwaitReport()
        {
            var lastMonthResult = AsyncLastMonth();
            var companyResult = AsyncCompany();
            var month = await lastMonthResult;
            Console.WriteLine(month);
            var company = await companyResult;
            Console.WriteLine(company);
        }
        public async Task<string> AsyncLastMonth()
        {
            for (var i = 0; i < 10; i++)
                Console.WriteLine(string.Format("正在计算上个月数据 第 {0} 条计算成功", i));
            return await Task.Run(() =>
               {
                   return "上个月数据统计完成";
               });
        }
        public async Task<string> AsyncCompany()
        {
            for (var i = 0; i < 10; i++)
                Console.WriteLine(string.Format("正在计算公司全年的营业额 第 {0} 条计算成功", i));
            return await Task.Run(() =>
             {
                 return "公司全年的营业额数据统计完成";
             });
        }
        public void ParalleReport()
        {
            Parallel.For(0, 10000, (x) =>
            {
                Console.WriteLine(string.Format("正在计算上个月数据 第 {0} 条计算成功", x));
            });

            Parallel.For(0, 10000, (x) =>
            {
                Console.WriteLine(string.Format("正在计算公司全年的营业额 第 {0} 条计算成功", x));
            });
            Console.WriteLine("顺利统计出数据");
        }
        public void CalcuLastMonth(object state)
        {

            for (var i = 0; i < 10; i++)
                Console.WriteLine(string.Format("正在计算上个月数据 第 {0} 条计算成功", i));
            AutoResetEvent reset = state as AutoResetEvent;
            if (reset != null)
                reset.Set();

        }
        public void CalcuCompany(object state)
        {
            for (var i = 0; i < 10; i++)
                Console.WriteLine(string.Format("正在计算公司全年的营业额 第 {0} 条计算成功", i));
            AutoResetEvent reset = state as AutoResetEvent;
            if (reset != null)
                reset.Set();
        }
    }
    public class SynThread
    {
        private static Mutex mut = new Mutex(false);
        private static int _sum = 0;
        private static object _locker = new object();
        public void LockThread()
        {
            Func<int> f = LockMethod;
            var task1 = Task.Run<int>(f);
            var task2 = Task.Run<int>(f);
            int sum = task1.Result;
            int sum2 = task2.Result;
            Console.WriteLine(sum2);
        }
        public int LockMethod()
        {
            lock (_locker)
            {
                for (int i = 0; i < 100000; i++)
                {
                    _sum += i;
                }
                return _sum;
            }
        }
        public void MonitorThread()
        {
            //从不同步的代码块中调用了对象同步方法
            //Monitor.PulseAll(_locker);
            //Monitor.Wait(_locker);

            int sum = 0;
            var task1 = Task.Run<int>(() =>
                {
                    try
                    {
                        bool lockToken = false;
                        Monitor.Enter(_locker, ref lockToken);
                        if (lockToken)
                            Monitor.Wait(_locker);//等待队列
                    }
                    finally
                    {
                        Monitor.Exit(_locker);
                    }
                    return sum += 1;
                });
            var task2 = Task.Run<int>(() =>
            {
                try
                {
                    bool lockToken = false;
                    Monitor.Enter(_locker, ref lockToken);
                    if (lockToken)
                        Monitor.Wait(_locker);//等待队列，阻断当前线程
                }
                finally
                {
                    Monitor.Exit(_locker);//释放了锁，释放了共享快
                }
                return sum += 1;
            });
            var task3 = Task.Run<int>(() =>
            {
                try
                {
                    bool lockToken = false;
                    Monitor.Enter(_locker, ref lockToken);
                    if (lockToken)
                        Monitor.Wait(_locker);//等待队列
                }
                finally
                {
                    Monitor.Exit(_locker);
                }
                return sum += 1;
            });
            Thread.Sleep(1000);
            lock (_locker)
            {
                Monitor.Pulse(_locker);//唤醒等待队列里面的线程第一个到就绪队列
            }
            Task.WaitAll(task1, task2, task3);
            Console.WriteLine(string.Format("计算的数值{0}", sum));
        }
        public void WaitHandlerThread()
        {
            // WaitHandle
        }
        public void MutexLocThread()
        {
            var task1 = Task.Run(() => { UseResource(); });
            var task2 = Task.Run(() => { UseResource(); });
            var task3 = Task.Run(() => { UseResource(); });
            var task4 = Task.Run(() => { UseResource(); });
            var task5 = Task.Run(() => { UseResource(); });
            Task.WaitAll(task1, task2, task3, task4, task5);
            Console.WriteLine(string.Format("计算结果{0}", _sum));
        }
        private void UseResource()
        {
            mut.WaitOne();
            for (int i = 0; i < 1000001; i++)
            {
                if (i == 1000000)
                    _sum += i;
            }
            mut.ReleaseMutex();
        }
        public void MutexThread()
        {
            bool flag = false;
            //True:基于创建Mutex线程所有权
            //Mutex mutex = new Mutex(true, "synMutex", out flag);
            //mutex.ReleaseMutex();
            //Mutex：获得所有权与释放所有权必须是同一线程
            Mutex mutex = new Mutex(false, "synMutex", out flag);
            var task1 = Task.Run(() => MutextAdd(mutex, flag));
            var task2 = Task.Run(() => MutextAdd(mutex, flag));
            var task3 = Task.Run(() => MutextAdd(mutex, flag));
            Task.WaitAll(task1, task2, task3);
            Console.WriteLine(_sum);
        }
        private void MutextAdd(Mutex mutex, bool flag)
        {
            try
            {
                //int a = Convert.ToInt32("123H");
                if (flag)
                {
                    for (int i = 0; i < 1000001; i++)
                    {
                        if (i == 1000000)
                            _sum += i;
                    }
                }
                mutex.WaitOne();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        private void MutextAddT(Mutex mutex, bool flag)
        {
            try
            {
                // throw new Exception();
                mutex.WaitOne();
                //  int a = 10;

                int a = Convert.ToInt32("123H");
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
        public void SemaphoreThread()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Semaphore semphore = new Semaphore(1, 8);
            var task1 = Task.Run(() =>
              {
                  Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "正在等待一个许可证……");
                  semphore.WaitOne();
                  Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "申请到许可证……");
                  for (int i = 0; i < 10000001; i++)
                  {
                      if (i == 10000000)
                          _sum += i;
                  }
                  Thread.Sleep(1000);
                  semphore.Release();
                  Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " 释放许可证……");
              });
            var task2 = Task.Run(() =>
             {
                 Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "正在等待一个许可证……");
                 semphore.WaitOne();
                 Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "申请到许可证……");
                 for (int i = 0; i < 10000001; i++)
                 {
                     if (i == 10000000)
                         _sum += i;
                 }
                 Thread.Sleep(1000);
                 semphore.Release();
                 Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " 释放许可证……");
             });
            var task3 = Task.Run(() =>
              {
                  Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "正在等待一个许可证……");
                  semphore.WaitOne();
                  Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "申请到许可证……");
                  for (int i = 0; i < 10000001; i++)
                  {
                      if (i == 10000000)
                          _sum += i;
                  }
                  Thread.Sleep(1000);
                  semphore.Release();
                  Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " 释放许可证……");
              });
            var task5 = Task.Run(() =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "正在等待一个许可证……");
                semphore.WaitOne();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "申请到许可证……");
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                Thread.Sleep(1000);
                semphore.Release();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " 释放许可证……");
            });
            var task6 = Task.Run(() =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "正在等待一个许可证……");
                semphore.WaitOne();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "申请到许可证……");
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                Thread.Sleep(1000);
                semphore.Release();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " 释放许可证……");
            });
            var task7 = Task.Run(() =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "正在等待一个许可证……");
                semphore.WaitOne();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "申请到许可证……");
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                Thread.Sleep(1000);
                semphore.Release();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " 释放许可证……");
            });
            var task8 = Task.Run(() =>
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "正在等待一个许可证……");
                semphore.WaitOne();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "申请到许可证……");
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                Thread.Sleep(1000);
                semphore.Release();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " 释放许可证……");
            });
            Task.WaitAll(task1, task2, task3, task5, task6, task7, task8);
            Console.WriteLine(string.Format("计算当前的数值{0}", _sum));
            stopwatch.Stop();
            Console.WriteLine(string.Format("共耗时{0}", stopwatch.ElapsedMilliseconds));
        }
        public void EventWaitHandleThread()
        {
            EventWaitHandle eventWaitHandle = new EventWaitHandle(true, EventResetMode.AutoReset);
            var task1 = Task.Run(() =>
            {
                eventWaitHandle.WaitOne();
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                eventWaitHandle.Set();
            });

            var task2 = Task.Run(() =>
            {
                eventWaitHandle.WaitOne();
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                eventWaitHandle.Set();
            });

            var task3 = Task.Run(() =>
            {
                eventWaitHandle.WaitOne();
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                eventWaitHandle.Set();
            });

            var task4 = Task.Run(() =>
            {
                eventWaitHandle.WaitOne();
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                eventWaitHandle.Set();
            });
            Task.WaitAll(task1, task2, task3, task4);
            Console.WriteLine(string.Format("计算结果为{0}", _sum));
        }
        public void AutoResetEventThread()
        {
            AutoResetEvent autoResetEvent = new AutoResetEvent(true);
            var task1 = Task.Run(() =>
            {
                autoResetEvent.WaitOne();
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                autoResetEvent.Set();
            });

            var task2 = Task.Run(() =>
            {
                autoResetEvent.WaitOne();
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                autoResetEvent.Set();
            });

            var task3 = Task.Run(() =>
            {
                autoResetEvent.WaitOne();
                for (int i = 0; i < 10000001; i++)
                {
                    if (i == 10000000)
                        _sum += i;
                }
                autoResetEvent.Set();
            });

            Task.WaitAll(task1, task2, task3);
            Console.WriteLine(string.Format("计算结果为{0}", _sum));

        }
        public void ManualResetEventThread()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            var task1 = Task.Run(() => CalcuAddManual(manualResetEvent));
            var task2 = Task.Run(() => CalcuAddManual(manualResetEvent));
            var task3 = Task.Run(() => CalcuAddManual(manualResetEvent));
            var task4 = Task.Run(() => CalcuAddManual(manualResetEvent));
            var task5 = Task.Run(() => CalcuAddManual(manualResetEvent));
            var task6 = Task.Run(() => CalcuAddManual(manualResetEvent));
            var task7 = Task.Run(() => CalcuAddManual(manualResetEvent));
            var task8 = Task.Run(() => CalcuAddManual(manualResetEvent));
            manualResetEvent.Set();
            Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8);
            Console.WriteLine(string.Format("计算结果为{0}", _sum));
            //a=>b=>c
        }
        private void CalcuAddManual(ManualResetEvent manualResetEvent)
        {
            manualResetEvent.WaitOne();
            //for (int i = 0; i < 1000; i++)
            //{
            //    _sum += i;
            //}

            Interlocked.Increment(ref _sum);
            // manualResetEvent.Reset();
            // manualResetEvent.Set();
        }
        public void CountDownEventDemoThread()
        {
            CountdownEvent cde = new CountdownEvent(3); // 创建SemaphoreSlim 初始化信号量最多计数为3次 
            Console.WriteLine(" InitialCount={0}, CurrentCount={1}, IsSet={2}", cde.InitialCount, cde.CurrentCount, cde.IsSet);

            // Launch an asynchronous Task that releases the semaphore after 100 ms
            Task t1 = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (!cde.IsSet)
                    {
                        cde.Signal();
                        Console.WriteLine(" InitialCount={0}, CurrentCount={1}, IsSet={2}", cde.InitialCount, cde.CurrentCount, cde.IsSet);
                    }
                }
            });
            cde.Wait();
            /*将 CurrentCount 重置为 InitialCount 的值。*/
            Console.WriteLine("将 CurrentCount 重置为 InitialCount 的值。");
            cde.Reset();

            cde.Wait();
            /*将 CurrentCount 重置为 5*/
            Console.WriteLine("将 CurrentCount 重置为 5");
            cde.Reset(5);
            cde.AddCount(2);

            cde.Wait();
            /*等待任务完成*/
            //  Task.WaitAll(t1);
            Console.WriteLine("任务执行完成");
            /*释放*/
            cde.Dispose();
            Console.ReadLine();
        }
        public void CountdownEventThread()
        {

            CountdownEvent countDowmEvent = new CountdownEvent(5);
            var task1 = Task.Run(() =>
            {
                CountDowmMethod(countDowmEvent);
            });
            var task2 = Task.Run(() =>
                {
                    CountDowmMethod(countDowmEvent);
                });
            var task3 = Task.Run(() =>
            {
                CountDowmMethod(countDowmEvent);
            });
            var task4 = Task.Run(() =>
            {
                CountDowmMethod(countDowmEvent);
            });
            var task5 = Task.Run(() =>
            {
                CountDowmMethod(countDowmEvent);
            });
            SpinWait.SpinUntil(() => true, 1000);
            countDowmEvent.Signal();
            countDowmEvent.Wait();
            Console.WriteLine("执行完毕");
            Console.WriteLine(string.Format("当前的计算量{0}", _sum));
        }
        private void CountDowmMethod(CountdownEvent countdownEvent)
        {
            Thread.Sleep(1000);
            //  countdownEvent.Signal();
        }
        public void RegisterWaitForSingleObjectThread()
        {
            ///基于信号量的等待操作
            AutoResetEvent wait = new AutoResetEvent(false);
            object state = new object();
            state = "zhangsan";
            RegisteredWaitHandle reg = ThreadPool.RegisterWaitForSingleObject(wait, (x, y) =>
                 {
                     Console.WriteLine("Hello World");
                     Thread.Sleep(1000);
                     if (y)
                         Console.WriteLine("已经超时");
                 }, state, 2000, false);
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
                if (i == 8)
                    Thread.Sleep(3000);
                wait.Set();
            }
            reg.Unregister(wait);
            Console.WriteLine("取消等待操作");

        }
        public void ContextBoundObjectThread()
        {

            AutoLock safeInstance = new AutoLock();
            CountdownEvent countDowmEvent1 = new CountdownEvent(10);
            var task1 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task2 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task3 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task4 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task5 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task6 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task7 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task8 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task9 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            var task10 = Task.Run(() => safeInstance.Demo(countDowmEvent1));
            countDowmEvent1.Wait();
            Console.WriteLine(string.Format("计算的数值{0}", _sum));
        }
        [Synchronization]
        public class AutoLock : ContextBoundObject
        {
            public void Demo(CountdownEvent countDowmEvent1)
            {
                for (var i = 0; i < 1000; i++)
                    _sum += i;

                countDowmEvent1.Signal();
            }
        }
        /// <summary>
        /// volatile ??
        /// </summary>
        public void MemoryBarrierThread()
        {
            int _answer = 0;
            bool _complete = false;
            var task1 = Task.Run(() =>
                 {
                     for (var i = 0; i < 10000; i++)
                     {
                         Thread.MemoryBarrier();
                         _answer += i;
                         Thread.MemoryBarrier();
                     }
                     _complete = true;
                 });
            var task2 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task3 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task4 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task5 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task6 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task7 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task8 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task9 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            var task10 = Task.Run(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Thread.MemoryBarrier();
                    _answer += i;
                    Thread.MemoryBarrier();
                }
                _complete = true;
            });
            Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
            Console.WriteLine(string.Format("计算值{0}", _answer));
        }
        public void InterlockedThread()
        {
            int localNum = 0;

            var task1 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }

            });

            var task2 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }

            });
            var task3 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            var task4 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            var task5 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            var task6 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            var task7 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            var task8 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            var task9 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            var task10 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                {
                    Interlocked.Add(ref _sum, i);

                    Interlocked.Increment(ref localNum);
                }
            });
            Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
            Console.WriteLine(string.Format("计算的最终数值{0},{1}", _sum, localNum));
        }
        public void ReaderWriterLockThread()
        {
            ReaderWriterLock rw = new ReaderWriterLock();
            var task1 = Task.Run(() => WriteLock(rw));
            var task11 = Task.Run(() => ReadLock(rw));
            var task2 = Task.Run(() => WriteLock(rw));
            var task12 = Task.Run(() => ReadLock(rw));
            var task3 = Task.Run(() => WriteLock(rw));
            var task13 = Task.Run(() => ReadLock(rw));
            var task4 = Task.Run(() => WriteLock(rw));
            var task14 = Task.Run(() => ReadLock(rw));
            var task5 = Task.Run(() => WriteLock(rw));
            var task15 = Task.Run(() => ReadLock(rw));
            var task6 = Task.Run(() => WriteLock(rw));
            var task16 = Task.Run(() => ReadLock(rw));
            var task7 = Task.Run(() => WriteLock(rw));
            var task17 = Task.Run(() => ReadLock(rw));
            var task8 = Task.Run(() => WriteLock(rw));
            var task18 = Task.Run(() => ReadLock(rw));
            var task9 = Task.Run(() => WriteLock(rw));
            var task19 = Task.Run(() => ReadLock(rw));
            var task10 = Task.Run(() => WriteLock(rw));
            Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
            Console.WriteLine(string.Format("计算的最终数值{0}", _sum));
        }
        public void ReadLock(ReaderWriterLock rw)
        {
            try
            {
                rw.AcquireReaderLock(Timeout.Infinite);
                Console.WriteLine(_sum);
            }
            finally
            {
                if (rw.IsReaderLockHeld)
                    rw.ReleaseReaderLock();
            }
        }
        public void WriteLock(ReaderWriterLock rw)
        {
            try
            {
                rw.AcquireWriterLock(Timeout.Infinite);
                for (int i = 0; i < 1000; i++)
                    _sum += i;
            }
            finally
            {
                if (rw.IsWriterLockHeld)
                    rw.ReleaseWriterLock();
            }
        }
        public void ReaderWriterLockSlimThread()
        {
            ReaderWriterLockSlim rw = new ReaderWriterLockSlim();
            var task1 = Task.Run(() => WriteLockSlim(rw));
            var task11 = Task.Run(() => ReadLockSlim(rw));
            var task2 = Task.Run(() => WriteLockSlim(rw));
            var task12 = Task.Run(() => ReadLockSlim(rw));
            var task3 = Task.Run(() => WriteLockSlim(rw));
            var task13 = Task.Run(() => ReadLockSlim(rw));
            var task4 = Task.Run(() => WriteLockSlim(rw));
            var task14 = Task.Run(() => ReadLockSlim(rw));
            var task5 = Task.Run(() => WriteLockSlim(rw));
            var task15 = Task.Run(() => ReadLockSlim(rw));
            var task6 = Task.Run(() => WriteLockSlim(rw));
            var task16 = Task.Run(() => ReadLockSlim(rw));
            var task7 = Task.Run(() => WriteLockSlim(rw));
            var task17 = Task.Run(() => ReadLockSlim(rw));
            var task8 = Task.Run(() => WriteLockSlim(rw));
            var task18 = Task.Run(() => ReadLockSlim(rw));
            var task9 = Task.Run(() => WriteLockSlim(rw));
            var task19 = Task.Run(() => ReadLockSlim(rw));
            var task10 = Task.Run(() => WriteLockSlim(rw));
            Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10);
            Console.WriteLine(string.Format("计算的最终数值{0}", _sum));
        }
        public void ReadLockSlim(ReaderWriterLockSlim rw)
        {
            try
            {
                rw.TryEnterReadLock(Timeout.Infinite);
                Console.WriteLine(_sum);
            }
            finally
            {
                if (rw.IsReadLockHeld)
                    rw.ExitReadLock();
            }
        }
        public void WriteLockSlim(ReaderWriterLockSlim rw)
        {

            try
            {
                rw.TryEnterWriteLock(Timeout.Infinite);
                for (int i = 0; i < 1000; i++)
                    _sum += i;
            }
            finally
            {
                if (rw.IsWriteLockHeld)
                    rw.ExitWriteLock();
            }
        }
        public void ThreadPoolDemo()
        {
            //SynchronizationContext
            CallContext.LogicalSetData("Name", "Jeffrey");
            ThreadPool.QueueUserWorkItem(state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));
            ExecutionContext.SuppressFlow();
            ThreadPool.QueueUserWorkItem(state => Console.WriteLine("Name={0}", CallContext.LogicalGetData("Name")));
            ExecutionContext.RestoreFlow();

            var cts = new CancellationTokenSource();
            cts.Token.Register(() => Console.WriteLine("Register is Cancel"));
            cts.Cancel();
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
            linkedCts.Token.Register(() => Console.WriteLine("After Cts Cancel"));

            Console.WriteLine("Cts1 canceled={0},linkedCts canceled={1}", cts.IsCancellationRequested, linkedCts.IsCancellationRequested);
            Console.Read();

            // Task<string>
            // OperationCanceledException
            //AggregateException
            // AggregateException
        }
        public async Task<int> GetSumFromResult()
        {

            return await Task.FromResult(GetAsyncResult()).Result;
        }
        public async Task<int> GetAsyncResult()
        {
            return await Task.Run(() => { return 10; });
        }
        public void TaskCanceled()
        {
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

            cancelTokenSource.Cancel();

            //try
            //{
            //    var task = GetResultAsync(cancelTokenSource.Token);

            //   // int a = task.Result;
            //}
            //catch (AggregateException ex)
            //{
            // //   throw ex.InnerException;
            //}

            var task = Task.Run(() =>
              {
                  GetResultAsync(cancelTokenSource.Token);
              });

            task.Wait();
        }
        public Task<int> GetResultAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
                //return Task.FromCanceled<int>(cancellationToken);
            }
            try
            {
                int a = Convert.ToInt32("ee");
                return Task.FromResult(GetResultSync());
            }
            catch (Exception exception)
            {
                return Task.FromException<int>(exception);
            }
        }
        public int GetResultSync()
        {
            return 10;
            // ...
        }
        public Task<int> TestAsync(int a, int b)
        {
            var tcs = new TaskCompletionSource<int>();
            Task.Factory.StartNew(() =>
            {
                if (a + b < 0)
                {
                    tcs.TrySetException(new InvalidOperationException("a + b < 0"));
                }
                else
                {
                    tcs.TrySetResult(a + b);
                }
            });
            return tcs.Task;
        }
    }
    public class TaskThread
    {
        public class Person
        {
            private static object _lockStatus = new object();
            public static int Count { get; set; }

            /// <summary>
            /// 静态方法在内部是线程安全的
            /// </summary>
            public static void Sum()
            {
                lock (_lockStatus)
                {
                    for (int i = 0; i < 10000000; i++)
                        Count++;
                }

            }
        }

        public async Task SayHello()
        {



            await Task.Run(() => Console.WriteLine("Hello World"));
            // System.Runtime.CompilerServices.TaskAwaiter
            //System.Runtime.CompilerServices.IAsyncStateMachine
            // System.Runtime.CompilerServices.AsyncTaskMethodBuilder
            // TaskCompletionSource
            //return await Task.Run(() =>
            //   {
            //       return 1;
            //   });
        }

        public async Task SayNotWait()
        {
            //作为一种同步方法执行
            Console.WriteLine("Hello World");
        }

        public void SimpleTask()
        {
            Task.Run(() => Console.WriteLine("HelloWorld"));
        }

        public void SayYield()
        {
            IList<A> aInstance = new List<A>
            {
                new A() {Name="zhangsan" }, new A() {Name="lisi" }
            };
            IList<B> bInstance = new List<B>
            {
                new B() {Age="14" }, new B() {Age="20" }
            };
            var leftInstance = (from a in aInstance.AsQueryable()
                                join b in bInstance.AsQueryable()
                                on a.Name equals b.Age into temp
                                from tt in temp.DefaultIfEmpty()
                                select new
                                {
                                    sName = a.Name,
                                    sAge = tt == null ? "" : tt.Age
                                }).ToList();
            var rightInstance = (from a in bInstance.AsQueryable()
                                 join b in aInstance.AsQueryable()
                                 on a.Age equals b.Name into temp
                                 from tt in temp.DefaultIfEmpty()
                                 select new
                                 {
                                     sName = tt == null ? "" : tt.Name,
                                     sAge = a.Age
                                 }).ToList();
            var unionList = leftInstance.Union(rightInstance).ToList();
        }
        public class A
        {
            public string Name { get; set; }
        }
        public class B
        {
            public string Age { get; set; }
        }

        //public class AllanControlAwaiter : INotifyCompletion
        //{

        //    //private readonly DependencyObject m_control;
        //    //private bool _IsCompleted = false;
        //    //public AllanControlAwaiter(DependencyObject control)
        //    //{
        //    //    m_control = control;
        //    //}

        //    //public bool IsCompleted
        //    //{
        //    //    get { return _IsCompleted; }
        //    //}

        //    //public void OnCompleted(Action continuation)
        //    //{
        //    //    var result = m_control.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => continuation());
        //    //    result.AsTask().Wait();
        //    //    _IsCompleted = true;
        //    //}

        //    //public void GetResult() { }


        //}

        public void MultStatic()
        {

            var t1 = new TaskFactory().StartNew(() => Person.Sum());
            var t2 = new TaskFactory().StartNew(() => Person.Sum());
            var t3 = new TaskFactory().StartNew(() => Person.Sum());
            var t4 = new TaskFactory().StartNew(() => Person.Sum());
            Task.WaitAll(t1, t2, t3, t4);
            Console.WriteLine(Person.Count);
        }
        public void TaskDemo()
        {
            // CancellationTokenSource cancelToken = new CancellationTokenSource(3000);
            // Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken);
            // Func<string> fun=()=>"Hello World";
            // Task<string> task = Task.Run<string>(fun, cancelToken);
            Task.Run(() => Console.WriteLine("Hello World"));
            // Console.ReadKey();


        }
        public void TaskFacotryDemo()
        {
            var task1 = Task.Factory.StartNew(() =>
            {
                var task2 = Task.Factory.StartNew(() =>
                {

                    Thread.Sleep(2000);

                    Console.WriteLine("Child Task Is Excuted Finshed");
                }, TaskCreationOptions.AttachedToParent);
                Console.WriteLine("Parent Task Is Excuted Finshed");
            });
            task1.Wait();

            Console.WriteLine("Finshed");
        }
        public void TaskFactoryContinue()
        {
            var taskw = Task.Factory.StartNew(() =>
                  {
                      var t1 = Task.Factory.StartNew<int>(() =>
                          {
                              Console.WriteLine("Task 1 running ....");

                              return 1;
                          });
                      t1.Wait();

                      var t2 = Task.Factory.StartNew<int>(() =>
                      {
                          Console.WriteLine("Task 2 running....");
                          return t1.Result + 2;
                      });

                      var t3 = Task.Factory.StartNew<int>(() =>
                      {
                          Console.WriteLine("Task 3 running....");

                          return t1.Result + 3;
                      }).ContinueWith<int>(task =>
                      {
                          Console.WriteLine("Task 4 running");
                          return task.Result + 4;
                      });

                      Task.WaitAll(t2, t3);

                      var result = Task.Factory.StartNew(() =>
                          {
                              Console.WriteLine(string.Format(" Task Finished! The Result is t2:{0}+t3:{1}={2}",
                                  t2.Result, t3.Result, t2.Result + t3.Result));
                          }, TaskCreationOptions.AttachedToParent);
                  });

            taskw.Wait();

            Console.WriteLine("Fished !");
        }
        public void TaskException()
        {
            ///这种wait 方式阻塞主线程
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Task t = new Task(() =>
            {
                Thread.Sleep(2000);
                throw new Exception("task1出现未知的异常");
            });
            t.Start();
            try
            {
                t.Wait();
            }
            catch (AggregateException ex)
            {
                foreach (var item in ex.InnerExceptions)
                    Console.WriteLine(item.Message);
            }
            Thread.Sleep(2000);
            stopwatch.Stop();
            Console.WriteLine(string.Format("共耗时{0}", stopwatch.ElapsedMilliseconds));

            ///这种方式也不会阻塞主线程
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Task t2 = new Task(() =>
            {
                Thread.Sleep(2000);
                throw new Exception("task2出现异常");
            });
            t2.Start();

            Task ttEnd = t2.ContinueWith(task =>
            {
                foreach (var item in task.Exception.InnerExceptions)
                    Console.WriteLine(item.Message);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("结束");

            Thread.Sleep(2000);
            watch.Stop();
            Console.WriteLine(string.Format("共耗时{0}", watch.ElapsedMilliseconds));

        }
        public class AggregationExcptionArgs : EventArgs
        {
            public AggregateException AggException { get; set; }
        }
        event EventHandler<AggregationExcptionArgs> _aggExceptionEvent;
        public void TaskExceptionEvent()
        {
            ///用事件的方式来捕获异常到主线程，不用阻塞主线程
            Stopwatch watch = new Stopwatch();
            watch.Start();
            _aggExceptionEvent += AggreationExceptionCatched;
            Task t = new Task(() =>
            {
                try
                {
                    Thread.Sleep(2000);
                    throw new Exception("Error");
                }
                catch (Exception ex)
                {
                    AggregationExcptionArgs errArgs = new AggregationExcptionArgs()
                    {
                        AggException = new AggregateException(ex)
                    };
                    _aggExceptionEvent(null, errArgs);
                }
            });
            t.Start();
            Console.WriteLine("主线程马上结束");
            Thread.Sleep(2000);
            watch.Stop();
            Console.WriteLine(string.Format("共耗时{0}", watch.ElapsedMilliseconds));
            Console.ReadKey();
        }
        public void AggreationExceptionCatched(object sender, AggregationExcptionArgs e)
        {
            foreach (var item in e.AggException.InnerExceptions)
                Console.WriteLine(item.Message);
        }
        public void CancelTokenSourceTask()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task Cancel detected");
                        throw new Exception("哈哈哈");
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Int value {0}", i));
                    }
                }
            }, token);

            var t2 = task.ContinueWith(task1 =>
                {
                    foreach (var item in task1.Exception.InnerExceptions)
                        Console.WriteLine(item.Message);
                }, TaskContinuationOptions.OnlyOnFaulted);
            task.Start();
            Console.ReadKey();
            Console.WriteLine("Main method complete . Press enter to finish");
            Console.ReadKey();
        }
        public void CancelTokenSourceRegister()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task Cancel detected");
                        throw new Exception("哈哈哈");
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Int value {0}", i));
                    }
                }
            }, token);


            var t2 = task.ContinueWith(task1 =>
            {
                foreach (var item in task1.Exception.InnerExceptions)
                    Console.WriteLine(item.Message);
            }, TaskContinuationOptions.OnlyOnFaulted);
            token.Register(() =>
            {
                Console.WriteLine(">>>>>> Delegate Invoked\n");
            });
            task.Start();
            Console.ReadKey();
            source.Cancel();
            Console.WriteLine("Main method complete . Press enter to finish");
            Console.ReadKey();

        }
        public void CancelTokenWaitHandle()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task Cancel detected");
                        throw new Exception("我是异常额");
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Int value {0}", i));
                    }
                }
            }, token);
            var t2 = task.ContinueWith(task1 =>
            {
                foreach (var item in task1.Exception.InnerExceptions)
                    Console.WriteLine(item.Message);
            }, TaskContinuationOptions.OnlyOnFaulted);
            token.Register(() =>
            {
                Console.WriteLine(">>>>>> Delegate Invoked");
            });

            var task3 = new Task(() =>
            {
                token.WaitHandle.WaitOne();

                Console.WriteLine("阻断继续执行");
            });
            task3.Start();
            task.Start();
            Console.ReadLine();
            source.Cancel();
            Thread.Sleep(500);
            Console.WriteLine("Main method complete . Press enter to finish");
            Console.ReadLine();
        }
        public void CancelTokenCompositeTask()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // create the cancellation token
            CancellationToken token = tokenSource.Token;
            Task task1 = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine("Task 1 - Int value {0}", i);
                }
            }, token);
            Task task2 = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Console.WriteLine("Task 2 - Int value {0}", i);
                }
            }, token);
            // wait for input before we start the tasks
            Console.WriteLine("Press enter to start tasks");
            Console.WriteLine("Press enter again to cancel tasks");
            Console.ReadLine();
            // start the tasks
            task1.Start();
            task2.Start();
            // read a line from the console.
            Console.ReadLine();
            // cancel the task
            Console.WriteLine("Cancelling tasks");
            tokenSource.Cancel();
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
        public void CancelTokenCompositedLinkToken()
        {
            CancellationTokenSource tokenSource1 = new CancellationTokenSource();
            CancellationTokenSource tokenSource2 = new CancellationTokenSource();
            CancellationTokenSource tokenSource3 = new CancellationTokenSource();
            CancellationTokenSource compositeSource = CancellationTokenSource.CreateLinkedTokenSource(
                tokenSource1.Token, tokenSource2.Token, tokenSource3.Token);
            Task task = new Task(() =>
            {
                compositeSource.Token.WaitHandle.WaitOne();
                throw new OperationCanceledException(compositeSource.Token);
            }, compositeSource.Token);
            task.Start();
            tokenSource2.Cancel();
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
        public void CancelTokenWaitHandleSleep()
        {
            CancellationTokenSource tokensource = new CancellationTokenSource();
            CancellationToken token = tokensource.Token;
            Task task1 = new Task(() =>
            {
                for (int i = 0; i < Int32.MaxValue; i++)
                {
                    bool cancelled = token.WaitHandle.WaitOne(5000);
                    Console.WriteLine("Task 1 - Int value {0}. Cancelled? {1}",
                    i, cancelled);
                    if (cancelled)
                    {
                        throw new OperationCanceledException(token);
                    }
                }
            }, token);
            task1.Start();
            Console.WriteLine("Press enter to cancel token.");
            Console.ReadLine();
            tokensource.Cancel();
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
        public void CancelTokenSpainWait()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // create the cancellation token
            CancellationToken token = tokenSource.Token;

            // create the first task, which we will let run fully
            Task task1 = new Task(() =>
            {
                for (int i = 0; i < Int32.MaxValue; i++)
                {
                    // put the task to sleep for 10 seconds
                    Thread.SpinWait(5000);
                    // print out a message
                    Console.WriteLine("Task 1 - Int value {0}", i);
                    // check for task cancellation
                    token.ThrowIfCancellationRequested();
                }
            }, token);

            // start task
            task1.Start();

            // wait for input before exiting
            Console.WriteLine("Press enter to cancel token.");

            Console.ReadLine();
            // cancel the token
            tokenSource.Cancel();

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
        public void LazyTask()
        {
            Lazy<Task<string>> lazyData = new Lazy<Task<string>>(() =>
            {
                return Task<string>.Factory.StartNew(() =>
                    {
                        Console.WriteLine("Task body working....");
                        return "Task Result";
                    });
            });

            Lazy<Task<string>> lazyData2 = new Lazy<Task<string>>(() =>
            {
                return Task.Factory.StartNew<string>(() =>
                  {
                      Console.WriteLine("Task Body working");
                      return "Task 2 Result";
                  });
            });

            Console.WriteLine("Calling second lazy variable");

            Console.WriteLine("Result form taske:{0}", lazyData2.Value.Result);

            Console.ReadLine();
        }
        public void BarrierTask()
        {
            var taskNum = 10;
            Task[] taskArray = new Task[taskNum];
            Barrier barrier = new Barrier(10, (_barrier) =>
            {
                Console.WriteLine("--------------Current Base {0}------------", _barrier.CurrentPhaseNumber);
                // if (_barrier.CurrentPhaseNumber == 1)
                // throw new InvalidOperationException("Phase 2 need to be TERMINTED!!!!!");
                // throw new Exception("异常");
            });
            var cts = new System.Threading.CancellationTokenSource();
            var ct = cts.Token;
            for (int i = 0; i < taskNum; i++)
            {
                taskArray[i] = Task.Factory.StartNew(num =>
                {
                    var taskId = Task.CurrentId;
                    Console.WriteLine("Task : #{0}   =====  Phase 0", taskId);
                    //  barrier.SignalAndWait();
                    // try
                    // {
                    // throw new Exception("我是异常");
                    // }
                    // catch (Exception ex)
                    // {
                    //     Console.WriteLine("Hello WOrld");
                    // }
                    //Console.WriteLine("Task : #{0}   =====  Phase 1", taskId);
                    // barrier.SignalAndWait();
                    // Console.WriteLine("Task : #{0}   =====  Phase 2", taskId);
                    // barrier.SignalAndWait();
                    // Console.WriteLine("Task : #{0}   =====  Phase 3", taskId);
                    // barrier.SignalAndWait();

                }, i, ct);
            }
            barrier.SignalAndWait();
            Console.WriteLine("I'm Waiting End");
            try
            {
                Task.WaitAll(taskArray);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                barrier.Dispose();
            }

            Console.ReadLine();

        }
        public void ManualResetEventSlimTask()
        {
            ManualResetEventSlim manual = new ManualResetEventSlim(false);
            var observer = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("阻止当前线程，使manual 处于等待状态....!");
                    manual.Wait();
                    while (true)
                    {
                        if (manual.IsSet)
                        {
                            Console.WriteLine("得到manual 信号 ,执行后续代码....!");
                        }
                        Thread.Sleep(100);
                    }
                });
            Thread.Sleep(2000);
            Console.WriteLine("取消manu 等待状态");
            manual.Set();
            Console.WriteLine("当前信号状态:{0}", manual.IsSet);
            Thread.Sleep(300);
            manual.Reset();
            // Console.WriteLine("当前信号状态:{0}", manual.IsSet);
            //  Thread.Sleep(300);
            //  manual.Set();
            //  Console.WriteLine("当前信号状态:{0}", manual.IsSet);
            // Thread.Sleep(300);
            // manual.Reset();
        }
        public void SemaphoreSlimTask()
        {
            // ConcurrentBag<string>
            SemaphoreSlim semaphore = new SemaphoreSlim(3);
            Task t1 = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        semaphore.Wait();
                        Console.WriteLine("允许进入 SemaphoreSlim 的线程的数量：{0}", semaphore.CurrentCount);
                        Thread.Sleep(10);
                    }
                });
            Thread.Sleep(3000);
            Console.WriteLine("退出信号量,并递增信号量的计数");
            semaphore.Release();
            Thread.Sleep(3000);
            /*退出3次信号量  并递增信号量的计数*/
            Console.WriteLine("退出三次信号量  并递增信号量的计数");
            semaphore.Release(3);
            /*等待任务完成*/
            Task.WaitAll(t1);
            /*释放*/
            semaphore.Dispose();
            Console.ReadLine();
        }
        public void SpinLockTask()
        {
            var li = new List<int>();
            var si = new SpinLock();
            Parallel.For(0, 1000 * 1000, r =>
                {
                    bool gotLock = false;
                    si.Enter(ref gotLock);//进入锁
                    li.Add(r);
                    if (gotLock)
                        si.Exit();//释放锁
                });
            Console.WriteLine(li.Count);
        }
        private static int _count = 1000;
        private static int _timeout_ms = 10;
        public void SpinWaitTask()
        {
            Thread thread = new Thread(() =>
             {
                 var sw = Stopwatch.StartNew();
                 for (int i = 0; i < _count; i++)
                 {
                     SpinWait.SpinUntil(() => true, _timeout_ms);
                 }

                 Console.WriteLine("SpinWait Consume Time:{0}", sw.Elapsed.ToString());
             });

            thread.IsBackground = true;
            thread.Start();
        }
        public void ThreadSleepInThread()
        {
            Thread thread = new Thread(() =>
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < _count; i++)
                {
                    Thread.Sleep(_timeout_ms);
                }
                Console.WriteLine("Thread Sleep Consume Time:{0}", sw.Elapsed.ToString());
            });
            thread.IsBackground = true;
            thread.Start();
        }
        /// <summary>
        ///           TaskCompletionSource<Task>
        ///   BlockingCollection<T>
        /// TaskScheduler
        ///   /// ParallelLoopState
        ///   OrderablePartitioner
        ///   Partitioner
        /// </summary>
        public void ParrelInvokeTask()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.Invoke(PararelInvokeRun1, PararelInvokeRun2);
            stopwatch.Stop();
            Console.WriteLine("Parallel run " + stopwatch.ElapsedMilliseconds + " ms.");

            stopwatch.Restart();
            PararelInvokeRun1();
            PararelInvokeRun2();
            stopwatch.Stop();
            Console.WriteLine("Normal run " + stopwatch.ElapsedMilliseconds + " ms.");

            IList<string> a = new List<String>();

        }
        private void PararelInvokeRun1()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Task 1 is cost 2 sec");
        }
        private void PararelInvokeRun2()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 is cost 3 sec");
        }
        public void ParallForTask()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 60000; j++)
                {
                    int sum = 0;
                    sum += i;
                }
            }
            stopWatch.Stop();
            Console.WriteLine("NormalFor run " + stopWatch.ElapsedMilliseconds + " ms.");
            stopWatch.Reset();
            stopWatch.Start();

            Parallel.For(0, 10000, item =>
                {
                    for (int j = 0; j < 60000; j++)
                    {
                        int sum = 0;
                        sum += item;
                    }
                });
            stopWatch.Stop();
            Console.WriteLine("ParallelFor run " + stopWatch.ElapsedMilliseconds + " ms.");
        }
        public void ParallForEachTask()
        {
            var listName = new System.Collections.Concurrent.ConcurrentBag<int>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var list = new List<int>();
            var loopCount = 1000000;

            var listAge = new List<int>();
            for (int i = 0; i < loopCount; i++)
                list.Add(i);
            Parallel.ForEach(list, item =>
                {
                    listName.Add(item);
                });
            stopwatch.Stop();
            Console.WriteLine("Parallel ForEach " + stopwatch.ElapsedMilliseconds + " ms.");
            stopwatch.Reset();
            stopwatch.Start();
            foreach (var item in list)
                listAge.Add(item);
            stopwatch.Stop();
            Console.WriteLine("For " + stopwatch.ElapsedMilliseconds + " ms.");
        }
        public void ParallLoopStateTask()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Parallel.For(0, 1000, (i, state) =>
            {
                if (bag.Count == 300)
                {
                    state.Stop();
                    return;
                }
                bag.Add(i);
            });
            stopWatch.Stop();
            Console.WriteLine("Bag count is " + bag.Count + ", " + stopWatch.ElapsedMilliseconds);
        }
        public void ParallExceptionTask()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                Parallel.For(0, 1000, (i, state) =>
                {
                    throw new Exception("Hell");
                });
            }
            catch (AggregateException ex)
            {
                foreach (var item in ex.InnerExceptions)
                    Console.WriteLine(item.Message);
            }
            stopWatch.Stop();
            Console.WriteLine("Bag count is " + bag.Count + ", " + stopWatch.ElapsedMilliseconds);
        }
        public void ParellOrderablePartitionerTask()
        {
            IList<string> array = new List<string>(){
                "Hello",
                "World",
                "Hell",
                "testt"
            };
            var rangeSize = 3;
            var part = Partitioner.Create(0, array.Count, rangeSize);
            Parallel.ForEach(part, (range, state, rangeIndex) =>
            {
                for (Int32 index = range.Item1; index < range.Item2; index++)
                {
                    Console.WriteLine("[{0,2}] {1} {2}", Thread.CurrentThread.ManagedThreadId, rangeIndex, index);

                    Console.WriteLine("{0}", array[index]);
                }
            });



        }
        public void LinqAsParrelTask()
        {
            Stopwatch sw = new Stopwatch();
            List<Custom> customs = new List<Custom>();
            for (int i = 0; i < 2000000; i++)
            {
                customs.Add(new Custom() { Name = "Jack", Age = 21, Address = "NewYork" });
                customs.Add(new Custom() { Name = "Jime", Age = 26, Address = "China" });
                customs.Add(new Custom() { Name = "Tina", Age = 29, Address = "ShangHai" });
                customs.Add(new Custom() { Name = "Luo", Age = 30, Address = "Beijing" });
                customs.Add(new Custom() { Name = "Wang", Age = 60, Address = "Guangdong" });
                customs.Add(new Custom() { Name = "Feng", Age = 25, Address = "YunNan" });
            }
            sw.Start();
            var result = customs.Where<Custom>(c => c.Age > 26).ToList();
            sw.Stop();
            Console.WriteLine("Linq time is {0}.", sw.ElapsedMilliseconds);

            sw.Restart();
            sw.Start();
            var result2 = customs.AsParallel().Where<Custom>(c => c.Age > 26).ToList();
            sw.Stop();
            Console.WriteLine("Parallel Linq time is {0}.", sw.ElapsedMilliseconds);
        }
        public class Custom
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Address { get; set; }
        }
        public void PararellEnumableTask()
        {
            Console.WriteLine(ParallelEnumerable.Range(0, 1000).Sum(i => Math.Sqrt(i)));

            int[] numbers = { 1, 2, 3 };
            int sum = numbers.Aggregate(0, (total, n) => total + n);   // 6

            var te = numbers.AsParallel().Aggregate(
                        () => 0,                                     // 种子工厂
                        (localTotal, n) => localTotal + n,           // 更新累加器方法
                        (mainTot, localTot) => mainTot + localTot,   // 合并累加器方法
                        finalResult => finalResult);
            string text = "Let’s suppose this is a really long string";
            //var letterFrequencies = new int[26];

            int[] result =
                      text.Aggregate(
                        new int[26],                // 创建“累加器”
                        (letterFrequencies, c) =>   // 聚合一个字母至累加器
                        {
                            int index = char.ToUpper(c) - 'A';
                            if (index >= 0 && index <= 26) letterFrequencies[index]++;
                            return letterFrequencies;
                        });
            int[] result2 =
                  text.AsParallel().Aggregate(
                    () => new int[26],             // 新建局部累加器

                    (localFrequencies, c) =>       // 聚合至局部累加器
                    {
                        int index = char.ToUpper(c) - 'A';
                        if (index >= 0 && index <= 26) localFrequencies[index]++;
                        return localFrequencies;
                    },
                    // 聚合局部累加器至主累加器
                    (mainFreq, localFreq) =>
                      mainFreq.Zip(localFreq, (f1, f2) => f1 + f2).ToArray(),
                    finalResult => finalResult     // 对结果进行
                  );                               // 最终变换
        }
        public void LookUpTask()
        {
            Stopwatch stopWatch = new Stopwatch();
            List<Custom> customs = new List<Custom>();
            for (int i = 0; i < 2000000; i++)
            {
                customs.Add(new Custom() { Name = "Jack", Age = 21, Address = "NewYork" });
                customs.Add(new Custom() { Name = "Jime", Age = 26, Address = "China" });
                customs.Add(new Custom() { Name = "Tina", Age = 29, Address = "ShangHai" });
                customs.Add(new Custom() { Name = "Luo", Age = 30, Address = "Beijing" });
                customs.Add(new Custom() { Name = "Wang", Age = 60, Address = "Guangdong" });
                customs.Add(new Custom() { Name = "Feng", Age = 25, Address = "YunNan" });
            }

            stopWatch.Restart();
            var groupByAge = customs.GroupBy(item => item.Age).ToList();
            foreach (var item in groupByAge)
            {
                Console.WriteLine("Age={0},count = {1}", item.Key, item.Count());
            }
            stopWatch.Stop();

            Console.WriteLine("Linq group by time is: " + stopWatch.ElapsedMilliseconds);


            stopWatch.Restart();
            var lookupList = customs.ToLookup(i => i.Age);
            foreach (var item in lookupList)
            {
                Console.WriteLine("LookUP:Age={0},count = {1}", item.Key, item.Count());
            }
            stopWatch.Stop();
            Console.WriteLine("LookUp group by time is: " + stopWatch.ElapsedMilliseconds);

        }
        public void TaskExceptionScheduler()
        {
            TaskScheduler.UnobservedTaskException +=
               (object sender, UnobservedTaskExceptionEventArgs eventArgs) =>
               {
                   //// 阻止程序崩溃的方法有2种  
                   ////第一种是：  
                   {
                       eventArgs.SetObserved();
                       Console.WriteLine("Exception handled");
                   }
                   //第二种，返回true  
                   //if (false)
                   //{
                   //    ((AggregateException)eventArgs.Exception).Handle(ex =>
                   //    {
                   //        Console.WriteLine("Exception handled");
                   //        return true;
                   //    });
                   //}
               };
            RunTask();
            // 不断分配内存，强制让GC收集Task对象，从而触发UnobservedTaskException  
            ArrayList arr = new ArrayList();
            while (true)
            {
                char[] array = new char[100000];
                arr.Add(array);
                GC.Collect();
            }
        }
        private void RunTask()
        {
            new Task(() => { throw new NullReferenceException(); }).Start();
        }
        public void TaskAsync()
        {
            int sum = 0;
            Task[] taskArray = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                taskArray[i] = Task.Run(() =>
                    {
                        for (int j = 0; j < 1000; j++)
                            Interlocked.Add(ref sum, j);
                    });
            }
            Task.WaitAll(taskArray);
            Console.WriteLine("当前计算出来的结果{0}", sum);
        }
        public async void AysncTask()
        {
            // AspNetSynchronizationContext
            // SynchronizationContext
            //SynchronizationContext
            //System.Runtime.CompilerServices.IAsyncStateMachine
            // System.Runtime.CompilerServices.IAsyncStateMachine
            // System.Runtime.CompilerServices.TaskAwaiter
            //   System.Runtime.CompilerServices.TaskAwaiter<string>
            // System.Runtime.CompilerServices.AsyncTaskMethodBuilder
            // System.Runtime.CompilerServices.AsyncTaskMethodBuilder<string>
            var result = await CalAsync();
            _sumValue = 20;
        }
        public int _sumValue = 0;
        private Task<int> CalAsync()
        {
            _sumValue = 100;
            return Task.Run<int>(() => _sumValue);
        }
        public Task<double> GetValueAsync(double num1, double num2)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    num1 = num1 / num2;
                }
                return num1;
            });
        }
        public void TaskAwait()
        {
            TaskAwaiter<double> awaiter = GetValueAsync(1234.5, 1.01).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                double result = awaiter.GetResult();
                Console.WriteLine(result);
            });
        }
    }
    public class TaskInDeep
    {

        public void TaskScheduleDemo()
        {
            LimitedConcurrencyLevelTaskScheduler lcts = new LimitedConcurrencyLevelTaskScheduler(2);
            List<Task> tasks = new List<Task>();

            // Create a TaskFactory and pass it our custom scheduler. 
            TaskFactory factory = new TaskFactory(lcts);
            CancellationTokenSource cts = new CancellationTokenSource();

            // Use our factory to run a set of tasks. 
            Object lockObj = new Object();
            int outputItem = 0;

            for (int tCtr = 0; tCtr <= 4; tCtr++)
            {
                int iteration = tCtr;
                Task t = factory.StartNew(() =>
                {
                    for (int i = 0; i < 3; i++)
                    {
                        lock (lockObj)
                        {
                            Console.Write("{0} in task t-{1} on thread {2}   ",
                                          i, iteration, Thread.CurrentThread.ManagedThreadId);
                            outputItem++;
                            if (outputItem % 3 == 0)
                                Console.WriteLine();
                        }
                    }
                }, cts.Token);
                tasks.Add(t);
            }
            // Use it to run a second set of tasks.                       
            //for (int tCtr = 0; tCtr <= 4; tCtr++)
            //{
            //    int iteration = tCtr;
            //    Task t1 = factory.StartNew(() => {
            //        for (int outer = 0; outer <= 10; outer++)
            //        {
            //            for (int i = 0x21; i <= 0x7E; i++)
            //            {
            //                lock (lockObj)
            //                {
            //                    Console.Write("'{0}' in task t1-{1} on thread {2}   ",
            //                                  Convert.ToChar(i), iteration, Thread.CurrentThread.ManagedThreadId);
            //                    outputItem++;
            //                    if (outputItem % 3 == 0)
            //                        Console.WriteLine();
            //                }
            //            }
            //        }
            //    }, cts.Token);
            //    tasks.Add(t1);
            //}

            // Wait for the tasks to complete before displaying a completion message.
            Task.WaitAll(tasks.ToArray());
            cts.Dispose();
            Console.WriteLine("\n\nSuccessful completion.");
        }
    }
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;

        // The list of tasks to be executed 
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks)

        // The maximum concurrency level allowed by this scheduler. 
        private readonly int _maxDegreeOfParallelism;

        // Indicates whether the scheduler is currently processing work items. 
        private int _delegatesQueuedOrRunning = 0;

        // Creates a new instance with the specified degree of parallelism. 
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        // Queues a task to the scheduler. 
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough 
            // delegates currently queued or running to process tasks, schedule another. 
            lock (_tasks)
            {
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }

        // Inform the ThreadPool that there's work to be executed for this scheduler. 
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items.
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue.
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed,
                            // note that we're done processing, and get out.
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;
                                break;
                            }

                            // Get the next item from the queue
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        // Execute the task we pulled out of the queue
                        base.TryExecuteTask(item);
                    }
                }
                // We're done processing items on the current thread
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        // Attempts to execute the specified task on the current thread. 
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining
            if (!_currentThreadIsProcessingItems) return false;

            // If the task was previously queued, remove it from the queue
            if (taskWasPreviouslyQueued)
                // Try to run the task. 
                if (TryDequeue(task))
                    return base.TryExecuteTask(task);
                else
                    return false;
            else
                return base.TryExecuteTask(task);

        }

        // Attempt to remove a previously scheduled task from the scheduler. 
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }

        // Gets the maximum concurrency level supported by this scheduler. 
        public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

        // Gets an enumerable of the tasks currently scheduled on this scheduler. 
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks;
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }
    }
    public class AyncAwaitDeep
    {
        public void ExplainAsyncAwait()
        {
            //2:创建一个IAsyncStateMachine 状态机
            //给状态机赋值
            //AsyncTaskMethodBuilder
            // private AsyncTaskMethodBuilder _builder;
            //private Task _task;
            //private int _state = -1;
            // private TaskAwaiter _awaiter;
            //  2:
            //AsyncTaskMethodBuilder 调用start 来调用状态机里面的MoveNext 实现

            //3:MoveNext 是一个状态机的实现方法 ，里面通过 TaskAwaiter的状态IsComplete来判断是否结束
            //没有结束 this.$awaiter.OnCompleted(MoveNext);继续执行状态机
            //结束之后调用this.$awaiter.Result取得结果
        }

        public TaskAwaiter GetAwaiter(int millsecond)
        {
            return Task.Delay(millsecond).GetAwaiter();
        }

        public void SimpleTaskTaskDeadLock()
        {
            //相互引用会造成死锁
            var bothTasksCreated = new TaskCompletionSource<bool>();
            Task t2 = null;
            Task t1 = Task.Run(async delegate
            {
                await bothTasksCreated.Task;
                Console.WriteLine("t1 waiting for t2…");
                // await t2; // instead of a synchronous t2.Wait() 
                t2.Wait();
                Console.WriteLine("… t1 done.");
            });
            t2 = Task.Run(async delegate
            {
                await bothTasksCreated.Task;
                Console.WriteLine("t2 waiting for t1…");
                //await t1; // instead of a synchronous t1.Wait() 
                t1.Wait();
                Console.WriteLine("… t2 done.");
            });
            bothTasksCreated.SetResult(true);
        }
    }
    public class AsyncState : IAsyncStateMachine
    {

        private AsyncTaskMethodBuilder _builder;
        private Task _task;
        private int _state = -1;
        private TaskAwaiter _awaiter;
        public void MoveNext()
        {
            switch (_state)
            {

                case 2:
                    //this.$awaiter = someObject.GetAwaiter();
                    //    if (!this.$awaiter.IsCompleted) 
                    //{
                    //        this.$state = 2;
                    //        this.$awaiter.OnCompleted(MoveNext);
                    //        return;
                    //    Label2:
                    //}
                    //    this.$awaiter.GetResult(); 
                    //… 
                    break;
            }
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            throw new NotImplementedException();
        }
    }
    public class AsyncContext
    {
        public async Task DemoAsync()
        {
            var d = new Dictionary<int, int>();
            for (int i = 0; i < 10000; i++)
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                int count;
                d[id] = d.TryGetValue(id, out count) ? count + 1 : 1;

                await Task.Yield();
            }
            foreach (var pair in d) Console.WriteLine(pair);
        }


        public void Run(Func<Task> func)
        {
            var prevCtx = SynchronizationContext.Current;
            try
            {
                var syncCtx = new SingleThreadSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(syncCtx);

                var t = func();
                t.ContinueWith(
                    delegate { syncCtx.Complete(); }, TaskScheduler.Default);

                syncCtx.RunOnCurrentThread();

                t.GetAwaiter().GetResult();
            }
            finally { SynchronizationContext.SetSynchronizationContext(prevCtx); }
        }


        /// <summary>
        /// Void 异常会直接返回SynchronizationContext上面 ，Task 会返回Task 对象上
        /// </summary>
        private async void ThrowExceptionAsync()
        {
            throw new InvalidOperationException();
        }
        public void AsyncVoidExceptions_CannotBeCaughtByCatch()
        {
            try
            {
                ThrowExceptionAsync();
            }
            catch (Exception)
            {
                // The exception is never caught here!
                throw;
            }
        }
    }
    public sealed class SingleThreadSynchronizationContext :
    SynchronizationContext
    {
        private readonly
         BlockingCollection<KeyValuePair<SendOrPostCallback, object>>
          m_queue =
           new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

        public override void Post(SendOrPostCallback d, object state)
        {
            m_queue.Add(
                new KeyValuePair<SendOrPostCallback, object>(d, state));
        }

        public void RunOnCurrentThread()
        {
            KeyValuePair<SendOrPostCallback, object> workItem;
            while (m_queue.TryTake(out workItem, Timeout.Infinite))
                workItem.Key(workItem.Value);
        }

        public void Complete() { m_queue.CompleteAdding(); }
    }
    public class ThreadDeep
    {
    }
}





