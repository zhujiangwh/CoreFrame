using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.Metadata.CSharpDesign;

namespace Core.Metadata.CSharpDefine
{
    public partial class EditCSharpPropertyControl : UserControl, IObjectEditer
    {
        public EditCSharpPropertyControl()
        {
            InitializeComponent();
        }

        #region IObjectEditer 成员

        public object EditedObject
        {
            get
            {
                return bindingSource1.DataSource;

            }
            set
            {
                bindingSource1.DataSource = value;
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            ClassPropertyDefine obj = EditedObject as ClassPropertyDefine;

            if (obj != null)
            {

                richTextBox1.Text = obj.GenCode();
            }
        }


        private void bindingSource1_CurrentItemChanged(object sender, EventArgs e)
        {
            ClassPropertyDefine obj = EditedObject as ClassPropertyDefine;

            richTextBox1.Text = obj.GenCode();

        }
    }
}
