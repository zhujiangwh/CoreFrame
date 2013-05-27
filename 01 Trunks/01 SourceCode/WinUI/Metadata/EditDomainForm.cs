using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Core.Metadata
{
    public partial class EditDomainForm : Form
    {
        public EditDomainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Domain domain = new Domain();

            domain.Name = "test";
            domain.Caption = "显示标题";
            domain.BusiType = BusinessType.Int;

            domain.ClassPropertyDefine = new CSharpDesign.ClassPropertyDefine(domain);

            editDomainControl1.EditedObject = domain;

        }
    }
}
