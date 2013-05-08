using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Authority
{
    [Serializable]
    public class OnlineUser
    {
        public virtual string UserID { get; set; }
        public virtual DateTime LoginTime { get; set; }

        public virtual DateTime LogoutTime { get; set; }

        public virtual string LoginKey { get; set; }

        public virtual IList<RunLog> RunLogList { get; set; }

        public OnlineUser()
        {
        }

        public  OnlineUser(string userID)
        {
            UserID = userID;
            LoginTime = DateTime.Now;

            LoginKey = Guid.NewGuid().ToString();

            RunLogList = new List<RunLog>();
        }

        public virtual void Login()
        {
            LoginTime = DateTime.Now;
        }


        public virtual void Logout()
        {
            LogoutTime = DateTime.Now;
        }

        public virtual  RunLog CreateRunLog()
        {
            RunLog runLog = new RunLog(LoginKey);

            runLog.FunctionCode = "sdf";
            runLog.AppServerID = "127.0.0.1";
            runLog.BeginExe();

            return runLog;
        }
    }
}
