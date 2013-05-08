using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

namespace Core.Serialize.XML
{
    [Serializable]
    public class XmlSerializeDefine
    {
        [XmlAttribute]
        public virtual string FullClassName { get; set; }
        [XmlAttribute]
        public virtual string ExtFileName { get; set; }
        [XmlAttribute]
        public virtual string RelatinPath { get; set; }

        public XmlSerializeDefine()
        {
            FullClassName = "全类名";
        }

        public XmlSerializeDefine(string fullClassName, string extFileName, string relationPath)
        {
            FullClassName = fullClassName;
            ExtFileName = extFileName;
            RelatinPath = relationPath;
        }

    }

    [Serializable]
    public class XmlSerializeDefineManager : IXmlSerialize
    {
        public XmlSerializeDefineManager()
        {
            XmlSerializeDefineList = new List<XmlSerializeDefine>();
            //XmlSerializeDefine d = new XmlSerializeDefine("Core.UI.SelectObjectUIC", "XXX", "");
            //XmlSerializeDefine e = new XmlSerializeDefine("Core.Serialize.XML.XmlSerializeDefineManager", "YYY", "");


            //XmlSerializeDefineList.Add(d);
            //XmlSerializeDefineList.Add(e);

        }

        public virtual List<XmlSerializeDefine> XmlSerializeDefineList { get; set; }

        public XmlSerializeDefine this[string fullClassName]
        {
            get
            {
                return XmlSerializeDefineList.Find(delegate(XmlSerializeDefine c3) { return c3.FullClassName == fullClassName; });
            }
        }

        public virtual void LoadXmlSerializeDefineList(IList list)
        {
            XmlSerializeDefineList.Clear();

            foreach (object item in list)
            {
                XmlSerializeDefineList.Add(item as XmlSerializeDefine);
            }
        }

        #region IXmlSerialize 成员

        public virtual string GetFileName()
        {
            return "对象序列化配置";
        }

        public virtual string GetPath()
        {
            return string.Empty;
        }

        public virtual void BeforeSerialize()
        {
        }

        public virtual void AfterDeserialize()
        {
        }

        #endregion





        #region IXmlSerialize 成员


        public string GetFileName(string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
