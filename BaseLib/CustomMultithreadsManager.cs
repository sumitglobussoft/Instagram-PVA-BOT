using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BaseLib
{
    public class CustomMultithreadsManager
    {
        //Manages pools of threads for multithreading processes of same class

        public string poolName { get; set; }

        public int maxNoOfThreadsInPool = 50;

        public int counterWorkerThreadsInPool = 0;

        public readonly object lockr_PoolThreads = new object();

        //public static List<CustomMultithreadsManager> lst = new List<CustomMultithreadsManager>();

        public static List<CustomMultithreads> lstCustomMultiThreads = new List<CustomMultithreads>();

        public CustomMultithreadsManager()
        {
            try
            {
                //CustomMultithreadManagerDictionary.countinstances_CustomMultithreadsManager++;
                //lst.Add(this);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        //int maxNoOfThreads = 10;
        public CustomMultithreads GetCustomMultithreadsInstance(Func<object, string> myMethodName, int maxNoOfThreads)
        {
            CustomMultithreads cms = new CustomMultithreads(myMethodName, maxNoOfThreads, maxNoOfThreadsInPool);
            //lstCustomMultiThreads.Add(cms);
            return cms;

        }

        public CustomMultithreads GetCustomMultithreadsInstance(Func<object, string> myMethodName, int maxNoOfThreads, int maxNoOfThreadsInPool, string poolName)
        {
            this.maxNoOfThreadsInPool = maxNoOfThreadsInPool;
            CustomMultithreads cmt = new CustomMultithreads(myMethodName, maxNoOfThreads, maxNoOfThreadsInPool, poolName);
            lstCustomMultiThreads.Add(cmt);
            //cmt.threadStartedEvent.processCompletedEvent += new EventHandler(threadStartedEvent_processCompletedEvent);
            //cmt.threadCompletedEvent.processCompletedEvent += new EventHandler(threadCompletedEvent_processCompletedEvent);
            cmt.threadStartedEvent.SubscribeProcessCompletedHandler(new EventHandler(threadStartedEvent_processCompletedEvent));
            cmt.threadCompletedEvent.SubscribeProcessCompletedHandler(new EventHandler(threadCompletedEvent_processCompletedEvent));
            return cmt;
        }

        public void CallMultithreadedMethod(CustomMultithreads cmt, object item)
        {
            try
            {
                //cmt.threadStartedEvent.processCompletedEvent += new EventHandler(threadStartedEvent_processCompletedEvent);
                //cmt.threadCompletedEvent.processCompletedEvent += new EventHandler(threadCompletedEvent_processCompletedEvent);
                lock (lockr_PoolThreads)
                {
                    if (counterWorkerThreadsInPool >= maxNoOfThreadsInPool)
                    {
                        lock (lockr_PoolThreads)
                        {
                            Monitor.Wait(lockr_PoolThreads);

                            if (counterWorkerThreadsInPool >= maxNoOfThreadsInPool)
                            {
                                Monitor.Wait(lockr_PoolThreads);
                            }
                        }
                    }
                }
                cmt.StartSingleMultithreadedMethod(item);

               
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }


        }

        void threadStartedEvent_processCompletedEvent(object sender, EventArgs e)
        {
            try
            {
                //lock (lockr_PoolThreads)
                //{
                    Interlocked.Increment(ref counterWorkerThreadsInPool);
                //}
                Console.WriteLine("counterWorkerThreadsInPool: " + counterWorkerThreadsInPool);
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        void threadCompletedEvent_processCompletedEvent(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("counterWorkerThreadsInPool: " + counterWorkerThreadsInPool);
                Interlocked.Decrement(ref counterWorkerThreadsInPool);
                lock (lockr_PoolThreads)
                {
                    //Interlocked.Decrement(ref counterWorkerThreadsInPool);
                    if (!(counterWorkerThreadsInPool >= maxNoOfThreadsInPool))
                    {
                        Monitor.Pulse(lockr_PoolThreads);
                    }
                }

                //CustomMultithreads cms = (CustomMultithreads)sender;
                //cms.threadStartedEvent.processCompletedEvent -= threadStartedEvent_processCompletedEvent;
                //cms.threadCompletedEvent.processCompletedEvent -= threadCompletedEvent_processCompletedEvent;
            }
            catch (Exception ex)
            {
                GlobusLogHelper.log.Error(" Error : " + ex.StackTrace);

            }
        }

        public static void StopAllCustomMultithreadedMethods()
        {
            try
            {
                foreach (CustomMultithreads cm in lstCustomMultiThreads)
                {
                    try
                    {
                        cm.StopMultithreadedMethods();
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
    }
}
