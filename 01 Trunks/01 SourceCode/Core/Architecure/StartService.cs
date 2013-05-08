using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

namespace Core.Architecure
{
    public interface IStartItemDisplayer
    {
        void DisplayText(string text);
    }

    #region 定义系统启动项。

    public class BaseStartItem
    {
        public string Text { get; set; }

        public event EventHandler OnBeginExecute;

        public event EventHandler OnEndExecute;


    
        public virtual void Execute()
        { 
        }
    }

    public class RemotingStart : BaseStartItem
    {
        public RemotingStart()
        {
            Text = "创建Remoting服务";
        }

        public override void Execute()
        {
            base.Execute();

            RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            Thread.Sleep(5000);

        }
 
    }

    public class SleepStart : BaseStartItem
    {
        public SleepStart()
        {
            Text = "睡眠 3 秒 ";
        }

        public override void Execute()
        {
            base.Execute();

            Thread.Sleep(3000);
        }

    }


    #endregion

    /// <summary>
    /// 应用程序启动服务。
    /// </summary>
    /// 
    [Serializable]
    public class StartService
    {

        public bool IsDisplayFlashForm { get; set; }
        public ObjectDefine StartItemDisplayerDefine { get; set; }

        public IStartItemDisplayer StartItemDisplayer { get; set; }

        public List<BaseStartItem> StartItemList { get; set; }

        public StartService()
        {
            StartItemDisplayerDefine = new ObjectDefine();

            StartItemList = new List<BaseStartItem>();

            SleepStart SleepStart1 = new SleepStart();
            RemotingStart RemotingStart = new RemotingStart();
            SleepStart sleepStart2 = new SleepStart();

            StartItemList.Add(SleepStart1);
            StartItemList.Add(RemotingStart);
            StartItemList.Add(sleepStart2);

        }

        public Form SplashForm;

        public void Initial()
        {
            StartItemDisplayerDefine.AssemblyName = "Server";
            StartItemDisplayerDefine.FullClassName = "Server.SplashForm";

                IsDisplayFlashForm = true;

            if (IsDisplayFlashForm)
            {
                //构建 Form 。
                StartItemDisplayer = StartItemDisplayerDefine.CreateObject() as IStartItemDisplayer;

                //构建每个启动项，将每 个启动项 装载进 启动服务中 。
                SplashForm = StartItemDisplayer as Form;
            }

            
        }

        /// <summary>
        /// 执行启动。
        /// </summary>
        public void Execute()
        {
            if (IsDisplayFlashForm && SplashForm != null)
            {
                SplashForm.Show();
            }
            // 初始化进度 。

            foreach (BaseStartItem item in StartItemList)
            {
                if (IsDisplayFlashForm && SplashForm != null)
                {
                    StartItemDisplayer.DisplayText(item.Text);
                }
                item.Execute();
                //更新进度
            }

            if (IsDisplayFlashForm && SplashForm != null)
            {
                //结束进度。
                SplashForm.Close();
            }
        }
    }
}
