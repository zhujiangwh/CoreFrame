using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Serialize.XML;

namespace Core.UI
{
    [Serializable]
    public class UIinteractive : IXmlSerialize
    {
        #region IXmlSerialize 成员

        public virtual string GetFileName()
        {
            return string.Empty;
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
