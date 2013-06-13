using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using Core.Architecure;

namespace Core.UI
{
    public partial class EditAllObjectForm : Form, IEditAllObjectDisplay
    {
        public EditAllObjectForm()
        {
            InitializeComponent();
        }

        #region IEditAllObjectDisplay 成员

        public void Dispaly(System.Collections.IList obj)
        {
            listBox1.DataSource = null;

            listBox1.DataSource = obj;
            listBox1.DisplayMember = "FullClassName";

        }

        public System.Collections.IList GetObejctList()
        {
            return listBox1.DataSource as IList;
        }

        public EditAllObjectUIC EditAllObjectUIC { get; set; }

        public IObjectEditer ObjectEditer { get; set; }

        public void AddEditControl( IObjectEditer objectEditer)
        {

            ObjectEditer = objectEditer;
            Control con = objectEditer as Control;

            if (con != null)
            {
                panel1.Controls.Add(con);

                con.Dock = DockStyle.Fill;
            }
 
        }

        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            EditAllObjectUIC.Save();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            IList list = EditAllObjectUIC.EditObjectList;

            if (listBox1.SelectedIndex >= 0)
            {
                //propertyGrid1.SelectedObject = list[listBox1.SelectedIndex];

                ObjectEditer.EditedObject = list[listBox1.SelectedIndex];
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            EditAllObjectUIC.GetAllObject();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            EditAllObjectUIC.NewObject();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Assembly asm = Assembly.Load("Core");//.LoadFrom("Core.dll");

            Type[] s = asm.GetTypes();

            ArrayList arrayList = new ArrayList();

            foreach (Type t in s)
            {
                ObjectDefine objectDefine = new ObjectDefine(t);

                arrayList.Add(objectDefine);


            }

            EditAllObjectUIC.EditObjectList = arrayList;

        }
    }
}
