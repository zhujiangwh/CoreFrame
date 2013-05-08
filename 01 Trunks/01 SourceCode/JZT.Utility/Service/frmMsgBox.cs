using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JZT.Utility
{
    partial class frmMsgBox : Form
    {
        public frmMsgBox()
        {
            InitializeComponent();
        }

        public JZTMsgDialogIcon Picture
        {
            set
            {
                switch (value)
                {
                    case JZTMsgDialogIcon.Success:
                        picIcon.Image = imageList1.Images["Right"];
                        break;
                    case JZTMsgDialogIcon.Warn:
                        picIcon.Image = imageList1.Images["War"];
                        break;
                    case JZTMsgDialogIcon.Question:
                        picIcon.Image = imageList1.Images["Qua"];
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnYES_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
