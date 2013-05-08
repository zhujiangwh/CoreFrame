using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.Architecure;

namespace Server
{
    public partial class SplashForm : Form, IStartItemDisplayer
    {
        public SplashForm()
        {
            InitializeComponent();
        }

        #region IStartItemDisplayer 成员

        public void DisplayText(string text)
        {
            textBox1.Text = string.Format(" {0}{1}{1}", textBox1.Text,text, Environment.NewLine);
            Application.DoEvents();
        }

        #endregion
    }
}
