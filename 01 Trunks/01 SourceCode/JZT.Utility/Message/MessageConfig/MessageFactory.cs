using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using JZT.Utility.ThreadPool;
using System.Windows.Forms;

namespace JZT.Utility.Message
{
    public class MessageFactory
    {

      


        private DataSet _dtMessageSettings;

        public static readonly MessageFactory Instance = new MessageFactory();

     
        private MessageFactory()
        {
           
        }

        public  DataSet MessageSettings
        {
            get { return _dtMessageSettings; }
            set
            {
                _dtMessageSettings = value;
            }
        }


        #region 发送消息
        /// <summary>
        /// 发送异步消息，无返回值。支持多管道发送
        /// </summary>
        /// <param name="MessageSettingName">消息名</param>
        public void SendMessage(string MessageSettingName)
        {
             SendMessage(MessageSettingName, null);
        }

        /// <summary>
        /// 发送异步消息，无返回值。支持多管道发送
        /// </summary>
        /// <param name="MessageSettingName">消息名</param>
        /// <param name="variant">参数列表</param>
        public void SendMessage(string MessageSettingName, Hashtable variant)
        {
            MessageSetting MsgSetting = GetMessgeSetting(MessageSettingName);

            if (MsgSetting != null)
            {
                SendMessage(MsgSetting, variant);
            }
            else
            {
                throw new Exception("未定义的MessageSettingName");
            }
        }

        private void SendMessage(MessageSetting MsgSetting, Hashtable variant)
        {
            foreach (MessageProvider msgProvide in MsgSetting.MsgProviderCollection)
            {
                msgProvide.MsgChannel.Content = msgProvide.MsgFormatter.Formatter(MsgSetting.Text, variant);
                AddPool((IWorkItem)msgProvide.MsgChannel);
            }
         
        }
        private void AddPool(IWorkItem item)
        {
            MessageWorkQueue.Instance.AddMessageToPool(item);
        }
      
    
        #endregion


        #region 弹出dialog

        #region ShowDialog
        /// <summary>
        /// 根据消息代码显示本地化的对话框,需要注意是 消息代码对应的多个管道中，如有多个返回值，只将返回最后一个Dialogresult~,ShowDialog 为同步调用。
        /// </summary>
        /// <param name="strMsgCode">消息代码</param>
        /// <param name="objArgs">消息的参数</param>
        /// <returns></returns>
        public void SendDialog(string strMsgCode, Hashtable variant)
        {
            MessageSetting MsgSetting = GetMessgeSetting(strMsgCode);
          
            foreach (MessageProvider msgProvide in MsgSetting.MsgProviderCollection)
            {
                if (msgProvide.MsgChannel != null && msgProvide.MsgChannel is DialogMsg)
                {
                    msgProvide.MsgChannel.Title = MsgSetting.MsgProviderCollection[0].MsgChannel.Title;
                    msgProvide.MsgChannel.Content = (string)MsgSetting.MsgProviderCollection[0].MsgFormatter.Formatter(MsgSetting.Text, variant);
                    ((DialogMsg)msgProvide.MsgChannel).Perform();
                }
                else
                {
                    msgProvide.MsgChannel.Content = msgProvide.MsgFormatter.Formatter(MsgSetting.Text, variant);
                    AddPool((IWorkItem)msgProvide.MsgChannel);
                }

            }
        }

        public DialogResult SendDialog(string strMsgCode, MessageBoxButtons buttons, Hashtable variant)
        {
            return SendDialog(strMsgCode, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, variant);
        }

        public DialogResult SendDialog(string strMsgCode, MessageBoxButtons buttons, MessageBoxIcon icon, Hashtable variant)
        {

            return SendDialog(strMsgCode, buttons, icon, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly, variant);
        }

        public DialogResult SendDialog(string strMsgCode, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, Hashtable variant)
        {
            return SendDialog(strMsgCode, buttons, icon, defaultButton, MessageBoxOptions.DefaultDesktopOnly, variant);
        }

        public DialogResult SendDialog(string strMsgCode, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, Hashtable variant)
        {

            MessageSetting MsgSetting = GetMessgeSetting(strMsgCode);
            DialogResult resoult = DialogResult.None;
            foreach (MessageProvider msgProvide in MsgSetting.MsgProviderCollection)
            {                
                if (msgProvide.MsgChannel != null && msgProvide.MsgChannel is DialogMsg)
                {
                    string title = MsgSetting.MsgProviderCollection[0].MsgChannel.Title;
                    string content = (string)MsgSetting.MsgProviderCollection[0].MsgFormatter.Formatter("", variant);
                    resoult = ((DialogMsg)msgProvide.MsgChannel).Show(title, content, buttons, icon, defaultButton, options, variant);
                }
                else
                {
                    msgProvide.MsgChannel.Content = msgProvide.MsgFormatter.Formatter(MsgSetting.Text, variant);
                    AddPool((IWorkItem)msgProvide.MsgChannel);
                }

            }
            return resoult;
        }
        
        #endregion




        #endregion 弹出dialog

       



        


        private MessageSetting GetMessgeSetting(string Name)
        {
            DataTable tableSource = MessageSettings.Tables[0];
            DataRow[] dataRowCollect;
            dataRowCollect = tableSource.Select("Name='" + Name + "'");
            if (dataRowCollect == null)
                return null;
            MessageSetting MsgSetting = new MessageSetting();
            foreach (DataRow dr in dataRowCollect)
            {
                //如果转换不了直接出错，抛出异常
                MsgSetting.Name = Convert.ToString(dr["Name"]);
                MsgSetting.Text = Convert.ToString(dr["Text"]);

                MessageProvider MsgProvider = new MessageProvider();
                MsgProvider.MsgChannel = (IChannel)JZT.Common.Cache.ObjectCacheManage.CreateObject(Convert.ToString(dr["ChannelAssembly"]), Convert.ToString(dr["ChannelNameSpance"]));
                if (MsgProvider.MsgChannel!= null)
                {
                    MsgProvider.MsgChannel.Title = Convert.ToString(dr["Title"]);
                    MsgProvider.MsgChannel.Receive = Convert.ToString(dr["Receive"]);

                }
                MsgProvider.MsgFormatter = (IFormatter)JZT.Common.Cache.ObjectCacheManage.CreateObject(Convert.ToString(dr["FormatterAssembly"]), Convert.ToString(dr["FormatterNameSpance"]));
                if (MsgProvider.MsgFormatter != null)
                    MsgProvider.MsgFormatter.FormatterTemplate = Convert.ToString(dr["Template"]);
                MsgSetting.MsgProviderCollection.Add(MsgProvider);
            }
            return MsgSetting;

        }



    }
}
