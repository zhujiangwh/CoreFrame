using System;
using System.Collections.Generic;
using System.Text;

namespace JZT.Utility.Message.MessageFomatter
{
    internal class MailFormatter : JZT.Utility.Message.IFormatter
    {


        #region IFormatter 成员

        public object Formatter(string title, System.Collections.Hashtable variant)
        {
            return null;
        }

        public string FormatterTemplate
        {
            get;
            set;
        }

        #endregion
    }
}
