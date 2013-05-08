using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Core.DB;
using System.Runtime.Remoting;

namespace Core.Architecure
{
    [Serializable]
    public class WinApplication
    {
        #region singleton

        private static WinApplication instance;

        private static string configFileName = "application.xml";

        public static WinApplication GetInstance()
        {
            if (instance == null)
            {
                instance = DeserializeXml(configFileName, typeof(WinApplication)) as WinApplication;
            }
            return instance;
        }

        public static object DeserializeXml(string fileName, Type tp)
        {
            if (!File.Exists(fileName)) return null;
            XmlSerializer ser = new XmlSerializer(tp);
            object o = null;
            try
            {
                using (StreamReader read = new StreamReader(fileName))
                {
                    o = ser.Deserialize(read);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return o;
        }

        /// <summary>
        /// 序列化对象到XML 文件
        /// </summary>
        /// <param name="objectToConvert"></param>
        /// <param name="path"></param>
        public static void SerializeXML(object objectToConvert, string path)
        {
            if (objectToConvert != null)
            {
                Type t = objectToConvert.GetType();

                XmlSerializer ser = new XmlSerializer(t);

                using (StreamWriter writer = new StreamWriter(path))
                {
                    ser.Serialize(writer, objectToConvert);
                    writer.Close();
                }
            }

        }


        private WinApplication()
        {
            MainFormDefine = new ObjectDefine();
            SplashFormDefine = new ObjectDefine();

            DBServerManager = new DBServerManager();

            RemotingServer = new RemotingServer("d", "d");

            IsUseRemoting = true;

            IsDisplayFlashForm = true;

        }

        #endregion


        public virtual string Caption { get; set; }

        public ObjectDefine MainFormDefine { get; set; }

        public ObjectDefine SplashFormDefine { get; set; }


        public DBServerManager DBServerManager { get; set; }

        public RemotingServer RemotingServer { get; set; }

        public bool IsUseRemoting { get; set; }

        public bool IsDisplayFlashForm { get; set; }


        //public WinApplication()
        //{
        //    Caption = "cap";


        //    MainFormDefine = new ObjectDefine();
        //    MainFormDefine.AssemblyName= "WinUI";
        //    MainFormDefine.FullClassName = "WinUI.MainForm";

        //    DBServerManager = new DBServerManager();
        //}

        [XmlIgnore]
        public StartService StartService { get; set; }

        public void Run()
        {
            //系统初始化。
            Initial();



            //显示登录.



            //显示主窗口.

            Form main = MainFormDefine.CreateObject() as Form;

            //设置窗口标题等.
            main.Text = Caption;

            Application.Run(main);

            //关闭系统。
            Close();
        }

        public void Initial()
        {
            //if (IsDisplayFlashForm)
            //{
            //    Form main = SplashFormDefine.CreateObject() as Form;

            //    main.ShowDialog();

            //}

            //if (IsUseRemoting)
            //{
            //    RemotingConfiguration.Configure(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            //}

            //显示FlashForm 



            //启动初始经化
            StartService = new StartService();

            StartService.Initial();

            StartService.Execute();



        }


        public void Close()
        {
            //关闭系统。
            SerializeXML(instance, configFileName);

        }
    }
}
