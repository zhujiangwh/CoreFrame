using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JZT.Utility;

namespace Core.Serialize.XML
{
    public class XmlSerialize :IObjectSerialize
    {
        #region IObjectSerialize 成员

        public void Save(object obj)
        {
            IXmlSerialize xs = obj as IXmlSerialize;

            if (xs != null)
            {
                //保存到文件。
                string fileName = xs.GetFileName();

                //string fileName = "sdsdfj.xml";

                StreamTool.SerializeXML(obj, fileName);
            }
        }

        public void Update(object obj)
        {
            //更新到文件
        }

        public void Delete(object obj)
        {
            //逻辑删除后保存文件
        }

        public T GetObject<T>(string Key)
        {
            // 
            return StreamTool.DeserializeXml<T>(string.Format("{0}.xml",Key));
        }

        public void RealDelete(object obj)
        {
            //删除文件
        }

        #endregion
    }
}
