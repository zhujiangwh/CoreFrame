using System;
using System.Collections.Generic;
using System.Text;
using JZT.Utility.ThreadPool;

namespace JZT.Utility.Message
{
    public class PhoneMessage 
    {
       
        //public void SendMessageToPhone(string [] phone,string message)
        //{
        //    if(phone!=null)
        //    {
        //        foreach(string ph in phone)
        //        {
        //            SendMessageToPhone(ph,message);
        //        }
        //    }
        //}
        public void SendMessageToPhone(string  phone,string message,string phoneUserName,string phonePwd,string phoneServiceAdress)
        {
            if (phone != "")
            {

                try
                {
                   
                    string sEncryptionKey = "12345678";
                    string encPwd = DES.Encrypt(phonePwd, sEncryptionKey);
                    string encDst = DES.Encrypt(phone, sEncryptionKey);
                    string encMsg = DES.Encrypt(message, sEncryptionKey);
                    string encTime = DES.Encrypt("200704011200", sEncryptionKey);
                    string encExNo = DES.Encrypt("01", sEncryptionKey);      
                    WS.MainServices service = new WS.MainServices();
                    service.Url = phoneServiceAdress;
                    string sRes = service.massSendCcdx(phoneUserName, encPwd, encDst, encMsg, encTime, encExNo, "CCDX");
                   
                    //此处需要分析返回码，检验发送是否成功，暂不检验
                    //num=1&success=13971002984&faile=&err=发送成功！&errid=0

                }
                catch
                {
                    throw new Exception("发送手机短息错误!");
                }
            }
            else
            {
                throw new Exception("该审核人未维护手机号!");
            }
        }

    }
}
