using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Core.UI;

namespace Core.UI.Controls.ControlAdapter
{
    public  class ToolStripAdapter : BaseControlAdapter
    {
        //public ToolStripAdapter()
        //{
        //    InitializeComponent();
        //}

        //public ToolStripAdapter(IContainer container)
        //{
        //    container.Add(this);

        //    InitializeComponent();
        //}


        public ToolStrip ToolStrip { get; set; }

        public void BindToolStrip(ToolStrip toolStrip)
        {
            ToolStrip = toolStrip;
        }


        private ToolStripItem CreateTreeNode(UIDisplayObject uiDisplayObject)
        {
            //if (OnBeforeCreateTreeNode != null)
            //{ }

            //UiDisplayObject root =  UiDisplayManager.CreateTreeStucture();

            ToolStripItem node;

            if (true)
            {
                node = new ToolStripButton();
                node.Text = uiDisplayObject.Title;
                node.Tag = uiDisplayObject;

                node.Click += new EventHandler(node_Click);
            }
            else
            {

                //如果该结点没有子结点，就生成 button ,如果有子结点则生成。

                //if (OnAfterCreateTreeNode != null)
                //{ }

                node = new ToolStripDropDownButton();
            }

            return node;
        }

        void node_Click(object sender, EventArgs e)
        {
            //JZT.Common.Others.Command.BaseCommand command = ((sender as ToolStripButton).Tag as UIDisplayObject).RealObject as JZT.Common.Others.Command.BaseCommand;

            //command.Execute();

              
            
        }



        private ToolStripItem CreateTreeNode(List<UIDisplayObject> list)
        {
            ToolStripItem rootNode = new ToolStripButton();

            foreach (UIDisplayObject item in list)
            {
                ToolStripItem node = CreateTreeNode(item);

                //可以引出事件自行再修改创建 好的 node .

                //查找父
                //ToolStripItem parentNode = GetParentNode(rootNode, item);

                //parentNode.Add(node);

                ToolStrip.Items.Add(node);
            }
            return rootNode;
        }


        #region 需要重载的方法

        public override void Display()
        {
            ToolStrip.Items.Clear();

            //ToolStripButton item = new  ToolStripButton();
            //item.Text = "这是测试的";

            //ToolStrip.Items.Add(item);

            CreateTreeNode(UiDisplayManager.UiDisplayObjectList);
 
        }



        #endregion


    }
}
