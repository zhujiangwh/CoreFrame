﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

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
                propertyGrid1.SelectedObject = list[listBox1.SelectedIndex];
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
    }
}