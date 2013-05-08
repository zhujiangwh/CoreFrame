using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;

/*
进程之间通讯的几种方法: 
在Windows程序中，各个进程之间常常需要交换数据，进行数据通讯。常用的方法有  
(1)使用内存映射文件  
(2)通过共享内存DLL共享内存  
(3)使用SendMessage向另一进程发送WM_COPYDATA消息  
 
比起前两种的复杂实现来,WM_COPYDATA消息无疑是一种经济实惠的一种方法.  
WM_COPYDATA消息的主要目的是允许在进程间传递只读数据。Windows在通过WM_COPYDATA消息传递期间，不提供继承同步方式。
SDK文档推荐用户使用SendMessage函数，接受方在数据拷贝完成前不返回，这样发送方就不可能删除和修改数据：  
 
这个函数的原型及其要用到的结构如下:  
SendMessage(hwnd, WM_COPYDATA, wParam, lParam);    
其中:
WM_COPYDATA对应的十六进制数为0x004A  
wParam设置为包含数据的窗口的句柄。
lParam指向一个COPYDATASTRUCT的结构：  
typedef  struct  tagCOPYDATASTRUCT
{  
         DWORD  dwData;  //用户定义数据  
         DWORD  cbData;  //数据大小  
         PVOID  lpData;  //指向数据的指针  
} COPYDATASTRUCT;  
该结构用来定义用户数据。  
 
具体过程如下:  
首先,在发送方，用FindWindow找到接受方的句柄，然后向接受方发送WM_COPYDATA消息。
接受方在DefWndProc事件中处理这条消息。由于中文编码是两个字节, 所以传递中文时候字节长度要搞清楚。  
protected  override  void  DefWndProc(ref  System.Windows.Forms.Message  m)  {  
 switch(m.Msg)  {  
     case  WinMessageUtil.WM_COPYDATA:  
           string str = WinMessageUtil.ReceiveMessage(ref m);
           break;
     default:  
           break;  
 }
 base.DefWndProc(ref  m);  
}  
*/

namespace JZT.Utility
{
    /// <summary>
    /// 本类封装了一些进程间通讯的细节
    /// </summary>
    public class WinMessageUtils
    {


        ///////////////////////////

        [DllImport("user32.dll")]
        static extern IntPtr GetTopWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, UInt32 uCmd);

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private static readonly UInt32 GW_HWNDNEXT = 2;



        //////////////////////////

        /// <summary>
        /// 
        /// </summary>
        public const int WM_COPYDATA = 0x004A;

        /////////////////
        //WM_COPYDATA消息所要求的数据结构
        public struct CopyDataStruct
        {
            public IntPtr dwData;
            public int cbData;

            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        //通过窗口的标题来查找窗口的句柄
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        //在DLL库中的发送消息函数
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage
            (
            int hWnd,                        // 目标窗口的句柄  
            int Msg,                        // 在这里是WM_COPYDATA
            int wParam,                    // 第一个消息参数
            ref  CopyDataStruct lParam        // 第二个消息参数
            );

        /// <summary>
        /// 进程间发送消息，只能传递一个自定义的消息ID和消息字符串
        /// </summary>
        /// <param name="destProcessName">目标进程名称，如果有多个，则给每个都发送</param>
        /// <param name="formTitle">窗体包含文字</param>
        /// <param name="msgID">自定义数据，可以通过这个来决定如何解析下面的strMsg</param>
        /// <param name="strMsg">传递的消息，是一个字符串</param>
        /// <returns>发送成功的进程数量</returns>
        public static int SendMessage(string destProcessName,string formTitle, int msgID, string strMsg)
        { 
            if (strMsg == null)
                return 0;

            //按进程名称查找，同名称的进程可能有许多，所以返回的是一个数组
            Process[] foundProcess = Process.GetProcessesByName(destProcessName);
            int isSendCount = 0; 
            foreach (Process p in foundProcess)
            {
                if (p.MainWindowHandle.ToInt32() != 0)
                {
                    // 如果 formTitle 不为空，要按formTitle查找
                    if (!String.IsNullOrEmpty(formTitle))
                    {
                        if (String.IsNullOrEmpty(p.MainWindowTitle) ||
                            p.MainWindowTitle.ToUpper().IndexOf(formTitle.ToUpper()) < 0)
                        {
                            continue;
                        }
                    }
                }
                // 不给当前进程发消息
                if (Process.GetCurrentProcess().Id == p.Id)
                    continue;

                int toWindowHandler = GetWnd(p, formTitle);//.MainWindowHandle.ToInt32();
                if (toWindowHandler != 0)
                {
                    CopyDataStruct cds;
                    cds.dwData = (IntPtr)msgID;   //这里可以传入一些自定义的数据，但只能是4字节整数      
                    cds.lpData = strMsg;            //消息字符串
                    cds.cbData = System.Text.Encoding.Default.GetBytes(strMsg).Length + 1;  //注意，这里的长度是按字节来算的

                    //发送方的窗口的句柄, 由于本系统中的接收方不关心是该消息是从哪个窗口发出的，所以就直接填0了
                    int fromWindowHandler = 0;
                    SendMessage(toWindowHandler, WM_COPYDATA, fromWindowHandler, ref  cds);
                    isSendCount++;
                }
            }

            return isSendCount;
        }

        static int GetWnd(Process p, String formTitle)
        {

            int toWindowHandler = p.MainWindowHandle.ToInt32();

            if (toWindowHandler != 0)
                return toWindowHandler;

            IntPtr h = GetTopWindow(IntPtr.Zero);
            while (h != IntPtr.Zero)
            {
                UInt32 newID;
                GetWindowThreadProcessId(h, out newID);
                if (newID == p.Id)
                {
                    StringBuilder sbClassName = new StringBuilder(200);
                    StringBuilder sbText = new StringBuilder(200);

                    GetClassName(h, sbClassName, 200);
                    GetWindowText(h, sbText, 200);
                    //if (sbClassName.ToString().IndexOf(className, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                    //    sbText.ToString().IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    //{
                    String caption = sbText.ToString();
                    if (caption.EndsWith(formTitle))
                    {
                        break;
                    }
                    //}
                }

                h = GetWindow(h, GW_HWNDNEXT);
            }

            return h.ToInt32();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="destProcessName"></param>
        /// <param name="formTitle"></param>
        /// <returns></returns>
        public static Process[] FindProcess(string destProcessName, string formTitle)
        {
            List<Process> processList = new List<Process>();
            //按进程名称查找，同名称的进程可能有许多，所以返回的是一个数组
            Process[] foundProcess = Process.GetProcessesByName(destProcessName);

            foreach (Process p in foundProcess)
            {
                // 如果 formTitle 不为空，要按formTitle查找
                if (!String.IsNullOrEmpty(formTitle))
                {
                    if (String.IsNullOrEmpty(p.MainWindowTitle) ||
                        p.MainWindowTitle.ToUpper().IndexOf(formTitle.ToUpper()) < 0)
                    {
                        continue;
                    }
                }
                // 不给当前进程发消息
                if (Process.GetCurrentProcess().Id == p.Id)
                    continue;

                processList.Add(p);
            }

            return processList.ToArray();
        }

        /// <summary>
        /// 进程间发送消息，只能传递一个自定义的消息ID和消息字符串
        /// </summary>
        /// <param name="destProcessName">目标进程名称，如果有多个，则给每个都发送</param>
        /// <param name="msgID">自定义数据，可以通过这个来决定如何解析下面的strMsg</param>
        /// <param name="strMsg">传递的消息，是一个字符串</param>
        /// <returns>发送成功的进程数量</returns>
        public static int SendMessage(string destProcessName, int msgID, string strMsg)
        {
            return SendMessage(destProcessName, null, msgID, strMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="msgID"></param>
        /// <param name="strMsg"></param>
        public static void SendMessage(Process process, int msgID, string strMsg)
        {
            int toWindowHandler = process.MainWindowHandle.ToInt32();
            if (toWindowHandler != 0)
            {
                CopyDataStruct cds;
                cds.dwData = (IntPtr)msgID;   //这里可以传入一些自定义的数据，但只能是4字节整数      
                cds.lpData = strMsg;            //消息字符串
                cds.cbData = System.Text.Encoding.Default.GetBytes(strMsg).Length + 1;  //注意，这里的长度是按字节来算的

                //发送方的窗口的句柄, 由于本系统中的接收方不关心是该消息是从哪个窗口发出的，所以就直接填0了
                int fromWindowHandler = 0;
                SendMessage(toWindowHandler, WM_COPYDATA, fromWindowHandler, ref  cds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        /// <param name="msgID"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SendObject(Process process, int msgID, object obj)
        {
            byte[] data = Serialize(obj);
            string str = Convert.ToBase64String(data, 0, data.Length);
            SendMessage(process, msgID, str);
        }

        /// <summary>
        /// 进程间发送对象
        /// </summary>
        /// <param name="destProcessName">目标进程名称</param>
        /// <param name="msgID"></param>
        /// <param name="obj">可序列化的对象</param>
        /// <returns>发送成功的进程数量</returns>
        public static int SendObject(string destProcessName, int msgID, object obj)
        {
            byte[] data = Serialize(obj);
            string str = Convert.ToBase64String(data, 0, data.Length);
            return SendMessage(destProcessName, msgID, str);
        }

        /// <summary>
        /// 进程间发送对象
        /// </summary>
        /// <param name="destProcessName">目标进程名称</param>
        /// <param name="formTitle">窗体包含文字</param>
        /// <param name="msgID"></param>
        /// <param name="obj">可序列化的对象</param>
        /// <returns>发送成功的进程数量</returns>
        public static int SendObject(string destProcessName, string formTitle, int msgID, object obj)
        {
            byte[] data = Serialize(obj);
            string str = Convert.ToBase64String(data, 0, data.Length);
            return SendMessage(destProcessName,formTitle, msgID, str);
        }

        /// <summary>
        /// 接收消息，得到消息字符串
        /// </summary>
        /// <param name="m">System.Windows.Forms.Message m</param>
        /// <returns>接收到的消息字符串</returns>
        public static string ReceiveMessage(ref  System.Windows.Forms.Message m)
        {
            CopyDataStruct cds = (CopyDataStruct)m.GetLParam(typeof(CopyDataStruct));
            return cds.lpData;
        }

        /// <summary>
        /// 接收对象
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Object ReceiveObject(ref  System.Windows.Forms.Message m)
        {
            String base64String = ReceiveMessage(ref m);
            return Deserialize(Convert.FromBase64String(base64String));
        }

        /// <summary>
        /// 通过指定应用程序的名称和一组命令行参数来启动一个进程资源，并将该资源与新的 System.Diagnostics.Process 组件相关联。
        /// </summary>
        /// <param name="fileName">要在该进程中运行的应用程序文件的名称。</param>
        /// <param name="msgID">消息ID（命令行参数1）</param>
        /// <param name="arguments">启动该进程时传递的命令行参数（可序列对象，命令行参数2）</param>
        /// <returns>与进程资源关联的新的 System.Diagnostics.Process 组件，或者如果没有启动进程资源（例如，如果重用了现有进程），则为 null。</returns>
        /// <exception cref="System.InvalidOperationException">fileName 或 arguments 参数为 null。</exception>
        /// <exception cref="System.ComponentModel.Win32Exception">打开关联的文件时发生了错误。</exception>
        /// <exception cref="System.ObjectDisposedException"> 该进程对象已被释放。</exception>
        /// <exception cref="System.IO.FileNotFoundException">PATH 环境变量有包含引号的字符串。</exception>
        public static Process RunProcess(string fileName, int msgID, Object arguments)
        {
            Process proc = null;

            if (arguments is String)
            {
                proc = Process.Start(fileName, CreateArguments(msgID.ToString() + " " + arguments.ToString()));
            }
            else
            {
                byte[] data = Serialize(arguments);
                string str = Convert.ToBase64String(data, 0, data.Length);
                proc = Process.Start(fileName, CreateArguments(msgID.ToString() + " " + str));
            }
            return proc;
        }


        /// <summary>
        /// 通过指定应用程序的名称、一组命令行参数、用户名、密码和域来启动一个进程资源，并将该资源与新的 System.Diagnostics.Process
        /// 组件关联起来。
        /// </summary>
        /// <param name="fileName">要在该进程中运行的应用程序文件的名称。</param>
        /// <param name="msgID">消息ID（命令行参数1）</param>
        /// <param name="arguments">启动该进程时传递的命令行参数（可序列对象，命令行参数2）</param>
        /// <param name="userName">启动进程时要使用的用户名。</param>
        /// <param name="password">一个 System.Security.SecureString，它包含启动进程时要使用的密码。</param>
        /// <param name="domain">启动进程时要使用的域。</param>
        /// <returns>与进程资源关联的新的 System.Diagnostics.Process 组件，或者如果没有启动进程资源（例如，如果重用了现有进程），则为 null。</returns>
        /// <exception cref="System.InvalidOperationException">未指定文件名。</exception>
        /// <exception cref="System.ComponentModel.Win32Exception">fileName 不是可执行 (.exe) 文件。</exception>
        /// <exception cref="System.ComponentModel.Win32Exception">打开关联的文件时发生了错误。</exception>
        /// <exception cref="System.ObjectDisposedException">该进程对象已被释放。</exception>
        public static Process RunProcess(string fileName, int msgID, Object arguments, string userName, SecureString password, string domain)
        {
            Process proc = null;

            if (arguments is String)
            {
                proc = Process.Start(fileName, CreateArguments(msgID.ToString() + " " + arguments.ToString()));
            }
            else
            {
                byte[] data = Serialize(arguments);
                string str = Convert.ToBase64String(data, 0, data.Length);
                proc = Process.Start(fileName, CreateArguments(msgID.ToString() + " " + str), userName, password, domain);
            }
            return proc;
        }

        //private static int SplitMaxLength = 1024;

        private static String CreateArguments(String arguments)
        {
            if (arguments.Length > 2080)
            {
                throw new ApplicationException("arguments 参数长度不能超过 2080");
            }

            return arguments;
            //String[] args = SplitString(arguments);
            //StringBuilder sb = new StringBuilder();
            //foreach (String arg in args)
            //{
            //    sb.Append(arg + " ");
            //}
            //return sb.ToString();
        }

        //private static String[] SplitString(String arguments)
        //{
        //    if (arguments.Length > SplitMaxLength)
        //    {
        //        List<String> strList = new List<String>();
        //        for (int i = 0; i < 8; i++)
        //        {
        //            int start = i * SplitMaxLength;
        //            int len = SplitMaxLength;

        //            if (start + len > arguments.Length)
        //            {
        //                len = arguments.Length - start;
        //            }
        //            if (len == 0)
        //                break;
        //            strList.Add(arguments.Substring(start, len));
        //            if (len < SplitMaxLength)
        //                break;
        //        }
        //        return strList.ToArray();
        //    }
        //    else
        //    {
        //        return new String[] { arguments };
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static Object ReadObject(String base64String)
        {
            return Deserialize(Convert.FromBase64String(base64String));
        }

        /// <summary>
        /// 序列化成二进制
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static byte[] Serialize(Object obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(ms, obj);
            return ms.GetBuffer();
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static Object Deserialize(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return binaryFormatter.Deserialize(ms);
        }
    }
}
