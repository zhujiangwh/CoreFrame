using System;
using System.Collections.Generic;
using System.Text;
using ASIMLib;
using JZT.Utility.ThreadPool;

namespace JZT.Utility.Message
{
    public class AMSAttachmentMsg : WorkItem, IChannel
    {

        private readonly string ServerIP = "219.139.241.226";
        private readonly string ServerPort = "7001";
        ASIMLib.IMClass im = null;

        public AMSAttachmentMsg()
        {
            base.Priority = System.Threading.ThreadPriority.Normal;
            im = new ASIMLib.IMClass();
            im.Port = Convert.ToInt16(ServerPort);
        }

        /// <summary>
        /// 初始化消息发送人
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Pwd"></param>
        /// <returns></returns>
        public int InitMessage(string Sender, string Pwd)
        {
            try
            {
                im.Init(ServerIP, Sender, Pwd);

                return im.GetLastErr();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 发送带附件消息
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Pwd"></param>
        /// <param name="Receivers"></param>
        /// <param name="Subject"></param>
        /// <param name="MsgBody"></param>
        /// <param name="nStyle"></param>
        /// <returns></returns>
        public int SendMsg(string Sender, string Pwd, string Receivers, string Subject, string MsgBody, string[] FileNames, int nStyle)
        {
            try
            {
                ASIMLib.MsgClass msg = new MsgClass();
                msg.ContentType = "Text/Text";
                msg.Subject = Subject;
                msg.Body = MsgBody;
                msg.MsgType = nStyle;
                foreach (string FileName in FileNames)
                {
                    msg.AddAttachment(FileName);
                }
                msg.FolderName = "UploadFile";
                int ret = im.SendMsgEx(msg, Receivers);

                return ret;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override void Perform()
        {
            //SendMsg("xukai", "pijiuhua", Receive, Title, Convert.ToString(Content), 0); 
        }

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
    }
}
