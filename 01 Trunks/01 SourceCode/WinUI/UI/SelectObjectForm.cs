using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.Serialize.DB;
using Core.Metadata;

namespace Core.UI
{
    public partial class SelectObjectForm : Form, ISelectObject
    {
        private SelectObjectUIC SelectObjectUIControl { get; set; }

        public SelectObjectForm()
        {
            InitializeComponent();
        }

        #region ISelectObject 成员

        //private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        //private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
        //private System.Windows.Forms.DataGridViewImageColumn Column4;
        //private System.Windows.Forms.DataGridViewLinkColumn Column5;
        //private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        //private System.Windows.Forms.DataGridViewButtonColumn Column7;


        public DataRow Display(DataTable table)
        {
            DataRow row = null;
            //显示取得的数据集。


            foreach (DataItemDisplayer item in SelectObjectUIControl.DisplayerDefine.CloumnDisplayerList)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();

                if (col != null)
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    col.DataPropertyName = item.ColumnName;
                    col.HeaderText = item.DisplayLable  ;
                    col.Width = item.DisplayWidth;
                    //col.Resizable = DataGridViewTriState.False;
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView1.Columns.Add(col);
            }

            dataGridView1.DataSource = table;

            if (ShowDialog() == DialogResult.OK)
            {
                row = (dataGridView1.CurrentRow.DataBoundItem as DataRowView).Row;
            }

            return row;
        }



        public void DisplayParamList(SqlScript sqlScript)
        {
            label1.Text = "shdfks";
        }



        public void ConfigForm(SelectObjectUIC selectObjectUIC)
        {
            SelectObjectUIControl = selectObjectUIC;

            Text = selectObjectUIC.Title;
            Height = selectObjectUIC.FormHeight;
            Width = selectObjectUIC.FormWidth;

            toolStrip.Visible = selectObjectUIC.IsDisplayToolbar;
            panel1.Visible = selectObjectUIC.SqlScript != null;
        }

        #endregion

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #region ISelectObject 成员

        public void CreateParamsUI(SqlScript sqlScript)
        {
            //在参数区构建参数显示控件。
            int i = 0;

            foreach (SqlParam item in sqlScript.ParamList)
            {

                Label label = new Label();
                label.Text = item.DisplayLable;

                label.Left = 10;
                label.Top = i * 30 + 10;

                panel1.Controls.Add(label);

                if (item.ParamType == DbType.String)
                {
                    TextBox textbox = new TextBox();

                    textbox.Left = 150;
                    textbox.Top = i * 30 + 10;

                    panel1.Controls.Add(textbox);

                }

                if (item.ParamType == DbType.Boolean)
                {
                    CheckBox cb = new CheckBox();

                    cb.Left = 150;
                    cb.Top = i * 30 + 10;

                    cb.Text = item.DisplayLable;

                    panel1.Controls.Add(cb);
                }

                i++;


            }


        }

        #endregion
    }
}
