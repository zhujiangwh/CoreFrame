using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using System.Xml.Serialization;


namespace Core.Architecure
{
    [Serializable]
    public class ObjectDefine
    {
        [XmlAttribute]
        public virtual string AssemblyName { get; set; }
        [XmlAttribute]
        public virtual string FullClassName { get; set; }

        public ObjectDefine()
        { }

        public ObjectDefine(string assemblyName, string fullClassName)
        {
            AssemblyName = assemblyName;
            FullClassName = fullClassName;
        }

        public virtual object CreateObject()
        {
            object obj = null;
            if (!string.IsNullOrEmpty(AssemblyName) && !string.IsNullOrEmpty(FullClassName))
            {
                obj = JZT.Common.Cache.ObjectCacheManage.CreateObject(AssemblyName, FullClassName);
            }
            else
            {
                //引发异常。
            }

            return obj;

        }
    }

    public enum ObjectType
	{
        Entity = 1 ,

        WinControl = 101,
        WinForm = 102,

        WpfControl = 201,

        WebControl = 301
	}

    [Serializable]
    public class SoftComponent : ObjectDefine, IGuidStringKey
    {
        [XmlAttribute]
        public virtual string GuidString { get; set; }
        [XmlAttribute]
        public virtual string Author { get; set; }
        [XmlAttribute]
        public virtual string Memo { get; set; }
        [XmlAttribute]
        public virtual string RelationObject { get; set; }

        public virtual SoftModule SoftModule { get; set; }

        [XmlAttribute]
        public virtual string ModuleGuidString { get; set; }

        public virtual ObjectType ObjectType { get; set; }


        #region IGuidStringKey 成员


        public virtual bool DeleteFlag { get; set; }

        #endregion
    }

}
