using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Core.UI;


namespace Core.UI.Controls.ControlAdapter
{
    public class ListViewAdapter : BaseControlAdapter
    {
        public event EventHandler OnAfterCreateListViewItem;

        public event EventHandler OnBeforeCreateListViewItem;
    
        //public ListViewAdapter()
        //{
        //    InitializeComponent();
        //}

        //public ListViewAdapter(IContainer container)
        //{



        //    //直接由控件本身生成列。

        //    container.Add(this);

        //    InitializeComponent();
        //}

        #region 绑定listview
        public ListView ListView { get; set; }

        public void BindTreeView(ListView listView)
        {
            ListView = listView;

            ListView.DoubleClick += new EventHandler(ListView_DoubleClick);
            ListView.Click += new EventHandler(ListView_Click);
            ListView.SelectedIndexChanged += new EventHandler(ListView_SelectedIndexChanged);
            ListView.ItemChecked += new ItemCheckedEventHandler(ListView_ItemChecked);

            ListView.KeyPress += new KeyPressEventHandler(ListView_KeyPress);
        }

        void ListView_KeyPress(object sender, KeyPressEventArgs e)
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

        void ListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

            UiDisplayManager.SetCheckBox(e.Item.Tag as UIDisplayObject, e.Item.Checked);
        }

        void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //如果不让触发就跳出。
            if (!UiDisplayManager.AllowLauchControlEvent)
            {
                return;
            }

            if (ListView.FocusedItem != null && ListView.FocusedItem.Tag != null)
            {
                UIDisplayObject uiDisplayObject = ListView.FocusedItem.Tag as UIDisplayObject;

                //改变当前对象，内部会触发改变事件。
                UiDisplayManager.Current = uiDisplayObject;
            }
        }

        void ListView_Click(object sender, EventArgs e)
        {
            //如果不让触发就跳出。
            if (!UiDisplayManager.AllowLauchControlEvent)
            {
                return;
            }



        }

        void ListView_DoubleClick(object sender, EventArgs e)
        {
            //如果不让触发就跳出。
            if (!UiDisplayManager.AllowLauchControlEvent)
            {
                return;
            }

            //触发 选定事件。
            UiDisplayManager.LaugchOnEditUiDisplayObject();

        }
        #endregion

        private ListViewItem CreateListViewItem(UIDisplayObject uiDisplayObject)
        {
            if (OnBeforeCreateListViewItem != null)
            { }

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = uiDisplayObject.Title;
            listViewItem.Tag = uiDisplayObject;
            listViewItem.ImageKey = uiDisplayObject.Image16Key;

            listViewItem.SubItems.Add(uiDisplayObject.Key);


            if (OnAfterCreateListViewItem != null)
            { 
            }

            return listViewItem;
        }

        #region 需要重载的方法

        public override void Display()
        {
            ListView.BeginUpdate();
            ListView.Clear();

            //根据列的定义生成列。只在第一次初始化的时候生成列。
            if (true)
            {
                //foreach ( ColumnDisplayer item  in UiDisplayManager. DisplayStyleDefine.CloumnDisplayerList)
                //{
                //    ColumnHeader col = new ColumnHeader();
                //    col.DisplayIndex = item.VisibleIndex ;
                //    col.Text = item.DisplayLable ;
                //    col.Width = item.DisplayWidth;

                //    ListView.Columns.Add(col);
                //}
            }


            //生成控件并加载到控件中。如果是自动生成列则不处理。

            foreach (UIDisplayObject item in UiDisplayManager.UiDisplayObjectList)
            {
                ListViewItem listViewItem = CreateListViewItem(item);



                ListView.Items.Add(listViewItem);
            }

            //如果是自动生成列，还要根据列定义调整列。
            if (false)
            { }

            ListView.EndUpdate();
        }

        public override void SetIsMulSelect(bool isMulSelect)
        {
            ListView.BeginUpdate();
            ListView.CheckBoxes = UiDisplayManager.IsMulSelect;
            ListView.EndUpdate();
        }

        public override void FocusNode(UIDisplayObject uiDisplayObject)
        {
            ListViewItem listViewItem = Find(uiDisplayObject);

            if (listViewItem != null)
            {
                ListView.FocusedItem = listViewItem;
                ListView.Focus();
            }
        }

        public override void SetChecked(UIDisplayObject uiDisplayObject, bool checkedFlag)
        {
            ListViewItem treeNode = Find(uiDisplayObject);

            if (treeNode != null && treeNode.Checked != checkedFlag)
            {
                treeNode.Checked = checkedFlag;
            }
        }

        #endregion

        public ListViewItem Find(UIDisplayObject uiDisplayObject)
        {
            ListViewItem listViewItem = null;

            foreach (ListViewItem node in ListView.Items)
            {
                if (node.Tag == uiDisplayObject)
                {
                    listViewItem = node;
                    break;
                }
            }

            return listViewItem;
        }

        #region 移动

        public override void MoveFrior()
        {
            ListViewItem curNode = ListView.FocusedItem;

            int index = ListView.Items.IndexOf(curNode);

            ListView.Items.Remove(curNode);
            ListView.Items.Insert(index -1 ,curNode);
        }

        public override void MoveNext()
        {
            ListViewItem curNode = ListView.FocusedItem;

            int index = ListView.Items.IndexOf(curNode);

            ListView.Items.Remove(curNode);
            ListView.Items.Insert(index + 1, curNode);
        }

        #endregion



    }
}
