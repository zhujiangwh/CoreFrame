using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Management;
using System.Windows.Forms ;

namespace JZT.Utility
{

/*----------------------------------------------------------------
// Copyright (C) 2011 九州通集团有限公司
// 版权所有。 
//
// 文件名：MyComments.cs
// 文件功能描述： 系统环境停止。
//     
// 
// 创建标识：朱江

//----------------------------------------------------------------*/
    public static class EnvironmentInfo
    {
        private static string m_IPAddress;
        
        /// <summary>
        /// 当前机器IP
        /// </summary>
        public static string IPAddress
        {
            get
            {
                pGetInfomations();
                return m_IPAddress;
            }
        }

        private static string m_MacAddress;
        /// <summary>
        /// 当前MAC
        /// </summary>
        public static string MacAddress
        {
            get
            {
                pGetInfomations();
                return m_MacAddress;
            }
        }

        private static string m_HostName;
        /// <summary>
        /// 当前机器名
        /// </summary>
        public static string HostName
        {
            get
            {
                pGetInfomations();
                return m_HostName;
            }
        }

        private static string m_OS_Caption;
        /// <summary>
        /// 操作系统名
        /// </summary>
        public static string OS_Caption
        {
            get
            {
                pGetInfomations();
                return m_OS_Caption;
            }
        }

        private static string m_OS_Version;
        /// <summary>
        /// 操作系统版本号
        /// </summary>
        public static string OS_Version
        {
            get
            {
                pGetInfomations();
                return m_OS_Version;
            }
        }

        private static string _devAssemblyPath;
        /// <summary>
        /// 
        /// </summary>
        public static string DevAssemblyPath
        {
            get
            {
                if (string.IsNullOrEmpty(_devAssemblyPath))
                {
                    _devAssemblyPath = System.Configuration.ConfigurationSettings.AppSettings["DevAssemblyPath"];
                }
                return _devAssemblyPath;
            }
            set
            {
                _devAssemblyPath = value;
                System.Configuration.ConfigurationSettings.AppSettings["DevAssemblyPath"] = _devAssemblyPath;
            }
        }

        public static string SetDevAssemblyPath ()
        {
            //打开目录选择对话框。

          
            return "";
        }





        private static bool m_hasGetInformations = false;

        private static void pGetInfomations()
        {
            if (m_hasGetInformations)
                return;

            StringBuilder sbMac = new StringBuilder();
            StringBuilder sbIP = new StringBuilder();

            NetworkInterface[] NetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in NetworkInterfaces)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    PhysicalAddress Physical = adapter.GetPhysicalAddress();
                    byte[] bts = Physical.GetAddressBytes();
                    foreach (byte bt in bts)
                    {
                        sbMac.Append(bt.ToString("X2"));
                    }

                    IPInterfaceProperties IPInterfaceProperties = adapter.GetIPProperties();
                    UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = IPInterfaceProperties.UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection)
                    {
                        if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            sbIP.Append(UnicastIPAddressInformation.Address.ToString());
                        }
                    }

                    sbMac.Append(";");
                    sbIP.Append(";");
                }
            }

            m_IPAddress = sbIP.ToString();
            m_MacAddress = sbMac.ToString();
            m_HostName = Dns.GetHostName();
            m_OS_Version = System.Environment.OSVersion.VersionString;

            SelectQuery query = new SelectQuery("Win32_OperatingSystem");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                using (ManagementObjectCollection objs = searcher.Get())
                {
                    StringBuilder sbOS = new StringBuilder();
                    foreach (System.Management.ManagementObject obj in objs)
                    {
                        sbOS.Append(
                            obj.GetPropertyValue("Caption").ToString() +
                            obj.GetPropertyValue("Version").ToString() +
                            ";");
                    }
                    m_OS_Caption = sbOS.ToString();
                }
            }
            m_hasGetInformations = true;
        }
    }
}
