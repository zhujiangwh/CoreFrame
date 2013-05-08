using System;
using System.Collections.Generic;
using System.Text;

namespace JZT.Utility.Message
{
    public interface IChannel
    {
        object Content
        {
            get;
            set;
        }
        string Title
        {
            get;
            set;
        }
        string Receive
        {
            get;
            set;
        }
        
       
    }
}
