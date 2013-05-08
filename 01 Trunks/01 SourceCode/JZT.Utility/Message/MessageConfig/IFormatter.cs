using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;


namespace JZT.Utility.Message
{
    public interface IFormatter
    {

         //在此方法中将MessageEntry转换为你所需要的类
         object Formatter(string title,Hashtable variant);
    
         string FormatterTemplate
         {
             set;
             get;
         }
    }
}
