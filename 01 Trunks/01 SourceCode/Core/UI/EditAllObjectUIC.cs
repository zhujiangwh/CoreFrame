using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Core.Architecure;
using System.Xml.Serialization;
using Core.Common;
using Core.Serialize.XML;

namespace Core.UI
{
    /// <summary>
    /// 界面实现接口.
    /// </summary>
    public interface IEditAllObjectDisplay
    {
        //显示对象.
        void Dispaly(IList obj);

        IList GetObejctList();

        EditAllObjectUIC EditAllObjectUIC { get; set; }

    }


    /// <summary>
    /// 在内存中编辑所有对象组的控制类，所有的成员都采用缓存更新的方式最终提交到数据库中。
    /// 界面 新创建的对象 可以采用 弹出 窗口 的方式编辑 ，也可以 在主界面上和界面 同时 显示出来。
    /// 编辑 所有的对象并不一定意味着 是该类型在数据库中的所有对象 ，取多少对象由数据库决定 。
    /// 对象 之间 可以是简单的列表结构 ，也应该可以是树结构 。
    /// 但 所有的对象 应该是 同样的类型。
    /// </summary>
    [Serializable]
    public class EditAllObjectUIC
    {
        /// <summary>
        /// 被编辑的对象,类型
        /// </summary>
        public virtual ObjectDefine ObjectTypeDefine { get; set; }

        /// <summary>
        /// 被编辑的对象.
        /// </summary>
        public virtual ObjectDefine EditAllObjectDisplayUIDefine { get; set; }

        [XmlIgnore]
        public virtual IEditAllObjectDisplay EditAllObjectDisplayUI { get; set; }

        [XmlIgnore]
        public virtual IList EditObjectList { get; set; }

        public ICommonObjectService service { get; set; }

        public EditAllObjectUIC()
        {
            EditAllObjectDisplayUIDefine = new ObjectDefine();
            ObjectTypeDefine = new ObjectDefine();

            EditObjectList = new ArrayList();
        }

        public void Initial()
        {
            //创建远程服务接口。
            //service = CreateService();

            //创建 界面 编辑 窗口。
            EditAllObjectDisplayUI = EditAllObjectDisplayUIDefine.CreateObject() as IEditAllObjectDisplay;
            EditAllObjectDisplayUI.EditAllObjectUIC = this;

            //初始化进 取回所有 对象 ， 这个对象可以由外界替换。。

            IList list = GetAllObject();

            if (list != null)
            {
                EditObjectList = GetAllObject();
            }

        }

        public virtual ICommonObjectService CreateService()
        {
            return WinApplication.GetInstance().RemotingServer.CreateRemotingInterface<ICommonObjectService>(); ;
        }

        public virtual object CreateObject()
        {
            return ObjectTypeDefine.CreateObject();
        }



        public virtual IList GetAllObject()
        {
            //这个地方得根据具体类取得。
            EditObjectList = service.GetAllObject(ObjectTypeDefine.FullClassName);



            if (EditAllObjectDisplayUI != null)
            {
                EditAllObjectDisplayUI.Dispaly(EditObjectList);
            }

            return EditObjectList;
        }

        public virtual void Save()
        {
            //从界面取回对象列表.

            if (EditAllObjectDisplayUI != null)
            {
                EditObjectList = EditAllObjectDisplayUI.GetObejctList();

                //保存对象列表.
                service.SaveAllObject(ObjectTypeDefine.FullClassName,EditObjectList);

                //界面反馈.
            }

        }

        public virtual void NewObject()
        {
            //创建一个新对象.
            //创建 对象 后 编辑 对象有两种 方式。
            // 一种是创建 的对象 选编辑 完成 后再 增加到对象 列表中。
            // 另一种方式是 创建对象后 直接 增加到对象表表中再在界面 上编辑 。


            if (EditObjectList == null)
            {
                EditObjectList = new ArrayList();
            }
            EditObjectList.Add(CreateObject());

            //界面刷新 .
            if (EditAllObjectDisplayUI != null)
            {
                EditAllObjectDisplayUI.Dispaly(EditObjectList);
            }

        }

        public virtual void DeleteObject(object delObject)
        {
            //在列表中删除这个对象.

            //界面刷新
        }


    }
}
