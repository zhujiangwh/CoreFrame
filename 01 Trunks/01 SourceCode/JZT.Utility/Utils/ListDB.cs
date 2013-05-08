using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JZT.Utility
{
    public enum UpdateStatus
    {
        Original,Update,New,Delete
    }

    [Serializable]
    public class ListDB
    {
        private  ArrayList sourceList ;

        private ArrayList newList;
        private ArrayList deleteList;
        private ArrayList updateList;

        public ListDB()
        {
            sourceList = new ArrayList();

            newList = new ArrayList();
            deleteList = new ArrayList();
            updateList = new ArrayList();
        }

        public void Load( ArrayList source)
        {
            sourceList.AddRange(source);
        }

        public void Clear()
        {
            sourceList.Clear();
            newList.Clear();
            updateList.Clear();
            deleteList.Clear();
        }

        public ArrayList GetNewList()
        {
            return newList;
        }

        public ArrayList GetUpdateList()
        {
            return updateList;
        }

        public ArrayList GetDeleteList()
        {
            return deleteList;
        }


        #region 更新操作。

        public void Add(object obj)
        {
            //对象列表中必须不存在，同一对象不能重复增加。
            if (!sourceList.Contains(obj))
            {
                sourceList.Add(obj);

                newList.Add(obj);
            }
            else
            {
                throw new Exception("列表中已存在该对象 !");
            }
        }

        public void Remove(object obj)
        {
            //更新列表中必须存在。
            if (sourceList.Contains(obj))
            {
                sourceList.Remove(obj);

                //如果是新增对象，则删除新增列表。
                if (newList.Contains(obj))
                {
                    newList.Remove(obj);
                }
                else
                {
                    //如果对象曾经更新过，则删除更新列表。
                    if (updateList.Contains(obj))
                    {
                        updateList.Remove(obj);
                    }
                    //删除数据源，并增加到删除列表。
                    deleteList.Add(obj);
                }
            }
            else
            {
                throw new Exception("列表中不存在该对象 !");
            }
        }

        public void Update(object obj)
        {
            //更新列表中必须存在。
            if (sourceList.Contains(obj))
            {
                if (newList.Contains(obj))
                {
                    //如果是新建的对象，则不用加到更新列表中。
                }
                else
                {
                    //如果 更新列表中不存在则加入到更新列表中。
                    if (!updateList.Contains(obj))
                    {
                        //首次被更新则记录到更新列表。
                        updateList.Add(obj);
                    }
                }
            }
            else
            {
                throw new Exception("列表中不存在该对象 !");
            }
        }

        #endregion

        public ListDB GetChange()
        {
            ListDB list = new ListDB();
            list.newList = newList;
            list.updateList = updateList;
            list.deleteList = deleteList;

            return list;
        }
    }
}
