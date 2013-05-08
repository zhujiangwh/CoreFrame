using System;
using System.Collections.Generic;
using System.Text;
using JZT.Utility.ThreadPool;
namespace JZT.Utility.Message
{
    internal class MessageWorkQueue : WorkQueue
    {

        public const string SENDAMS = "AMS";
        public const string SENDEMAIL = "EMALI";
        public const string SENDDIALOG = "DIALOG";


        public static readonly MessageWorkQueue Instance = new MessageWorkQueue();

        private static object sync = new object();

        static MessageWorkQueue()
        {
            Instance.ConcurrentLimit = 20; //限制并发数
            ((WorkThreadPool)Instance.WorkerPool).MinThreads = 1; //最小线程数
            ((WorkThreadPool)Instance.WorkerPool).MaxThreads = 10; //最大线程数
            Instance.AllWorkCompleted += new EventHandler(Instance_AllWorkCompleted);
            Instance.WorkerException += new ResourceExceptionEventHandler(Instance_WorkerException);
            Instance.ChangedWorkItemState += new ChangedWorkItemStateEventHandler(Instance_ChangedWorkItemState);


        }

        static void Instance_ChangedWorkItemState(object sender, ChangedWorkItemStateEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void Instance_WorkerException(object sender, ResourceExceptionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        static void Instance_AllWorkCompleted(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void AddMessageToPool(IWorkItem item)
        {
            if (item != null)
                base.Add(item);
        }
    
    }
}
