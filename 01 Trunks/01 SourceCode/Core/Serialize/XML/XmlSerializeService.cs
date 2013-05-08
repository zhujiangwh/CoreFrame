using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JZT.Utility;
using Core.UI;

namespace Core.Serialize.XML
{
    public class XmlSerializeService
    {
        public XmlSerializeService()
        {
            XmlSerializeDefineManager = StreamTool.DeserializeXml<XmlSerializeDefineManager>(@"Config\对象序列化配置.YYY.XML");
        }

        public void SaveXmlSerializeDefineManager()
        {
            StreamTool.SerializeXML(XmlSerializeDefineManager, @"Config\对象序列化配置.YYY.XML"); 
        }

        public virtual void SaveToFile(IXmlSerialize obj)
        {
            obj.BeforeSerialize();

            string fullClassName = obj.GetType().FullName;
            
            //获取扩展名。
            string filename = GetFileName(fullClassName, obj.GetFileName());// string.Format("{0}.{1}.{2}", obj.GetFileName(), xmlSerializeDefine.ExtFileName, "XML");

            SaveToFile(obj, filename);
        }

        public string GetFileName(string fullClassName , string leftFileName)
        {
            XmlSerializeDefine xmlSerializeDefine = XmlSerializeDefineManager[fullClassName];

            return string.Format(@"Config\{0}.{1}.{2}", leftFileName, xmlSerializeDefine.ExtFileName, "XML");
        }

        public virtual void SaveToFile(object obj , string fileName)
        {
            StreamTool.SerializeXML(obj,fileName);
        }



        public virtual object LoadFormFile(string fullClassName,string fileName)
        {
            Type type = Type.GetType(fullClassName);
            IXmlSerialize obj = StreamTool.DeserializeXml(fileName, type) as IXmlSerialize;
            obj.AfterDeserialize();
            return obj;
        }

        public virtual XmlSerializeDefineManager XmlSerializeDefineManager { get; set; }

 
    }
}
