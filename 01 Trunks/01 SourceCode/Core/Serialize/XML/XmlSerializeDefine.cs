using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using JZT.Utility;

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

        public void Save(object obj)
        {
            IXmlSerialize xs = obj as IXmlSerialize;

            if (xs != null)
            {
                //保存到文件。
                string fileName = string.Format("{0}.{1}.xml",xs.GetFileName(),ExtFileName);
                StreamTool.SerializeXML(obj, fileName);
            }
        }

        public T GetObject<T>(string key)
        {
            string fileName = string.Format("{0}.{1}.xml", key, ExtFileName);

            return StreamTool.DeserializeXml<T>(fileName);
        }

    }

    [Serializable]
    public class XmlSerializeDefineManager : IXmlSerialize
    {
        #region singleton

        private static XmlSerializeDefineManager _instance;

        private XmlSerializeDefineManager()
        {
            XmlSerializeDefineList = new List<XmlSerializeDefine>();
            //XmlSerializeDefine d = new XmlSerializeDefine("Core.UI.SelectObjectUIC", "XXX", "relationPath");
            //XmlSerializeDefine e = new XmlSerializeDefine("Core.Serialize.XML.XmlSerializeDefineManager", "YYY", "relationPath");


            //XmlSerializeDefineList.Add(d);
            //XmlSerializeDefineList.Add(e);
        }

        public static XmlSerializeDefineManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = StreamTool.DeserializeXml<XmlSerializeDefineManager>("对象序列化配置.xml");
            }

            return _instance;
        }

        #endregion

        public virtual string BasePath { get; set; }

 
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

        public void Save(object obj)
        {
            string fullClassName = obj.GetType().FullName;

            XmlSerializeDefine xmlSerializeDefine = this[fullClassName];

            if (xmlSerializeDefine != null)
            {
                xmlSerializeDefine.Save(obj);
            }
            else
            {
                throw new Exception (" 该类型的存储器没有定义 ！");
            }
        }

        public T GetObject<T>(string key)
        {
            T ret = default(T);



            string fullClassName = typeof(T).FullName;

            XmlSerializeDefine xmlSerializeDefine = this[fullClassName];

            if (xmlSerializeDefine != null)
            {
                ret = xmlSerializeDefine.GetObject<T>(key);
            }
            else
            {
                throw new Exception(" 该类型的存储器没有定义 ！");
            }

            return ret;

        }


        #region IXmlSerialize 成员

        public virtual string GetFileName()
        {
            return "对象序列化配置.xml";
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

        public string GetFileName(string key)
        {
            return string.Format("{0}.xml",key);
        }

        #endregion
    }
}
