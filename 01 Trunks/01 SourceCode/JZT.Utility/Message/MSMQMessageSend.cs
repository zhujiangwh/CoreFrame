using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Messaging;



namespace JZT.Utility.Message
{
    public class MSMQMessageSend :JZT.Utility.ThreadPool.WorkItem, JZT.Utility.Message.IMessage
    {
        static string ServerMSMQIP = "";
        static string ServerMSMQPath = string.Empty;
        static string LocalServerMSMQName = "reqchannel";
        static string ClientMSMQPath = @".\private$\rspchannel";
        static Thread threadReceive;
        static private MessageQueue mqClientListen;
        static private MessageQueue mqServerListen;


        object SendMessage;

        public MSMQMessageSend()
        {
            //组成完整的Path
            if (ServerMSMQPath == string.Empty)
            {
                //获取配置文件中的IP,如果没有则使用默认IP
                string strMSMQServerIP = (string)System.Configuration.ConfigurationSettings.AppSettings["MSMQLogServer"];

                if (strMSMQServerIP != null)
                {
                    ServerMSMQIP = strMSMQServerIP;
                }
                ServerMSMQPath = @"FormatName:DIRECT=tcp:" + ServerMSMQIP.Trim() + @"\" + LocalServerMSMQName;
            }

            //创建客户端MSMQ的对象
            if (mqClientListen == null)
            {
                if (!MessageQueue.Exists(ClientMSMQPath))
                {
                    mqClientListen = MessageQueue.Create(ClientMSMQPath, true);
                }
                else
                {
                    mqClientListen = new MessageQueue(ClientMSMQPath);

                }
                mqClientListen.Formatter = new BinaryMessageFormatter();
            }

            #region 检查服务器与客户端是否在同一IP下
            if (mqServerListen == null)
            {
                string strHostName = System.Net.Dns.GetHostName(); //得到本机的主机名
                System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostByName(strHostName); //取得本机IPs

                bool IsLocal = false;
                for (int i = 0; i < ipEntry.AddressList.Length; i++)
                {
                    string strAddr = ipEntry.AddressList[i].ToString(); //假设本地主机为单网卡
                    if (string.Equals(ServerMSMQIP.Trim(), strAddr))
                    {
                        IsLocal = true;
                    }
                }
                if (IsLocal == true)
                {
                    string strLocalServerMSMQPath = string.Format(@".\{0}", LocalServerMSMQName);
                    if (!MessageQueue.Exists(strLocalServerMSMQPath))
                    {
                        MessageQueue.Create(strLocalServerMSMQPath, true);
                    }
                    mqServerListen = new MessageQueue(strLocalServerMSMQPath);
                }
                else
                {
                    mqServerListen = new MessageQueue(ServerMSMQPath);
                }
            }
            #endregion

        }


        public void SendLog(object SendMessageInfo)
        {
            if (threadReceive == null)
            {
                threadReceive = new Thread(new ThreadStart(ClientMSMQListen));
                threadReceive.Start();
            }

            MessageQueueTransaction msta = new MessageQueueTransaction();
            try
            {
                msta.Begin();
                mqClientListen.Send(SendMessageInfo, msta);
                msta.Commit();
            }
            catch
            {
                msta.Abort();
            }


        }


        private void RollBack(System.Messaging.Message ms)
        {
            MessageQueueTransaction mstc = new MessageQueueTransaction();
            mstc.Begin();
            mqClientListen.Send(ms, mstc);
            mstc.Commit();
        }
        //这里设置同步监听还是异步监听有待验证
        private void ClientMSMQListen()
        {
            while (true)
            {
                System.Messaging.Message ms = mqClientListen.Receive();

                MessageQueueTransaction mstc = new MessageQueueTransaction();
                try
                {
                    mstc.Begin();
                    mqServerListen.Send(ms, mstc);
                    mstc.Commit();
                }
                catch (Exception ex)
                {
                    Thread.Sleep(3000);
                    mstc.Abort();
                    //将取出的数据重新插入到客户端消息队列上
                    RollBack(ms);
                }
            }
        }

        #region IMessage 成员

        public void AddMessage(MessageInfo msg)
        {

           // SendLog(msg);
        }


        public void AddMessage(object msg)
        {
            SendMessage = msg;
        }

        #endregion

        public override void Perform()
        {
            if (SendMessage != null)
            {
                SendLog(SendMessage);
            }
        }
    }
    //public enum InterSate { Connect, Interruption };
    //public class InternetState
    //{
    //    public static InterSate interState = InterSate.Interruption;
    //}
}
