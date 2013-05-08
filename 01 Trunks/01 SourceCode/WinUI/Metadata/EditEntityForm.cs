using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.UI;
using Core.Metadata.CSharpDesign;
using Core.Metadata.HBMDesign;
using Core.Metadata.SQLDesign;

namespace Core.Metadata
{
    public partial class EditEntityForm : Form, ISingleObjectDisplay
    {
        public EditEntityForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
                CommonUIinteractive.NewObject();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            CommonUIinteractive.GetObject("Entity1");

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            CommonUIinteractive.Save();
        }

        #region ISingleObjectDisplay 成员

        public void Dispaly(object obj)
        {
            editEntityControl1.SetEditedObject(obj);

            //ShowDialog();
        }

        public EditSingleObjectUIC CommonUIinteractive { get; set; }

        #endregion

        private CSharpClassDefine classDefine;

        private void button1_Click(object sender, EventArgs e)
        {

            classDefine = new CSharpClassDefine(CommonUIinteractive.EditedObject as BusiEntity);

            richTextBox1.Text = classDefine.GenCode();

            sqlBlockDefine = new SQLBlockDefine();

            TableDefine tableDefine = new TableDefine(CommonUIinteractive.EditedObject as BusiEntity);
            sqlBlockDefine.TableDefineList.Add(tableDefine);

            richTextBox3.Text = sqlBlockDefine.GenCode();

            hbmBlockDefine = new HBMBlockDefine();

            hbmBlockDefine.Assembly = "JZT.Common";
            hbmBlockDefine.NameSpace = "JZT.GOS.Modal.Basis.Entity";

            HBMClassDefine hbmClassDefine = new HBMClassDefine(CommonUIinteractive.EditedObject as BusiEntity,classDefine, sqlBlockDefine.TableDefineList[0]);

            hbmBlockDefine.HBMClassDefineList.Add(hbmClassDefine);

            richTextBox2.Text = hbmBlockDefine.GenCode();


            


        }


        private HBMBlockDefine hbmBlockDefine;

        private void button2_Click(object sender, EventArgs e)
        {
            hbmBlockDefine = new HBMBlockDefine();

            hbmBlockDefine.Assembly = "JZT.Common";
            hbmBlockDefine.NameSpace = "JZT.GOS.Modal.Basis.Entity";

           // HBMClassDefine hbmClassDefine = new HBMClassDefine(classDefine, sqlBlockDefine[ .TableDefineList[0]);

            //hbmBlockDefine.HBMClassDefineList.Add(hbmClassDefine);

            richTextBox2.Text = hbmBlockDefine.GenCode();

        }

        private SQLBlockDefine sqlBlockDefine;
        private void button3_Click(object sender, EventArgs e)
        {
            sqlBlockDefine = new SQLBlockDefine();

            TableDefine tableDefine = new TableDefine(CommonUIinteractive.EditedObject as BusiEntity);
            sqlBlockDefine.TableDefineList.Add(tableDefine);

            richTextBox3.Text = sqlBlockDefine.GenCode();
        }

     }
}
