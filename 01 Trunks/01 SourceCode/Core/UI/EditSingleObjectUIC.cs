using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Architecure;
using Core.Metadata;

namespace Core.UI
{
    /// <summary>
    /// 界面实现接口.
    /// </summary>
    public interface ISingleObjectDisplay
    {
        //显示对象.
        void Dispaly(object obj);

        

        EditSingleObjectUIC CommonUIinteractive { get; set; }


    }

    public class EditSingleObjectUIC : UIinteractive
    {
        public IEntityService service { get; set; }

        /// <summary>
        /// 被编辑的对象.
        /// </summary>
        public ObjectDefine EditedObjectDefine { get; set; }

        public object EditedObject { get; set; }


        public ObjectDefine EditFormDefine { get; set; }

        public ISingleObjectDisplay EditForm { get; set; }

        public EditSingleObjectUIC()
        {
            EditedObjectDefine = new ObjectDefine();

            EditFormDefine = new ObjectDefine();

        }


        public virtual void Initial()
        {
            service = WinApplication.GetInstance().RemotingServer.CreateRemotingInterface<IEntityService>();

            EditForm = EditFormDefine.CreateObject() as ISingleObjectDisplay;

            EditForm.CommonUIinteractive = this;
        }



        /// <summary>
        /// 创建一个新对象.
        /// </summary>
        public void NewObject()
        {
            //创建新对象.
            //EditedObject = EditedObjectDefine.CreateObject();

            BusiEntity entity = EditedObjectDefine.CreateObject() as  BusiEntity;
            entity.EntityCode = "001";
            entity.EntityName = "实体名";


            BusiDataItem item = new BusiDataItem();
            item.Caption = "caption";
            entity.DataItemList.Add(item);


            //在弹出窗口中，编辑新系统。
            //如果保存窗口，则显示 ，否则。
            EditedObject = entity;

            EditForm.Dispaly(entity);

            //


        }

        public void Save()
        {
            service.SaveBusiEntity(EditedObject as BusiEntity);
        }

        public void GetObject(string name)
        {
            EditedObject = service.LoadBusiEntity(name);

            EditForm.Dispaly(EditedObject);


 
        }

    }
}
