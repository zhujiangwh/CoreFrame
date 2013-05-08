using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Serialize.XML;

namespace Core.Common
{
    [Serializable]
    public class BaseObject : IXmlSerialize
    {
        #region IXmlSerialize 成员

        public virtual string GetFileName()
        {
            return string.Empty;
        }

        public virtual void BeforeSerialize()
        {
        }

        public virtual void AfterDeserialize()
        {
        }

 

        public string GetPath()
        {
            return string.Empty;
        }

        #endregion
    }
}
