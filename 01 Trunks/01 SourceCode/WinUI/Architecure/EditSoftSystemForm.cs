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
    public partial class EditSoftSystemForm : Form, IObjectDisplay
    {
        public EditSoftSystemForm()
        {
            InitializeComponent();
        }


 
        private void button1_Click(object sender, EventArgs e)
        {
            //保存.
            CommonUIinteractive.Save();

            DialogResult = DialogResult.OK;




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
