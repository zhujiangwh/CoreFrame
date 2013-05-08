using System;
using System.Collections.Generic;
using System.Text;
using JZT.Utility.ThreadPool;
using System.Windows.Forms;
using System.Collections;

namespace JZT.Utility.Message
{
    public class DialogMsg : WorkItem,IChannel
    {
     

        public DialogMsg()
        {
            base.Priority = System.Threading.ThreadPriority.Highest;
        }

        public override void Perform()
        {
            MessageBox.Show( Convert.ToString(Content),Title);
        
        }


        public DialogResult Show(string title,string text, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options, Hashtable variant)
        {
            return System.Windows.Forms.MessageBox.Show(text, title, buttons, icon, defaultButton);
        }

        #region IChannel 成员

        public object Content
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Receive
        {
            get;
            set;
        }
      
        #endregion
    }
}
