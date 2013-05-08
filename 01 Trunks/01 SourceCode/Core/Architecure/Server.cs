using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Core.Architecure
{
    [Serializable]
    public class Server
    {
        [Category("01 服务器")]
        [Description("名字")]
        [XmlAttribute]
        public string Name { get; set; }

        [Category("01 服务器")]
        [Description("编码")]
        [XmlAttribute]
        public string Code { get; set; }

        [Category("01 服务器")]
        [Description("说明")]
        [XmlAttribute]
        public string Text { get; set; }

        [Category("01 服务器")]
        [Description("IP 地址")]
        [XmlAttribute]
        public string IPAddress { get; set; }

        public virtual string GetText()
        {
            return string.Format(@"[{0}:{1}] ", Name, IPAddress);
        }
    }

    [Serializable]
    public class RemotingServer : Server
    {
        public RemotingServer()
        { }

        public RemotingServer(string protocol, string ipAddress)
        {
            Protocol = protocol;
            IPAddress = ipAddress;
        }


        [Category("02 Remoting服务器")]
        [Description("协议。 1. TCP 2.HTTP")]
        [XmlAttribute]
        public string Protocol { get; set; }

        public T CreateRemotingInterface<T>(string url)
        {
            return (T)Activator.GetObject(typeof(T), string.Format(@"{0}://{1}/{2}", Protocol, IPAddress, url));
        }

        public T CreateRemotingInterface<T>()
        {
            //根据 T 的类型取得 服务接口。
            string interfaceName = typeof(T).Name;
            string url = interfaceName.Substring(1, interfaceName.Length - 1);

            return (T)Activator.GetObject(typeof(T), string.Format(@"{0}://{1}/{2}", Protocol, IPAddress, url));
        }


        public override string GetText()
        {
            return string.Format(@"[{0}:{1}://{2}] ", Name, IPAddress, Protocol);
        }


    }

}
