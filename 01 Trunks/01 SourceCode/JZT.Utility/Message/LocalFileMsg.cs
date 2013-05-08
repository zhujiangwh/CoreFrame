using System;
using System.Collections.Generic;
using System.Text;
using JZT.Utility.ThreadPool;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace JZT.Utility.Message
{
    public class LocalFileMsg : WorkItem, IChannel
    {
        #region IChannel 成员

        public object Content
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Receive
        {
            get;
            set;
        }

        #endregion

        public override void Perform()
        {
            Logger.Write(Convert.ToString(Content),"Common Event");
        }
    }
}
