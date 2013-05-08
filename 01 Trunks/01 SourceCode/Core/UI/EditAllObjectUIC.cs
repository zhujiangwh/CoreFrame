using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Core.Architecure;
using System.Xml.Serialization;
using Core.Common;

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
        }

        public void Initial()
        {
            service = WinApplication.GetInstance().RemotingServer.CreateRemotingInterface<ICommonObjectService>();

            EditAllObjectDisplayUI = EditAllObjectDisplayUIDefine.CreateObject() as IEditAllObjectDisplay;

            EditAllObjectDisplayUI.EditAllObjectUIC = this;

            //初始化进 取回所有 对象。
            EditObjectList =GetAllObject();
        }

        public virtual IList GetAllObject()
        {
            //获取所有的对象 
            EditObjectList = service.GetAllObject();// 取回整个对象列表.

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
                EditObjectList = EditAllObjectDisplayUI.GetObejctList() ;

                //保存对象列表.
                service.SaveAllObject(EditObjectList);



                //界面反馈.

            }

        }

        public virtual void NewObject()
        {
            //创建一个新对象.
            EditObjectList.Add(ObjectTypeDefine.CreateObject());

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
