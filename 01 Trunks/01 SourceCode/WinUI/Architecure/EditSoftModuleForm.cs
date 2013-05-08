using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.UI;

namespace Core.Architecure
{
    public partial class EditSoftModuleForm : Form, IObjectDisplay
    {
        public EditSoftModuleForm()
        {
            InitializeComponent();
        }

        #region IObjectDisplay 成员

        public void Dispaly(object obj)
        {
            bindingSource1.DataSource = obj;

            if (ShowDialog() == DialogResult.OK)
            {
            }
        }

        public CommonUIinteractive CommonUIinteractive { get; set; }
        #endregion
    }
}
