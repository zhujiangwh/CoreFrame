using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JZT.Utility
{
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.ComponentModel.Browsable(false)]
    partial class frmCommonMsgAsk : Form
    {
        private JZTMsgDialogIcon m_icon;
        private string m_message;
        private JZTMsgDialogResult m_result;

        public frmCommonMsgAsk()
        {
            InitializeComponent();
        }

        public JZTMsgDialogResult ShowDialog(JZTMsgDialogIcon icon, string msg)
        {
            m_icon = icon;
            m_message = msg;
            this.ShowDialog();
            return m_result;
        }

        public JZTMsgDialogResult ShowDialogNoNewBill(JZTMsgDialogIcon icon, string msg)
        {
            btnNew.Visible = false;
            m_icon = icon;
            m_message = msg;
            this.ShowDialog();
            return m_result;
        }


        private void frmCommonMsgAsk_Load(object sender, EventArgs e)
        {
            switch (m_icon)
            {
                case JZTMsgDialogIcon.Success:
                    pictureBox1.Image = imageList1.Images["Right"];
                    break;
                case JZTMsgDialogIcon.Warn:
                    pictureBox1.Image = imageList1.Images["War"];
                    break;
                case JZTMsgDialogIcon.Question:
                    pictureBox1.Image = imageList1.Images["Qua"];
                    break;
                default:
                    break;
            }
            textBox1.Text = m_message;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            m_result = JZTMsgDialogResult.NewAccout;
            this.Close();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            m_result = JZTMsgDialogResult.Quit;
            this.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            m_result = JZTMsgDialogResult.Print;
            //MsgBox.ShowErr("该功能未实现，现在关闭对话框");
            this.Close();
        }
    }
}
