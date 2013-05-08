using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Core.UI;

namespace Core.UI.Controls.ControlAdapter
{
    public class TreeNodeAdapter
    {
        public event EventHandler OnAfterCreateTreeNode;

        public event EventHandler OnBeforeCreateTreeNode;

        public UiDisplayManager UiDisplayManager { get; set; }


        public TreeNodeAdapter()
        {
            UiDisplayManager = new UiDisplayManager();
        }


        private TreeNode CreateTreeNode(UIDisplayObject uiDisplayObject)
        {
            if (OnBeforeCreateTreeNode != null)
            { }

            TreeNode node = new TreeNode();
            node.Text = uiDisplayObject.Title;
            node.Tag = uiDisplayObject;

            if (OnAfterCreateTreeNode != null)
            { }

            return node;
        }

        public  TreeNode CreateTreeNode(List<UIDisplayObject> list)
        {
            UiDisplayManager.UiDisplayObjectList.AddRange(list);
            TreeNode rootNode = new TreeNode();

            foreach (UIDisplayObject item in UiDisplayManager.UiDisplayObjectList)
            {
                TreeNode node = CreateTreeNode(item);

                //可以引出事件自行再修改创建 好的 node .

                //查找父
                TreeNode parentNode = GetParentNode(rootNode, item);

                parentNode.Nodes.Add(node);
            }
            return rootNode;
        }

        private TreeNode GetParentNode(TreeNode rootNode, UIDisplayObject uiDisplayObject)
        {
            TreeNode node = rootNode;

            string ls = UiDisplayManager.LevelString.GetParentString(uiDisplayObject.LevelString);

            List<TreeNode> nodeList = GetTreeNodeList(rootNode);

            foreach (TreeNode item in nodeList)
            {
                UIDisplayObject UiDisplayObject = item.Tag as UIDisplayObject;

                if (UiDisplayObject == null)
                {
                    continue;
                }

                if (ls == UiDisplayObject.LevelString)
                {
                    node = item;
                    break;
                }
            }
            return node;
        }

        private List<TreeNode> GetTreeNodeList(TreeNode node)
        {
            List<TreeNode> list = new List<TreeNode>();

            list.Add(node);

            foreach (TreeNode item in node.Nodes)
            {
                list.AddRange(GetTreeNodeList(item));
            }

            return list;
        }



 
    }

    public class TreeViewAdapter : BaseControlAdapter
    {
        public event EventHandler OnAfterCreateTreeNode;

        public event EventHandler OnBeforeCreateTreeNode;
    
        public TreeView TreeView { get; set; }

        #region 需要重载的方法

        public override void SetIsMulSelect(bool isMulSelect)
        {
            TreeView.BeginUpdate();
            TreeView.CheckBoxes = base.UiDisplayManager.IsMulSelect;
            TreeView.EndUpdate();
        }


        public override void Display()
        {
            //创建节点
            TreeNode node = CreateTreeNode(UiDisplayManager.UiDisplayObjectList);

            TreeView.BeginUpdate();
            //清空
            TreeView.Nodes.Clear();

            foreach (TreeNode item in node.Nodes)
            {
                TreeView.Nodes.Add(item);
            }

            //展开
            TreeView.ExpandAll();
            TreeView.EndUpdate();
        }


        public override void FocusNode(UIDisplayObject uiDisplayObject)
        {
            TreeNode treeNode = Find(uiDisplayObject);

            if (treeNode != null)
            {
                TreeView.SelectedNode = treeNode;
            }
        }


        public override void SetChecked(UIDisplayObject uiDisplayObject, bool checkedFlag)
        {
            TreeNode treeNode = Find(uiDisplayObject);

            if (treeNode != null && treeNode.Checked != checkedFlag)
            {
                treeNode.Checked = checkedFlag;
            }
        }

        public override void NewUiDisplayObject(UIDisplayObject uiDisplayObject)
        {
            TreeNode node = CreateTreeNode(uiDisplayObject);
            //为当前结点增加一个树结点。
            UIDisplayObject parent = UiDisplayManager.GetParent(uiDisplayObject);
            TreeNode parentTreeNode = Find(parent);

            if (parentTreeNode == null)
            {
                TreeView.Nodes.Add(node);
            }
            else
            {
                parentTreeNode.Nodes.Add(node);
            }
        }

        public override void InsertUiDisplayObject(UIDisplayObject uiDisplayObject, UIDisplayObject curUIDisplayObject)
        {
            TreeNode node = CreateTreeNode(uiDisplayObject);

            //为当前结点增加一个树结点。
            UIDisplayObject parent = UiDisplayManager.GetParent(uiDisplayObject);
            TreeNode parentTreeNode = Find(parent);

            List<UIDisplayObject> list = UiDisplayManager.GetChild(parent == null ? string.Empty : parent.LevelString);

            //获取当前节点在列表中的索引，然后插入在控件中的指定位置中。
            int index = list.IndexOf(uiDisplayObject);

            if (parentTreeNode == null)
            {
                TreeView.Nodes.Insert(index, node);
            }
            else
            {
                parentTreeNode.Nodes.Insert(index, node);
            }
        }

        public override void DelUiDisplayObject(UIDisplayObject uiDisplayObject)
        {
            //找到当前结点，删除 。
            TreeNode treeNode = Find(uiDisplayObject);

            if (treeNode != null)
            {
                treeNode.Remove();
            }
        }

        #endregion


        #region 绑定树操作

        public void BindTreeView(TreeView treeView)
        {
            TreeView = treeView;

            treeView.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
            treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
            treeView.KeyPress += new KeyPressEventHandler(treeView_KeyPress);
            treeView.DoubleClick += new EventHandler(treeView_DoubleClick);
            treeView.MouseUp += new MouseEventHandler(treeView_MouseUp);

        }

        void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            TreeNode node = TreeView.GetNodeAt(e.X, e.Y);

            if (node != null)
            {
                //JZT.Common.Others.Command.BaseCommand command = ((node as TreeNode).Tag as UiDisplayObject).RealObject as JZT.Common.Others.Command.BaseCommand;
                //command.Execute();
            }
        }

  

        void treeView_DoubleClick(object sender, EventArgs e)
        {
            //如果不让触发就跳出。
            if (!UiDisplayManager.AllowLauchControlEvent)
            {
                return;
            }

            //触发 选定事件。
            UiDisplayManager.LaugchOnEditUiDisplayObject();
        }

        void treeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果不让触发就跳出。
            if (!UiDisplayManager.AllowLauchControlEvent)
            {
                return;
            }

            //触发 选定事件。
            if (e.KeyChar == (char)13) 
            {
                //触发 选定事件。
                UiDisplayManager.LaugchOnEditUiDisplayObject();
            }
        }

        void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                UIDisplayObject uiDisplayObject = e.Node.Tag as UIDisplayObject;

                //改变当前对象，内部会触发改变事件。
                UiDisplayManager.Current = uiDisplayObject;
            }
        }

        void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //如果不让触发就跳出。
            if (!UiDisplayManager.AllowLauchControlEvent)
            {
                return;
            }

            UiDisplayManager.SetCheckBox(e.Node.Tag as UIDisplayObject, e.Node.Checked);
        }


        #endregion

        #region 移动

        public override void MoveFrior()
        {
            TreeNode parentNode = TreeView.SelectedNode.Parent;
            TreeNode curNode = TreeView.SelectedNode;

            TreeNodeCollection nodes;
            if (parentNode == null)
            {
                nodes = TreeView.Nodes;
            }
            else
            {
                nodes = parentNode.Nodes;
            }

            int index =  nodes.IndexOf(curNode);
            nodes.Remove(curNode);
            nodes.Insert(index - 1, curNode);
        }

        public override void MoveNext()
        {
            TreeNode parentNode = TreeView.SelectedNode.Parent;
            TreeNode curNode = TreeView.SelectedNode;

            TreeNodeCollection nodes;
            if (parentNode == null)
            {
                nodes = TreeView.Nodes;
            }
            else
            {
                nodes = parentNode.Nodes;
            }

            int index = nodes.IndexOf(curNode);
            nodes.Remove(curNode);
            nodes.Insert(index + 1, curNode);
        }

        #endregion


        #region 控件 工具方法

        /// <summary>
        /// 根据对象的Key,取得对象的节点。
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private TreeNode GetNode(string Key)
        {
            return null;
        }



        private TreeNode CreateTreeNode(UIDisplayObject uiDisplayObject)
        {
            if (OnBeforeCreateTreeNode != null)
            { }

            TreeNode node = new TreeNode();
            node.Text = uiDisplayObject.Title;
            node.Tag = uiDisplayObject;

            if (OnAfterCreateTreeNode != null)
            { }

            return node;
        }

        private TreeNode CreateTreeNode(List<UIDisplayObject> list)
        {
            TreeNode rootNode = new TreeNode();

            foreach (UIDisplayObject item in UiDisplayManager.UiDisplayObjectList)
            {
                TreeNode node = CreateTreeNode(item);

                //可以引出事件自行再修改创建 好的 node .

                //查找父
                TreeNode parentNode = GetParentNode(rootNode, item);

                parentNode.Nodes.Add(node);
            }
            return rootNode;
        }


        private TreeNode GetParentNode(TreeNode rootNode, UIDisplayObject uiDisplayObject)
        {
            TreeNode node = rootNode;

            string ls = UiDisplayManager.LevelString.GetParentString(uiDisplayObject.LevelString);

            List<TreeNode> nodeList = GetTreeNodeList(rootNode);

            foreach (TreeNode item in nodeList)
            {
                UIDisplayObject UiDisplayObject = item.Tag as UIDisplayObject;

                if (UiDisplayObject == null)
                {
                    continue;
                }

                if (ls == UiDisplayObject.LevelString)
                {
                    node = item;
                    break;
                }
            }
            return node;
        }

        private List<TreeNode> GetTreeNodeList(TreeNode node)
        {
            List<TreeNode> list = new List<TreeNode>();

            list.Add(node);

            foreach (TreeNode item in node.Nodes)
            {
                list.AddRange(GetTreeNodeList(item));
            }

            return list;
        }

        private List<TreeNode> GetTreeNodeList(TreeView tree)
        {
            List<TreeNode> list = new List<TreeNode>();

            foreach (TreeNode node in tree.Nodes)
            {
                list.AddRange(GetTreeNodeList(node));
            }

            return list;
        }




        public TreeNode Find(UIDisplayObject uiDisplayObject)
        {
            TreeNode treeNode = null;

            foreach (TreeNode node in GetTreeNodeList(TreeView))
            {
                if (node.Tag == uiDisplayObject)
                {
                    treeNode = node;
                    break;
                }
            }

            return treeNode;
        }


        #endregion


    }
}
