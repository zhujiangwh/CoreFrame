using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;
using System.Xml.Serialization;
using System.Collections;
using JZT.Utility;


namespace Core.UI
{
    [Serializable]
    public class ControlService
    {
        #region singleton

        private static ControlService _instance;

        public static ControlService GetInstance()
        {
            if (_instance == null)
            { 
                _instance = new ControlService();
            }
            return _instance;
        }

        #endregion


        public ControlDefine this[string name]
        {
            get
            {
                return ControlDefineManager[name];
            }
        }

        public ControlDefineManager ControlDefineManager { get; set; }


        private ControlService()
        {
            ControlDefineManager = StreamTool.DeserializeXml<ControlDefineManager>("ControlDefine.xml");
            ControlDefineManager.Initial();
        }



    }

    [Serializable]
    public class ControlDefineManager
    {
        public virtual List<ControlDefine> ControlDefineList { get; set; }

        [XmlIgnore]
        public virtual Hashtable Hash { get; set; }

        public ControlDefineManager()
        {
            Hash =  new Hashtable();
            ControlDefineList = new List<ControlDefine>();
        }

        public void Initial()
        {
            foreach (ControlDefine item in ControlDefineList)
            {
                Hash.Add(item.Name, item);
            }
        }

        public ControlDefine this[string name]
        {
            get
            {
                return Hash[name] as ControlDefine;
            }
        }

    }



    public enum ControlType
    {
        Win, Web, Wpf
    }


    [Serializable]
    public class ControlDefine
    {
        [XmlAttribute]
        public virtual ControlType Catalog { get; set; }

        [XmlAttribute]
        public virtual string Name { get; set; }

        [XmlAttribute]
        public virtual string IconName { get; set; }


        public virtual ObjectDefine ControlType { get; set; }

        public ControlDefine()
        { }

        public ControlDefine(string name, ControlType catalog, string iconName, string assemblyName, string fullClassName)
        {
            Name = name;
            IconName = iconName;

            ControlType = new ObjectDefine(assemblyName, fullClassName);
        }
    }
}
