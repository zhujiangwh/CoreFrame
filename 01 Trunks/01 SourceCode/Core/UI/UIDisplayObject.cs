using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Common;
using System.Collections;
using System.Data;

namespace Core.UI
{
    public interface IDisplayUIObject
    {
        UIDisplayObject Convert();

        void Load(UIDisplayObject uiDisplayObject);
    }

    [Serializable]
    public class UIDisplayObject : IDisplayUIObject
    {
        public string Key { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public string LevelString { get; set; }
        public int Index { get; set; }

        //图标控制
        public string Image16Key { get; set; }
        public string SelectedImage16Key { get; set; }
        public string Image24Key { get; set; }

        public object RealObject { get; set; }

        public List<UIDisplayObject> List { get; set; }
        public UIDisplayObject Parent { get; set; }


        public UIDisplayObject()
        { 
            List = new List<UIDisplayObject>(); 
        }

        #region IDisplayUIObject 成员

        public UIDisplayObject Convert()
        {
            return this ;
        }

        public void Load(UIDisplayObject uiDisplayObject)
        {
        }

        #endregion


        public List<UIDisplayObject> GetAllList()
        {
            List<UIDisplayObject> uiDisplayObjectlist = new List<UIDisplayObject>();

            uiDisplayObjectlist.Add(this);

            foreach (UIDisplayObject item in List)
            {
                uiDisplayObjectlist.AddRange(item.GetAllList());
            }

            return uiDisplayObjectlist;
        }
    }


    public enum DisplayMode
    {
        DataTable, ObjectList
    }


    public class UiDisplayManager : IList
    {
        public LevelString LevelString { get; set; }

        public bool AllowLauchControlEvent { get; set; }

        //public TableDisplayer DisplayStyleDefine { get; set; }


        #region 当前对象改变。
        public UIDisplayObject _current;
        public UIDisplayObject Current
        {
            get
            {
                return _current;
            }
            set
            {
                if (value != _current)
                {
                    _current = value;
                    CurrentChanged(_current);
                }
            }
        }

        protected void CurrentChanged(UIDisplayObject uiDisplayObject)
        {
            foreach (BaseControlAdapter item in ControlAdapterList)
            {
                item.FocusNode(uiDisplayObject);
            }

            //触发当前对象改变事件。
            if (OnCurrentChanged != null)
            {
                //会更新。控件的显示 。

            }
        }

        public int SelectIndex
        {
            get
            {
                return IndexOf(Current);
            }
        }
        #endregion

        #region 选择相关

        /// <summary>
        /// 是否采用级联选择，使用树型控件的时候会用这个属性。
        /// </summary>
        public bool CascadeSelect { get; set; }

        protected bool _isMulSelect;

        public void SetCheckBox(UIDisplayObject uiDisplayObject, bool checkFlag)
        {
            if (checkFlag)
            {
                if (!UiDisplayObjectList_Selected.Contains(uiDisplayObject))
                {
                    UiDisplayObjectList_Selected.Add(uiDisplayObject);
                }
                else
                {
                    //异常
                }
            }
            else
            {
                if (UiDisplayObjectList_Selected.Contains(uiDisplayObject))
                {
                    UiDisplayObjectList_Selected.Remove(uiDisplayObject);
                }
                else
                {
                    //异常
                }

            }

            foreach (BaseControlAdapter item in ControlAdapterList)
            {
                item.SetChecked(uiDisplayObject, checkFlag);
            }
        }

        public void SelectAll(bool flag)
        {
            foreach (UIDisplayObject item in UiDisplayObjectList)
            {
                SetCheckBox(item, true);
            }
        }

        public void SelectCurrent(bool flag)
        {
            if (Current != null)
            {
                SetCheckBox(Current, true);
            }
        }

        private void SetIsMulSelect()
        {
            foreach (BaseControlAdapter item in ControlAdapterList)
            {
                item.SetIsMulSelect(IsMulSelect);
            }

        }

        public bool IsMulSelect
        {
            get { return _isMulSelect; }
            set { _isMulSelect = value; SetIsMulSelect(); }
        }

        public List<UIDisplayObject> UiDisplayObjectList { get; set; }

        protected List<UIDisplayObject> UiDisplayObjectList_New { get; set; }
        protected List<UIDisplayObject> UiDisplayObjectList_Modified { get; set; }
        protected List<UIDisplayObject> UiDisplayObjectList_Delete { get; set; }
        protected List<UIDisplayObject> UiDisplayObjectList_Selected { get; set; }


        public event EventHandler OnBeforeCreateUIObject;
        public event EventHandler OnAfterCreateUIObject;

        public event EventHandler OnCurrentChanged;


        protected List<BaseControlAdapter> ControlAdapterList { get; set; }


        public List<UIDisplayObject> GetSelectedUiDisplayObjectList()
        {
            return UiDisplayObjectList_Selected;
        }


        #endregion 缺省行为
        public event EventHandler OnEditUiDisplayObject;

        public void LaugchOnEditUiDisplayObject()
        {
            if (OnEditUiDisplayObject != null)
            {
                //触发缺省行为事件。
            }
        }

        #region

        #endregion


        public UiDisplayManager()
        {
            UiDisplayObjectList = new List<UIDisplayObject>();

            UiDisplayObjectList_New = new List<UIDisplayObject>();
            UiDisplayObjectList_Modified = new List<UIDisplayObject>();
            UiDisplayObjectList_Delete = new List<UIDisplayObject>();
            UiDisplayObjectList_Selected = new List<UIDisplayObject>();

            ControlAdapterList = new List<BaseControlAdapter>();

            LevelString = new  LevelString();
            LevelString.AddSections(2, 2, 2, 2, 2);

            IsMulSelect = false; //启用多择。
            CascadeSelect = false;//级联选择。
        }


        public void Initial()
        {
            AllowLauchControlEvent = true;
            UiDisplayObjectList.Clear();

            UiDisplayObjectList_New.Clear();
            UiDisplayObjectList_Modified.Clear();
            UiDisplayObjectList_Delete.Clear();
            UiDisplayObjectList_Selected.Clear();
        }

        #region 显示控件接口

        public void Regist(BaseControlAdapter ControlAdapter)
        {
            ControlAdapter.UiDisplayManager = this;
            ControlAdapterList.Add(ControlAdapter);
        }


        public void UnRegist(BaseControlAdapter ControlAdapter)
        {
            ControlAdapter.UiDisplayManager = null;
            ControlAdapterList.Remove(ControlAdapter);
        }

        #endregion

        #region 将传入的对象列表，转换为内部格式列表。

        public DisplayMode DisplayMode { get; set; }

        public string KeyField { get; set; }
        public string LevelField { get; set; }
        public string TitleField { get; set; }

        public UIDisplayObject ConverToUiDisplayObject(DataRow row)
        {
            UIDisplayObject ui = new UIDisplayObject();

            //触发事前事件。
            if (OnBeforeCreateUIObject != null)
            {
            }

            ui.Key = row[KeyField].ToString();
            ui.Title = row[TitleField].ToString();
            ui.LevelString = row[LevelField].ToString();

            ui.RealObject = row;

            //触发事后事件。
            if (OnAfterCreateUIObject != null)
            {
            }

            return ui;
        }

        public UIDisplayObject ConverToUiDisplayObject(object item)
        {
            UIDisplayObject ui = new UIDisplayObject();

            UIDisplayObject obj = item as UIDisplayObject;

            ui = obj;

            //触发事前事件。
            if (OnBeforeCreateUIObject != null)
            {
            }

            //触发事后事件。
            if (OnAfterCreateUIObject != null)
            {
            }
            return ui;
        }

        public object _dataSource;
        public object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                if (value is DataTable)
                {
                    ConvertToList(value as DataTable);
                }
                else
                {
                    ConvertToList(value as List<object>);
                }

                _dataSource = value;
            }
        }


        private void ConvertToList(DataTable table)
        {
            DisplayMode = DisplayMode.DataTable;
            //根据 table 预定义的列对应关系 转换 为内部列表。

            Initial();

            foreach (DataRow row in table.Rows)
            {
                UIDisplayObject ui = ConverToUiDisplayObject(row);


                //可以引出事件由外界再
                UiDisplayObjectList.Add(ui);
            }
        }

        private void ConvertToList(List<object> objList)
        {
            DisplayMode = DisplayMode.ObjectList;

            UiDisplayObjectList.Clear();

            foreach (object item in objList)
            {
                UIDisplayObject ui = ConverToUiDisplayObject(item);
                UiDisplayObjectList.Add(ui);
            }
        }

        #endregion

        #region 自定义

        /// <summary>
        /// 获取顶级的子结点。
        /// </summary>
        /// <param name="levelString"></param>
        /// <returns></returns>
        public List<UIDisplayObject> GetTopLevelChild()
        {
            return GetChild(string.Empty);
        }

        public List<UIDisplayObject> GetChild(UIDisplayObject uiDisplayObject)
        {
            return GetChild(uiDisplayObject == null ? string.Empty : uiDisplayObject.LevelString);
        }

        public List<UIDisplayObject> GetChild(string levelString)
        {
            int curLevelLen = levelString.Length;

            int levelCount = LevelString.GetSectionCount(levelString);
            int nextChildLevelLen = LevelString.GetSectionEnd(++levelCount);

            List<UIDisplayObject> list = UiDisplayObjectList.FindAll(delegate(UIDisplayObject c3) { return (c3.LevelString.Length > curLevelLen && c3.LevelString.Length == nextChildLevelLen) && c3.LevelString.IndexOf(levelString) == 0; });
            //list.Sort();
            return list;
        }


        public UIDisplayObject GetPriorChild(UIDisplayObject uiDisplayObject)
        {
            return GetPriorChild(uiDisplayObject.LevelString);
        }

        public UIDisplayObject GetPriorChild(string levelString)
        {
            string parentLevelString = LevelString.GetParentString(levelString);
            List<UIDisplayObject> list = GetChild(parentLevelString);

            UIDisplayObject uiDisplayObject = list.Find(delegate(UIDisplayObject c3) { return c3.LevelString == levelString; });
            UIDisplayObject priorNode = uiDisplayObject;

            int index = list.IndexOf(uiDisplayObject);
            if (index != -1 && index > 0)
            {
                priorNode = list[index - 1];
            }

            return priorNode;
        }

        public UIDisplayObject GetNextChild(UIDisplayObject uiDisplayObject)
        {
            return GetNextChild(uiDisplayObject.LevelString);
        }

        public UIDisplayObject GetNextChild(string levelString)
        {
            string parentLevelString = LevelString.GetParentString(levelString);
            List<UIDisplayObject> list = GetChild(parentLevelString);

            UIDisplayObject uiDisplayObject = list.Find(delegate(UIDisplayObject c3) { return c3.LevelString == levelString; });
            UIDisplayObject priorNode = uiDisplayObject;

            int index = list.IndexOf(uiDisplayObject);
            if (index != -1 && index < list.Count - 1)
            {
                priorNode = list[index + 1];
            }

            return priorNode;
        }

        public UIDisplayObject GetLastChild(UIDisplayObject uiDisplayObject)
        {
            return GetLastChild(uiDisplayObject == null ? string.Empty : uiDisplayObject.LevelString);
        }

        public UIDisplayObject GetLastChild(string levelString)
        {
            UIDisplayObject uiDisplayObject = null;

            List<UIDisplayObject> list = GetChild(levelString);

            if (list.Count > 0)
            {
                uiDisplayObject = list[list.Count - 1];
            }

            return uiDisplayObject;
        }

        public UIDisplayObject GetFirstChild(UIDisplayObject uiDisplayObject)
        {
            return GetFirstChild(uiDisplayObject == null ? string.Empty : uiDisplayObject.LevelString);
        }

        public UIDisplayObject GetFirstChild(string levelString)
        {
            UIDisplayObject uiDisplayObject = null;

            List<UIDisplayObject> list = GetChild(levelString);

            if (list.Count > 0)
            {
                uiDisplayObject = list[0];
            }

            return uiDisplayObject;
        }

        public List<UIDisplayObject> GetAllChild(UIDisplayObject uiDisplayObject)
        {
            return GetAllChild(uiDisplayObject.LevelString);
        }

        public List<UIDisplayObject> GetAllChild(string levelString)
        {
            int curLevelLen = levelString.Length;
            //大于本级长度的所有下一级。
            List<UIDisplayObject> list = UiDisplayObjectList.FindAll(delegate(UIDisplayObject c3) { return c3.LevelString.Length > curLevelLen && c3.LevelString.IndexOf(levelString) == 0; });
            list.Sort();
            return list;
        }

        public UIDisplayObject GetParent(UIDisplayObject uiDisplayObject)
        {
            return GetParent(uiDisplayObject.LevelString);
        }

        public UIDisplayObject GetParent(string levelString)
        {
            //获取本节点的 Parnet .
            string parentLevelString = LevelString.GetParentString(levelString);
            return UiDisplayObjectList.Find(delegate(UIDisplayObject c3) { return c3.LevelString == parentLevelString; });
        }

        public string GetNewChildLevelString(UIDisplayObject uiDisplayObject)
        {
            return GetNewChildLevelString(uiDisplayObject == null ? string.Empty : uiDisplayObject.LevelString);
        }

        public string GetNewChildLevelString(string parentLevel)
        {
            string levelString = string.Empty;
            //获取该对象的列表。
            UIDisplayObject last = GetLastChild(parentLevel);

            string lastChildLevelString = string.Empty;
            if (last != null)
            {
                lastChildLevelString = last.LevelString;
            }

            levelString = LevelString.GetNewChildLevelStringByChildLevel(parentLevel, lastChildLevelString);

            return levelString;
        }


        public List<UIDisplayObject> GetSelectedSuns(string levelString)
        {
            return null;
        }



        public UIDisplayObject CreateTreeStucture()
        {
            UIDisplayObject root = new UIDisplayObject();

            foreach (UIDisplayObject item in UiDisplayObjectList)
            {
                UIDisplayObject parent = null;

                if (parent == null)
                {
                    root.List.Add(item);
                    item.Parent = root;
                }
                else
                {
                    parent.List.Add(item);
                    item.Parent = parent;
                }
            }
            return root;
        }



        public void Sort()
        {
        }


        public virtual void Display()
        {
            //排序列表。
            foreach (BaseControlAdapter item in ControlAdapterList)
            {
                item.Display();
            }

            if (Current == null)
            {
                First();
            }
        }

        public virtual void FindNode(UIDisplayObject uiDisplayObject)
        {
        }

        #endregion


        #region  重设置结点层级串。

        private Hashtable GetChildList(UIDisplayObject uiDisplayObject)
        {
            List<UIDisplayObject> list = GetChild(uiDisplayObject == null ? string.Empty : uiDisplayObject.LevelString);

            //1. 将当前所有节点 的所以子点节点，存入 hash表中。
            //这步操作必须 先做，固定好各个节点及子节点的父子关系 ，避免以后插入节点时，会打乱这个父子关系 。
            Hashtable childList = new Hashtable();

            foreach (UIDisplayObject item in list)
            {
                List<UIDisplayObject> l = GetAllChild(item.LevelString);
                childList.Add(item, l);
            }

            return childList;
        }

        /// <summary>
        /// 更换所选节点的 levelstring , 除了更新它本身外，它所有子节点的前缀也都要更新。
        /// </summary>
        /// <param name="uiDisplayObject"></param>
        /// <param name="levelString"></param>
        protected void UpdateLevelStringOfParentLevelString(UIDisplayObject uiDisplayObject, string levelString, List<UIDisplayObject> childList)
        {
            int len = levelString.Length;
            if (childList != null)
            {
                foreach (UIDisplayObject item in childList)
                {
                    //更新所有 levelstring 的前缀。
                    string beginPart = levelString;
                    string endPart = item.LevelString.Substring(len, item.LevelString.Trim().Length - len);

                    item.LevelString = string.Format("{0}{1}", beginPart, endPart);

                    (item.RealObject as IDisplayUIObject).Load(item);
                }
            }

            uiDisplayObject.LevelString = levelString;
            (uiDisplayObject.RealObject as IDisplayUIObject).Load(uiDisplayObject);
        }


        private void ResetLevelString(Hashtable childList, List<UIDisplayObject> list, UIDisplayObject parent)
        {
            //更新一个结点的 levelstring 时 ，它所有的子结点的 levelstring 都要变化 。
            string parentLevelString = parent == null ? string.Empty : parent.LevelString.ToString();// LevelString.GetParentString(curUiDisplayObject.LevelString);
            string lastChildLevelString = string.Empty;

            //更新此列表下的所有层级串。
            //不能一条一条的更新，只能取回所有的明细，再依次更新，否则更新会出错。
            foreach (UIDisplayObject item in list)
            {
                string newLevelString = LevelString.GetNewChildLevelStringByChildLevel(parentLevelString, lastChildLevelString);

                if (item.LevelString != newLevelString)
                {
                    UpdateLevelStringOfParentLevelString(item, newLevelString, childList[item] as List<UIDisplayObject>);
                }
                lastChildLevelString = newLevelString;
            }
        }

        #endregion

        public void Insert(UIDisplayObject curUiDisplayObject, UIDisplayObject newUiDisplayObject)
        {
            UIDisplayObject parent = GetParent(curUiDisplayObject);
            List<UIDisplayObject> childList = GetChild(parent);

            //1. 将当前所有节点 的所以子点节点，存入 hash表中。
            //这步操作必须 先做，固定好各个节点及子节点的父子关系 ，避免以后插入节点时，会打乱这个父子关系 。
            Hashtable childListHashTable = GetChildList(parent);

            //1. 在临时的队列中先行插入，然后重新计算层级串。
            int index = curUiDisplayObject == null ? 0 : childList.IndexOf(curUiDisplayObject); //没有当前节点，序号直接用0 。
            childList.Insert(index, newUiDisplayObject);

            ResetLevelString(childListHashTable, childList, parent);

            //3. 获取基类 在全列表中的 index , 插入到列表中。
            index = curUiDisplayObject == null ? 0 : UiDisplayObjectList.IndexOf(curUiDisplayObject);

            //插入列表。
            Insert(index, newUiDisplayObject);

            //4. 刷新界面。
            foreach (BaseControlAdapter item in ControlAdapterList)
            {
                item.InsertUiDisplayObject(newUiDisplayObject, curUiDisplayObject);
            }

            //设置当前结点。
            Current = newUiDisplayObject;
        }

        #region IList 成员

        public void AddChild()
        {
            //获取当前结点。
            if (Current == null)
            {
                return;
            }

            string levelString = Current.LevelString;
            string childLevelString = LevelString.GetNewChildLevelString(levelString);

            //根据当前结点获取新对象的 层级串。
            UIDisplayObject newUI = new UIDisplayObject();
            newUI.LevelString = childLevelString;

            //再创建新的对象，由用户进行编辑 。将对象加入到它的子列表中的最后一个。
            //真实对象由业务对象创建，并且由用户编辑 。
            //调用内置对象，对象加入到队列中。
            Add(newUI);
        }

        public int Add(object value)
        {
            //是否允许增加由附加件类或事件决定 。
            UIDisplayObject cur = Current;

            //新增列表中有二种方式，一种是 选择用户编辑对象后再增加入列表 。 别一种是 增加到列表后再由用户编辑 
            UIDisplayObject ui = ConverToUiDisplayObject(value);
            UiDisplayObjectList.Add(ui);
            UiDisplayObjectList_New.Add(ui);

            UiDisplayObjectList.Sort();
            UiDisplayObjectList_New.Sort();

            //刷新界面。
            foreach (BaseControlAdapter item in ControlAdapterList)
            {
                item.NewUiDisplayObject(ui);
            }

            //定位新增的树节点为当前节点。
            Current = ui;

            return UiDisplayObjectList.Count - 1;
        }

        public void Insert(int index, object value)
        {
            //是否允许插入可以附加控件类或事件决定 。

            UIDisplayObject ui = ConverToUiDisplayObject(value);
            UiDisplayObjectList.Insert(index, ui);
            UiDisplayObjectList_New.Add(ui);

            //插入点以后的 levelstring 全部要更新。


            //获取该节的列表。


            UiDisplayObjectList.Sort();
            UiDisplayObjectList_New.Sort();



        }

        public void Remove(object value)
        {
            //是否真的删除可以附加控制类或事件决定 。
            UIDisplayObject ui = value as UIDisplayObject;

            if (ui == null)
            {
                //对象为空时退出。
                return;
            }

            //如果有子结点，则不允许直接删除。
            //删除含子结点时，需要处理其它包含的子节点 。
            if (GetChild(ui.LevelString).Count > 0)
            {
                return;
            }

            UIDisplayObject parentUI = GetParent(ui);

            //此处引入事件或拦截类，由最终用户来控制。
            bool flag = true;

            if (flag)
            {
                List<UIDisplayObject> childList = GetChild(parentUI);

                //1. 将当前所有节点 的所以子点节点，存入 hash表中。
                //这步操作必须 先做，固定好各个节点及子节点的父子关系 ，避免以后插入节点时，会打乱这个父子关系 。
                Hashtable childListHashTable = GetChildList(parentUI);

                childList.Remove(ui);

                //删除结点后，要重排该结点这一级的层级串
                ResetLevelString(childListHashTable, childList, parentUI);


                //在列表中删除 ，并做相应处理。
                UiDisplayObjectList.Remove(ui);
                if (UiDisplayObjectList_New.Contains(ui))
                {
                    UiDisplayObjectList_New.Remove(ui);
                }
                else
                {
                    UiDisplayObjectList_Delete.Add(ui);
                }

                //刷新界面。
                foreach (BaseControlAdapter item in ControlAdapterList)
                {
                    item.DelUiDisplayObject(ui);
                }

                //设定当前结点。
            }

        }

        public void RemoveAt(int index)
        {
            UIDisplayObject ui = UiDisplayObjectList[index];

            if (ui != null)
            {
                Remove(ui);
            }
            else
            {
                //可考虑抛出异常
            }
        }



        public void Clear()
        {
            UiDisplayObjectList.Clear();
            UiDisplayObjectList_New.Clear();
            UiDisplayObjectList_Modified.Clear();
            UiDisplayObjectList_Delete.Clear();

            //是否也需要刷新界面  ？
            //刷新界面。
            foreach (BaseControlAdapter item in ControlAdapterList)
            {
                item.Clear();
            }

            Current = null;
        }

        public bool Contains(object value)
        {
            bool flag = false;

            foreach (UIDisplayObject item in UiDisplayObjectList)
            {
                if (item.RealObject == value)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        public int IndexOf(object value)
        {
            return UiDisplayObjectList.IndexOf(value as UIDisplayObject);
        }


        public bool IsFixedSize
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }


        public object this[int index]
        {
            get
            {
                return UiDisplayObjectList[index];
            }
            set
            {
                UiDisplayObjectList[index] = value as UIDisplayObject;
            }
        }

        #endregion

        #region ICollection 成员

        public void CopyTo(Array array, int index)
        {
        }

        public int Count
        {
            get { return UiDisplayObjectList.Count; }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable 成员

        public IEnumerator GetEnumerator()
        {
            return UiDisplayObjectList.GetEnumerator();
        }

        #endregion

        #region 定位

        public void First()
        {
            if (Current == null)
            {
                return;
            }

            UIDisplayObject parent = GetParent(Current);
            Current = GetFirstChild(parent);
        }

        public void Last()
        {
            if (Current == null)
            {
                return;
            }

            UIDisplayObject parent = GetParent(Current);
            Current = GetLastChild(parent);
        }

        public void Prior()
        {
            if (Current == null)
            {
                return;
            }

            Current = GetPriorChild(Current);
        }

        public void Next()
        {
            if (Current == null)
            {
                return;
            }

            Current = GetNextChild(Current);
        }

        #endregion

        #region 移动位置
        //在层级结构中，移动位置会涉及相关结点的 层级串的调整。
        //在层级结构中，


        public void MovePrior()
        {
            //在层级结构中，移动位置会涉及相关结点的 层级串的调整。
            //在层级结构中，只能中同级结构中移动位置。


            //移动后需要调整控件的显示。
            if (Current == null)
            {
                return;
            }

            UIDisplayObject cur = Current;
            UIDisplayObject prior = GetPriorChild(Current);

            if (cur == prior)
            {

            }
            else
            {
                //移除要变化的。
                int index = IndexOf(prior);

                if (index >= 0)
                {
                    #region 处理层级串。

                    UIDisplayObject parentUI = GetParent(cur);
                    List<UIDisplayObject> childList = GetChild(parentUI);

                    //1. 将当前所有节点 的所以子点节点，存入 hash表中。
                    //这步操作必须 先做，固定好各个节点及子节点的父子关系 ，避免以后插入节点时，会打乱这个父子关系 。
                    Hashtable childListHashTable = GetChildList(parentUI);

                    int i = childList.IndexOf(prior);
                    childList.Remove(cur);
                    childList.Insert(i, cur);

                    //删除结点后，要重排该结点这一级的层级串
                    ResetLevelString(childListHashTable, childList, parentUI);

                    #endregion

                    UiDisplayObjectList.Remove(cur);
                    UiDisplayObjectList.Insert(index, cur);



                    AllowLauchControlEvent = false;


                    foreach (BaseControlAdapter item in ControlAdapterList)
                    {
                        item.MoveFrior();
                    }

                    AllowLauchControlEvent = true;

                    //强制设置当前位置
                    CurrentChanged(cur);

                }
                else
                {
                    //可老虎触发事件。
                }


                //插入到指定位置。
            }

        }


        public void MoveNext()
        {
            //移动后需要调整控件的显示。
            if (Current == null)
            {
                //可以引出事件通知用户。
                return;
            }

            UIDisplayObject cur = Current;
            UIDisplayObject next = GetNextChild(Current);

            if (cur == next)
            {
                //通知客户。
            }
            else
            {
                int index = IndexOf(next);

                if (index < UiDisplayObjectList.Count - 1)
                {
                    #region 处理层级串。

                    UIDisplayObject parentUI = GetParent(cur);
                    List<UIDisplayObject> childList = GetChild(parentUI);

                    //1. 将当前所有节点 的所以子点节点，存入 hash表中。
                    //这步操作必须 先做，固定好各个节点及子节点的父子关系 ，避免以后插入节点时，会打乱这个父子关系 。
                    Hashtable childListHashTable = GetChildList(parentUI);

                    int i = childList.IndexOf(next);
                    childList.Remove(cur);
                    childList.Insert(i, cur);


                    //删除结点后，要重排该结点这一级的层级串
                    ResetLevelString(childListHashTable, childList, parentUI);

                    #endregion

                    UiDisplayObjectList.Remove(cur);
                    UiDisplayObjectList.Insert(index, cur);



                    UiDisplayObjectList.Remove(cur);
                    UiDisplayObjectList.Insert(index, cur);

                    AllowLauchControlEvent = false;

                    foreach (BaseControlAdapter item in ControlAdapterList)
                    {
                        item.MoveNext();
                    }

                    AllowLauchControlEvent = true;


                    //强制设置当前位置
                    CurrentChanged(cur);
                }
                else
                {
                    //可老虎触发事件。
                }
            }
        }

        #endregion

    }


}
