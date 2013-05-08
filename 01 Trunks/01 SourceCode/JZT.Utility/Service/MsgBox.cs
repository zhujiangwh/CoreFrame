using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace JZT.Utility
{
    public enum JZTMsgDialogResult
    {
        NewAccout,
        Quit,
        Print,
        Cancel
    }

    public enum JZTMsgDialogIcon
    {
        Success,
        Warn,
        Question
    }

    public static class MsgBox
    {
        /// <summary>
        /// 弹出信息框
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static void ShowInfo(string format, params object[] arg0)
        {
            MessageBox.Show(
                string.Format(format, arg0),
                "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 弹出警告框
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static void ShowExcl(string format, params object[] arg0)
        {
            MessageBox.Show(
                string.Format(format, arg0),
                "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// 弹出错误框
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static void ShowErr(string format, params object[] arg0)
        {
            MessageBox.Show(
                string.Format(format, arg0),
                "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 弹出是否的询问框，用户点击“是”返回true，用户点击“否”返回false 
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static bool ShowYesNo(string format, params object[] arg0)
        {
            DialogResult dr = MessageBox.Show(
                string.Format(format, arg0),
                "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            return (dr == DialogResult.Yes);
        }

        /// <summary>
        /// 弹出“确定、取消”的对话框，用户点击“确定”返回true，用户点击“取消”返回false 
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static bool ShowOKCancel(string format, params object[] arg0)
        {
            DialogResult dr = MessageBox.Show(
                string.Format(format, arg0),
                "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            return (dr == DialogResult.OK);
        }

        /// <summary>
        /// 弹出通用的单据对话框
        /// </summary>
        /// <param name="icon">图标</param>
        /// <param name="format">字符串格式串</param>
        /// <param name="arg0">字符串格式参数</param>
        /// <returns>选取结果</returns>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static JZTMsgDialogResult ShowCommonDlg(JZTMsgDialogIcon icon, string format, params object[] arg0)
        {
            using (frmCommonMsgAsk frm = new frmCommonMsgAsk())
            {
                JZTMsgDialogResult ret = frm.ShowDialog(icon, string.Format(format, arg0));
                return ret;
            }
        }

        /// <summary>
        /// 弹出通用的单据对话框
        /// </summary>
        /// <param name="icon">图标</param>
        /// <param name="format">字符串格式串</param>
        /// <param name="arg0">字符串格式参数</param>
        /// <returns>选取结果</returns>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static JZTMsgDialogResult ShowCommonDlgNoNewBill(JZTMsgDialogIcon icon, string format, params object[] arg0)
        {
            using (frmCommonMsgAsk frm = new frmCommonMsgAsk())
            {
                JZTMsgDialogResult ret = frm.ShowDialogNoNewBill(icon, string.Format(format, arg0));
                return ret;
            }
        }


        /// <summary>
        /// 弹出重试或取消的对话框，如果点击“重试”则返回true,点击“取消” 返回false
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static bool ShowRetryCancel(string format, params object[] arg0)
        {
            DialogResult dr = MessageBox.Show(
                string.Format(format, arg0),
                "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Hand);

            return (dr == DialogResult.Retry);
        }

        /// <summary>
        /// 弹出“是、否、取消”的对话框，返回对话框结果
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static DialogResult ShowYesNoCancel(string format, params object[] arg0)
        {
            DialogResult dr = MessageBox.Show(
                string.Format(format, arg0),
                "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            return dr;
        }

        /// <summary>
        /// 弹出异常信息
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static void ShowErr(Exception exception)
        {
            //Assembly.Load("JZT.Common");
            string errorBoxAssembly = "JZT.Common";
            string errorBoxNamespace = "JZT.Common.ExceptionBox";

            try
            {
                Assembly assembly = Assembly.Load(errorBoxAssembly);
                Type tp = assembly.GetType(errorBoxNamespace);
                Form frm = (Form)Activator.CreateInstance(tp, new object[] { exception });
                frm.ShowDialog();
            }
            catch
            {
                MsgBox.ShowErr("无法找到[{0}, {1}] 用来显示错误窗体, 当前异常信息：\n{2}",
                                    errorBoxAssembly, errorBoxNamespace, exception.Message);
            }
        }

        /// <summary>
        /// 弹出提示信息（长内容，带拖动条）
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        public static void ShowInfoEx(string format, params object[] args)
        {
            using (frmMsgBox dlg = new frmMsgBox())
            {
                dlg.Picture = JZTMsgDialogIcon.Success;
                dlg.txtContent.Text = string.Format(format, args);
                dlg.btnNo.Visible = false;
                dlg.btnYES.Visible = false;
                dlg.btnOK.Visible = true;
                dlg.AcceptButton = dlg.btnOK;
                dlg.CancelButton = dlg.btnOK;
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// 弹出警告信息（长内容，带拖动条）
        /// </summary>
        public static void ShowWarnEx(string format, params object[] args)
        {
            using (frmMsgBox dlg = new frmMsgBox())
            {
                dlg.Picture = JZTMsgDialogIcon.Warn;

                if (args.Length == 0)
                {
                    dlg.txtContent.Text = format;
                }
                else
                {
                    dlg.txtContent.Text = string.Format(format, args);
                }
                dlg.btnNo.Visible = false;
                dlg.btnYES.Visible = false;
                dlg.btnOK.Visible = true;
                dlg.AcceptButton = dlg.btnOK;
                dlg.CancelButton = dlg.btnOK;
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// 弹出“是、否”的对话框，返回对话框结果（长内容，带拖动条），YES - No 
        /// </summary>
        public static bool ShowYesNoEx(string format, params object[] args)
        {
            using (frmMsgBox dlg = new frmMsgBox())
            {
                dlg.Picture = JZTMsgDialogIcon.Question;
                dlg.txtContent.Text = string.Format(format, args);
                dlg.btnNo.Visible = true;
                dlg.btnYES.Visible = true;
                dlg.btnOK.Visible = false;
                dlg.AcceptButton = dlg.btnYES;
                dlg.CancelButton = dlg.btnNo;
                dlg.ControlBox = false;
                return (dlg.ShowDialog() == DialogResult.Yes);
            }
        }
    }
}
