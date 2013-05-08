using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Authority
{
    [Serializable]
    public class RunLog
    {
        public virtual string PKey { get; set; }

        public virtual string LoginKey { get; set; }

        public virtual string FunctionCode { get; set; }

        public virtual DateTime BeginTime { get; set; }

        public virtual DateTime EndTime { get; set; }

        public virtual string Msg { get; set; }

        public virtual string AppServerID { get; set; }


        public RunLog()
        {
            PKey = Guid.NewGuid().ToString();
            Msg = "运行成功";
        }

        public RunLog(string loginKey)
        {
            PKey = Guid.NewGuid().ToString();
            LoginKey = loginKey;
            Msg = "运行成功";
        }

        public virtual void BeginExe()
        {
            BeginTime = DateTime.Now;
        }

        public virtual void EndExe()
        {
            EndTime = DateTime.Now;
        }


    }
}
