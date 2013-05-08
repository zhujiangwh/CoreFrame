using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Core.UI;
using Core.Architecure;
using Core.UI.Controls.ControlAdapter;
using Core.Common;
using Core.Serialize.DB;
using System.Collections;
using Core.Metadata;
using Core.Serialize.XML;
using JZT.Utility;

namespace WinUI
{
    public partial class MainForm : Form , IObjectDisplay
    {
        private RemotingServer remotingServer; //= new RemotingServer("tcp", "127.0.0.1:8545");

        public MainForm()
        {
            InitializeComponent();

            CommonUIinteractive = new CommonUIinteractive();

            remotingServer = WinApplication.GetInstance().RemotingServer;


            CommonUIinteractive.CommonObjectService = remotingServer.CreateRemotingInterface<ICommonObjectService>("CommonObjectService");

            CommonUIinteractive.EditedObjectDefine.AssemblyName = "Core";
            CommonUIinteractive.EditedObjectDefine.FullClassName = "Core.Architecure.SoftSystem";


            CommonUIinteractive.EditFormDefine.AssemblyName = "WinUI";
            CommonUIinteractive.EditFormDefine.FullClassName = "Core.Architecure.EditSoftSystemForm";

            CommonUIinteractive.EditModuleFormDefine.AssemblyName = "WinUI";
            CommonUIinteractive.EditModuleFormDefine.FullClassName = "Core.Architecure.EditSoftModuleForm";


            CommonUIinteractive.Initial();

            CommonUIinteractive.DisplayForm = this;

            
        }

        //private CommonUIinteractive UIInteractive { get; set; }

        #region 选单

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonUIinteractive.NewObject();
        }

        #endregion

        #region IObjectDisplay 成员

        public void Dispaly(object obj)
        {
            SoftSystem softSystem = obj as SoftSystem;

            //清空树.
            tvSoftSystem.Nodes.Clear();

            //显示新创建软件系统.
            UiDisplayManager uiDisplayManager = new UiDisplayManager();



            TreeViewAdapter tvd1 = new TreeViewAdapter();
            tvd1.BindTreeView(tvSoftSystem);
            uiDisplayManager.Regist(tvd1);

            List<object> list = new List<object>();
            List<UIDisplayObject> dk = softSystem.Convert().GetAllList();


            TreeNodeAdapter softSystemNode = new TreeNodeAdapter();

            TreeNode node = softSystemNode.CreateTreeNode(softSystem.Convert().GetAllList());

            tvSoftSystem.Nodes.Add(node.Nodes[0]);
        }


        public CommonUIinteractive CommonUIinteractive { get; set; }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //新建模块。
            if (tvSoftSystem.SelectedNode != null && tvSoftSystem.SelectedNode.Tag != null)
            {
                SoftSystem softSystem = (tvSoftSystem.SelectedNode.Tag as UIDisplayObject).RealObject as SoftSystem;
                if (softSystem != null)
                {
                    SoftModule softModule = CommonUIinteractive.NewSoftModule(softSystem);
                }

                SoftModule softModule2 = (tvSoftSystem.SelectedNode.Tag as UIDisplayObject).RealObject as SoftModule;
                if (softModule2 != null)
                {
                    SoftModule softModule1 = CommonUIinteractive.NewSoftModule(softModule2);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CommonUIinteractive.GetObject(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EditEntityForm form = new EditEntityForm();

            EditSingleObjectUIC uic = new EditSingleObjectUIC();

            uic.EditForm = form;
            form.CommonUIinteractive = uic;

            uic.NewObject();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ISelectObjectService selectObjectService = remotingServer.CreateRemotingInterface<ISelectObjectService>();
            SelectObjectUIC uic = selectObjectService.LoadSelectObjectUIC (@"SelectModule");

            uic.Initial();

            DataRow row = uic.GetSelectedRow();

            //selectObjectService.SaveSelectObjectUIC(uic);
        }

        private void 测试对象ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DispalyForm(new TestObjectForm());
        }

        public void DispalyForm(Form form)
        {
            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        private void 编辑所有对象ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditAllObjectUIC uic = new EditAllObjectUIC();

            uic.EditAllObjectDisplayUIDefine.AssemblyName = "WinUI";
            uic.EditAllObjectDisplayUIDefine.FullClassName = "Core.UI.EditAllObjectForm";

            uic.ObjectTypeDefine.AssemblyName = "Core";
            uic.ObjectTypeDefine.FullClassName = "Core.Serialize.XML.XmlSerializeDefine";

            uic.Initial();


            DispalyForm(uic.EditAllObjectDisplayUI as Form);

        }

        private void 编辑对象ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSingleObjectUIC uic = new EditSingleObjectUIC();

            uic.EditedObjectDefine.AssemblyName = "Core";
            uic.EditedObjectDefine.FullClassName = "Core.Metadata.BusiEntity";

            uic.EditFormDefine.AssemblyName = "WinUI";
            uic.EditFormDefine.FullClassName = "Core.Metadata.EditEntityForm";

            uic.Initial();
            uic.NewObject();

            DispalyForm(uic.EditForm as Form);

        }

    }
}
