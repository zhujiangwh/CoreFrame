using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;
using JZT.Utility.ThreadPool;
namespace JZT.Utility.Message
{
  
    public class MailMsg : WorkItem,IChannel
    {
       

       

        public MailMsg()
        {
            base.Priority = System.Threading.ThreadPriority.Normal;
        }
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="toname">接收者显示姓名</param>
        /// <param name="toemail">接收者mail</param>
        /// <param name="smtpclient">smtp服务器</param>
        /// <param name="fromname"> 发送者显示姓名</param>
        /// <param name="fromemail">发送email</param>
        /// <param name="password">密码</param>
        /// <param name="subject">正文</param>
        /// <param name="body">主题</param>
        /// <returns></returns>
        public bool SendEmail(string toname, string toemail, string smtpclient, string fromname, string fromemail, string password, string subject, string body)
        {

            try
            {
                string [] emailInfo = System.Configuration.ConfigurationManager.AppSettings["EmailAddress"].Split(',');
                SmtpClient client = new SmtpClient(emailInfo[0]);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(emailInfo[1], emailInfo[2]);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage mail = new MailMessage(emailInfo[1], emailInfo[3]);
                mail.Subject = subject;
                mail.BodyEncoding = System.Text.Encoding.Default;
                mail.Body = body;
                mail.IsBodyHtml = false;
                client.Send(mail);

            }
            catch (Exception ex)
            {

            }
            return true;

            //try
            //{
            //    SmtpClient client = new SmtpClient("mail.jztey.com");
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = new System.Net.NetworkCredential("jzterpadmin@jztey.com", "jzterp");
            //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //    MailMessage mail = new MailMessage("jzterpadmin@jztey.com", "jzterpadmin@jztey.com");
            //    mail.Subject = subject;
            //    mail.BodyEncoding = System.Text.Encoding.Default;
            //    mail.Body = body;
            //    mail.IsBodyHtml = false;
            //    client.Send(mail);
            //    return true;

            //}
            //catch (Exception ex)
            //{

            //}
            //return false;


            //try
            //{
            //    MailAddress from = new MailAddress(fromemail, fromname);
            //    MailAddress to = new MailAddress(toemail, toname);
            //    MailMessage message = new MailMessage(from, to);
            //    message.Subject = subject;
            //    message.Body = body;
            //    message.Priority = MailPriority.High;
            //    message.IsBodyHtml = true;

            //    SmtpClient client = new SmtpClient(smtpclient);//伺服器,如"smtp.163.com"
            //    client.Credentials = new System.Net.NetworkCredential(fromemail, password);
            //    client.Send(message);

            //    return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
        
        public override void Perform()
        {
            SendEmail(Receive, Receive, "smtp.163.com", "新业务系统", "xuwei316@163.com", "745664745203", Title, Convert.ToString(Content));
        }

        //public void SendEmail(string title, string errorMsg)
        //{


        //}

        //public void SendEmail(string title, Exception ex)
        //{
        //    string name = "";
        //    string ip = "";
        //    try
        //    {
        //        name = Dns.GetHostName();
        //        ip = Dns.GetHostEntry(name).AddressList[0].ToString();
        //    }
        //    catch
        //    { }

        //    string temp = JZT.Utility.Service.UtilObject.FormatException(ex);
        //    temp += string.Format("{0}:{1}\r\n", name, ip); ;
        //    SendEmail(title, temp);
        //}
      

    

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
        public string MessageType
        {
            get;

            set;

        }
      
      
       
        #endregion
    }
}
