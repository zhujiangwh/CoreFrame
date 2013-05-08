using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Data;
using System.Collections;
using System.Xml;

namespace JZT.Utility.Service
{
    public static class UtilObject
    {

        public static string GetPropertyToString(string title,object[] obj)
        {
            if (obj == null || obj.Length == 0)
                return title+":无";

            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < obj.Length; i++)
                {

                    sb.AppendLine(title+":");
                    if (obj[i] != null)
                    {
                        sb.AppendLine(GetPropertyToString(obj[i]));
                    }
                    else
                    {
                        sb.AppendLine("Null");
                    }
                }
                return sb.ToString();
                
            }
        }
        public static string GetPropertyToString(object obj)
        { 
            StringBuilder sb = new StringBuilder();
            Type tp = obj.GetType();
            try
            {
                if (obj == null)
                    return sb.ToString();
                if ( tp == typeof(Nullable<decimal>) ||  tp == typeof(Nullable<float>) || tp == typeof(Nullable<double>) ||
                    tp == typeof(double) || tp == typeof(float) || tp == typeof(decimal) ||
                    tp == typeof(Int16) || tp == typeof(Int32) || tp == typeof(Int64) ||
                    tp == typeof(Nullable<Int16>) || tp == typeof(Nullable<Int32>) || tp == typeof(Nullable<Int64>)
                    )
                {
                    sb.AppendLine(obj!= null?obj.ToString():"Null").ToString();
                }
                else
                {
                    if (obj is DataSet)
                    {
                        XmlDataDocument xml = new XmlDataDocument(obj as DataSet);
                        sb.AppendLine(xml.OuterXml);
                    }
                    else if (obj is DataTable)//魏刚20100812
                    {
                        XmlDataDocument xml = new XmlDataDocument(((DataTable)obj).DataSet);
                        sb.AppendLine(xml.OuterXml);
                    }
                    else
                    {
                        JZT.Utility.Service.CustomXmlSerializer ser = new CustomXmlSerializer();
                        sb.AppendLine(ser.WriteText(obj).ToString());
                    }
                }
            }
            catch { }
            return sb.ToString();
        }

      
    }
}
