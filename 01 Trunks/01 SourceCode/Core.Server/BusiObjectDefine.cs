using System;
using System.Collections.Generic;
using Core.Architecure;
using Core.Serialize.XML;
using JZT.Utility;

namespace Core.Server
{
    [Serializable]
    public class BusiObjectPool : IXmlSerialize
    {
        #region singleton
        private static BusiObjectPool _instance;

        public static BusiObjectPool GetInstance()
        {
            if (_instance == null)
            {
                _instance = StreamTool.DeserializeXml<BusiObjectPool>("BoPool.CO.xml");
            }

            return _instance;

        }

        private BusiObjectPool()
        {
            BusiObjectDefineList = new List<BusiObjectDefine>();

        }

        #endregion

        public virtual List<BusiObjectDefine> BusiObjectDefineList { get; set; }

        public BusiObjectDefine this[string fullClassName]
        {
            get
            {
                return BusiObjectDefineList.Find(delegate(BusiObjectDefine c3) { return c3.FullClassName == fullClassName; });
            }
        }


        #region IXmlSerialize 成员

        public string GetFileName()
        {
            return "BoPool";
        }

        public string GetFileName(string key)
        {
            return key;
        }

        public string GetPath()
        {
            throw new NotImplementedException();
        }

        public void BeforeSerialize()
        {
            throw new NotImplementedException();
        }

        public void AfterDeserialize()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    [Serializable]
    public class BusiObjectDefine
    {
        public virtual string FullClassName { get; set; }

        public virtual ObjectDefine BusiObjectDefi { get; set; }

    }

}
