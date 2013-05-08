using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Authority
{
    [Serializable]
    public class Function
    {
        public event EventHandler OnExecute;

        public event EventHandler OnBeforeExecute;

        public event EventHandler OnAfterExecute;

        public virtual string FunctionID { get; set; }

        public virtual string FunctionName { get; set; }

        public virtual bool Execute()
        {
            bool flag = true;

            //取此用户的登录ID
            RunLog runLog = new RunLog("");

            string msg = string.Empty;

            try
            {
                runLog.BeginExe();

                if (OnBeforeExecute != null)
                { }

                //检验该用户是否有权限 。


                //执行操作。

                if (OnExecute != null)
                {
                }
                else
                {
                }

                if (OnAfterExecute != null)
                {
                }
            }
            catch (Exception ex)
            {
                //记录日志。
                msg = ex.ToString();
            }

            finally
            {

                //保存运行日志
                runLog.EndExe();
                runLog.Msg = msg;
            }



            return flag;
        }


    }
}
