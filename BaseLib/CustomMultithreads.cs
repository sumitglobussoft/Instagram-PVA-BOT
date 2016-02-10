using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;

namespace BaseLib
{
    public class CustomMultithreads
    {
        public string poolName { get; set; }

        public readonly object lockerManageThreads = new object();

        Func<object, string> myMethodName;

        private bool isCounterIncremented = false;



        //public bool isStopCalled = false;
        public static bool isStopCalled = false;

        private readonly object lockerisCounterIncremented = new object();

        private List<Thread> listThreads = new List<Thread>();
        //private static List<Thread> listThreads = new List<Thread>();

        int maxNoOfThreads = 10;

        int counterWorkerThreads = 0;

        //public Events loggingEvents = new Events();
        public Events processCompletedEvent = new Events();
        public Events processCompletedEventAllParsers = new Events();
        public Events threadCompletedEvent = new Events();
        public Events threadStartedEvent = new Events();

        public bool isProcessAccomplished = false;

        public int timeForCheckingProcessAccomplished = 15000;

        public int maxNoOfThreadsInPool = 50;

        public int counterPoolThreads = 0;

        public CustomMultithreads(Func<object, string> myMethodName, int maxNoOfThreads)
        {
            try
            {
                this.myMethodName = myMethodName;
                this.maxNoOfThreads = maxNoOfThreads;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        public CustomMultithreads(Func<object, string> myMethodName, int maxNoOfThreads, int maxNoOfThreadsInPool)
        {
            try
            {
                this.myMethodName = myMethodName;
                this.maxNoOfThreads = maxNoOfThreads;
                this.maxNoOfThreadsInPool = maxNoOfThreadsInPool;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        public CustomMultithreads(Func<object, string> myMethodName, int maxNoOfThreads, int maxNoOfThreadsInPool, string poolName)
        {
            try
            {
                this.myMethodName = myMethodName;
                this.maxNoOfThreads = maxNoOfThreads;
                this.maxNoOfThreadsInPool = maxNoOfThreadsInPool;
                this.poolName = poolName;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        public void ThreadMethod(object parameters)
        {
            string strReturn = "";
            try
            {
                //Interlocked.Increment(ref counterWorkerThreads);
                Interlocked.Increment(ref counterWorkerThreads);
                ThreadStarted();

                if (!isStopCalled)
                {
                    SetTrueIsCounterIncrementedThreadSafe();

                    listThreads.Add(Thread.CurrentThread);

                    Console.WriteLine("Worker Threads : " + counterWorkerThreads);
                    Log("Worker Threads : " + counterWorkerThreads);

                    Console.WriteLine("Thread Pool Name: " + poolName);
                    Log("Thread Pool Name: " + poolName);

                    Log("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " Started");
                    strReturn = myMethodName(parameters); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Interlocked.Decrement(ref counterWorkerThreads);
                //Interlocked.Decrement(ref base.counterWorkerThreadsInPool);
                ThreadCompleted();

                lock (lockerManageThreads)
                {
                    Monitor.Pulse(lockerManageThreads);
                }

                if (counterWorkerThreads == 0)
                {
                    SetFalseIsCounterIncrementedThreadSafe();
                    Thread.Sleep(timeForCheckingProcessAccomplished);//Thread.Sleep(3000);
                    if (counterWorkerThreads == 0 && !isCounterIncremented)
                    {
                        Console.WriteLine("CustomMultithreads Process Accomplished");
                        isProcessAccomplished = true;
                        ProcessCompleted(strReturn);
                        Log("Process Accomplished");
                    }
                }
                Log("Thread ID: " + Thread.CurrentThread.ManagedThreadId + " Completed");
            }
        }

        public void StartMultithreadedMethods(ICollection arrayparameters)
        {
            try
            {
                foreach (object item in arrayparameters)
                {
                    try
                    {
                        if (!isStopCalled)
                        {
                            lock (lockerManageThreads)
                            {
                                if (counterWorkerThreads >= maxNoOfThreads)
                                {
                                    lock (lockerManageThreads)
                                    {
                                        Monitor.Wait(lockerManageThreads);
                                    }
                                }
                            }

                            if (!isStopCalled)
                            {
                                Thread threadThreadMethod = new Thread(ThreadMethod);
                                threadThreadMethod.IsBackground = true;
                                threadThreadMethod.Start(item);
                                Interlocked.Increment(ref counterWorkerThreads);
                                //ThreadStarted();
                                //Interlocked.Increment(ref base.counterWorkerThreadsInPool);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        public void StartSingleMultithreadedMethod(object item)
        {
            try
            {
                //foreach (object item in arrayparameters)
                {
                    if (!isStopCalled)
                    {
                        lock (lockerManageThreads)
                        {
                            if (counterWorkerThreads >= maxNoOfThreads)// || base.counterWorkerThreadsInPool >= base.maxNoOfThreadsInPool)//if (counterWorkerThreads >= maxNoOfThreads)
                            {
                                lock (lockerManageThreads)
                                {
                                    Monitor.Wait(lockerManageThreads);
                                }
                            }
                        }

                        if (!isStopCalled)
                        {
                            Thread threadThreadMethod = new Thread(ThreadMethod);
                            threadThreadMethod.IsBackground = true;
                            threadThreadMethod.Start(item);
                            //Interlocked.Increment(ref counterWorkerThreads);
                            //Interlocked.Increment(ref base.counterWorkerThreadsInPool);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        public void StopMultithreadedMethods()
        {
            try
            {
                isStopCalled = true;

                for (int i = 0; i < 2; i++)
                {
                    List<Thread> temp = new List<Thread>();
                    temp.AddRange(listThreads);

                    foreach (Thread thread in temp)
                    {
                        try
                        {
                            thread.Abort();
                            listThreads.Remove(thread);
                        }
                        catch (Exception ex)
                        {
                            GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

                        }
                    }
                    Thread.Sleep(1000);
                }
                Log("Threads Stopped");

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        public int WorkerThreadsCount
        {
            get
            {
                return counterWorkerThreads;
            }
        }

        private void SetFalseIsCounterIncrementedThreadSafe()
        {
            try
            {
                lock (lockerisCounterIncremented)
                {
                    isCounterIncremented = false;
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        private void SetTrueIsCounterIncrementedThreadSafe()
        {
            try
            {
                lock (lockerisCounterIncremented)
                {
                    isCounterIncremented = true;
                }
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        private void Log(string log)
        {
            try
            {
                //EventsArgs eArgs = new EventsArgs(log);
                //loggingEvents.LogText(eArgs);

                Console.WriteLine(log);
                Console.WriteLine(log);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        private void ProcessCompleted(string strReturn)
        {
            try
            {
                try
                {
                    EventsArgs eArgs = new EventsArgs();
                    processCompletedEvent.RaiseProcessCompletedEvent(eArgs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.StackTrace);
                }

                try
                {
                    EventsArgs eArgs1 = new EventsArgs(strReturn);
                    processCompletedEventAllParsers.RaiseProcessCompletedEvent(eArgs1);
                }
                catch (Exception ex)
                {
                    GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);
                }

                processCompletedEvent.UnSubscribeProcessCompletedHandler();


                threadStartedEvent.UnSubscribeProcessCompletedHandler();
                threadCompletedEvent.UnSubscribeProcessCompletedHandler();

            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        private void ThreadCompleted()
        {
            try
            {
                EventsArgs eArgs = new EventsArgs();
                threadCompletedEvent.RaiseProcessCompletedEvent(eArgs);

                //threadStartedEvent.UnSubscribeProcessCompletedHandler();
                //threadCompletedEvent.UnSubscribeProcessCompletedHandler();
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        private void ThreadStarted()
        {
            try
            {
                EventsArgs eArgs = new EventsArgs();
                threadStartedEvent.RaiseProcessCompletedEvent(eArgs);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }
       
    }

 
}
