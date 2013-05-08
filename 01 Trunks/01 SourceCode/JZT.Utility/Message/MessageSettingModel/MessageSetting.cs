using System;
using System.Collections.Generic;
using System.Text;

namespace JZT.Utility.Message
{
    internal class MessageSetting
    {
        public string Name;
        public string Text;

        public List<MessageProvider> MsgProviderCollection=new List<MessageProvider> ();

    }
}
